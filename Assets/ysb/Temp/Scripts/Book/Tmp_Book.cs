using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Tmp_Book : MonoBehaviour
{
    [SerializeField]
    private Transform mainCam;
    private Animator anim;
    private bool animDone = false;

    //(��/����)
    private Transform bookObj;      //��Ʈ ������Ʈ
    [SerializeField]
    private Transform bookPanel;    //��Ʈ �г�(UI)

    private List<WordData> wordDatas = new List<WordData>();    //ȹ���� �ܾ� ������
    private List<WordBookData> bookDatas = new List<WordBookData>();    //�ܾ� ��Ʈ ĭ
    private int bookIndex = 0;    //�ܾ� ��Ʈ ĭ index

    private bool isOpen = false;

    //������
    private int pageIndex = 0;
    private List<BookPage> pages;

    //�̺�Ʈ
    public UnityEvent onBookOpen;
    public UnityEvent onBookClose;

    //���� Ǯ�� �߿� ����ϴ� ������
    private GameObject openBtn;
    private GameObject closeBtn;

    public float smoothTime = 0.3F;
    private float yVelocity = 5.0F;

    //���� ��ġ
    private Vector3 basePos;       //�⺻ ��ġ
    private Quaternion baseRot;    //�⺻ ȸ����

    //
    private float upPos;    //�ö���� ��ġ
    private float downPos;  //�������� ��ġ
    public float sTime = 0.0005f;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();

        bookObj = this.transform;
        bookDatas.AddRange(bookPanel.GetComponentsInChildren<WordBookData>());
        if(isOpen == false) { ClosePanel(); }

        //������
        pages = new List<BookPage>();
        pages.AddRange(bookPanel.GetComponentsInChildren<BookPage>());
        pages[0].OnPage();

        //���� ����
        openBtn = bookPanel.transform.Find("OpenBtn").gameObject;
        closeBtn = bookPanel.transform.Find("CloseBtn").gameObject;
        openBtn.SetActive(false);
        closeBtn.SetActive(false);

        //���� ��ġ
        basePos = mainCam.position + new Vector3(-2.5f, -2f, 2.5f);     //�ʱ� ��ġ
        baseRot = mainCam.rotation;
    }
    private void Start()
    {
        //Load
        LoadBookData();
    }
    public void SaveBookData()
    {
        SaveAndLoad.SaveBookData(bookDatas);
    }
    private void LoadBookData()
    {
        BookData bookData = SaveAndLoad.LoadBookData();
        if (bookData == null) { return; }

        int i = 0;
        foreach (var book in bookDatas)
        {
            if (bookData.words[i] == null) { break; }
            book.LoadData(bookData.words[i], bookData.memos[i], bookData.meanings[i]);
            wordDatas.Add(bookData.words[i]);
            bookIndex++;
            i++;
        }
        Debug.Log("�ε� �Ϸ�");
    }

    //====================================== ���� �߰�
    public void AddWordList(List<WordData> words)   //���� �� �߰�
    {
        foreach (WordData word in words)
        {
            if (wordDatas.Contains(word) == true) { continue; }
            if (bookIndex >= bookDatas.Count) { break; }
            wordDatas.Add(word);
            bookDatas[bookIndex].AddWord(word);
            bookIndex++;
        }
    }

    public void AddWordMeaning(List<int> meanings)
    {
        foreach (WordBookData word in bookDatas)
        {
            word.AddMeaning(meanings);
        }
    }

    //====================================== ���� ��/����
    public void OpenOrClose()
    {
        isOpen = !isOpen;
        if (isOpen) { OpenPanel(); }
        else { ClosePanel(); }
    }

    public void OpenPanel()
    {
        bookObj.position = mainCam.position + new Vector3(-2.5f, -2f, 2.5f);
        onBookOpen.Invoke();
    }
    public void ClosePanel()
    {
        bookObj.position = new Vector3(0f, -100f, 0f);
        onBookClose.Invoke();
    }

    //====================================== ���� Ǯ�� ��
    public void SetBookObject_Open(float objSize, Vector3 pos, float upos = 0)
    {
        onBookOpen.Invoke();
        openBtn.SetActive(true);
        //���� ����
        bookObj.position = pos;
        bookObj.eulerAngles = new Vector3(0, 0, 0);
        bookObj.localScale = new Vector3(objSize, objSize, objSize);
        upPos = upos;       //�ö�� ��ġ
        downPos = pos.y;    //������ ��ġ
    }

    public void SetBookObject_Close()
    {
        //����
        bookObj.rotation = baseRot;
        bookObj.localScale = new Vector3(1f, 1f, 1f);

        BtnOff();
        ClosePanel();
    }
    public void BtnOff()    //��ư off
    {
        openBtn.SetActive(false);
        closeBtn.SetActive(false);
    }

    //====================================== ��
    public void UpBookObject()
    {
        openBtn.SetActive(false);
        StartCoroutine(UPBook());
    }
    private IEnumerator UPBook()
    {
        while (true)
        {
            float yPos = Mathf.SmoothDamp(bookObj.position.y, upPos, ref yVelocity, smoothTime);
            bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
            if (yPos >= (upPos - sTime))
            {
                yPos = upPos;
                bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
                closeBtn.SetActive(true);
                yield break;
            }
            yield return null;
        }
    }

    //====================================== �ٿ�
    public void DownBookObject()
    {
        closeBtn.SetActive(false);
        StartCoroutine(DownBook());
    }
    private IEnumerator DownBook()
    {
        while (true)
        {
            float yPos = Mathf.SmoothDamp(bookObj.position.y, downPos, ref yVelocity, smoothTime);
            bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
            if (yPos <= (downPos + sTime))
            {
                yPos = downPos;
                bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
                openBtn.SetActive(true);
                yield break;
            }
            yield return null;
        }
    }

    //====================================== ������ �ѱ�
    public void onNextPage()
    {
        if(animDone == true || pageIndex >= pages.Count - 1) { return; }
        anim.SetTrigger("NextPage");
        StartCoroutine(NextPage());
    }
    private IEnumerator NextPage()
    {
        pages[pageIndex].OffPage();
        while (true)
        {
            if(animDone == true)
            {
                SetNextPage();
                animDone = false;
                yield break;
            }
            yield return null;
        }
    }
    
    public void onPreviousPage()
    {
        if (animDone == true || pageIndex <= 0) { return; }
        anim.SetTrigger("PreviousPage");
        StartCoroutine(PreviousPage());
    }
    private IEnumerator PreviousPage()
    {
        pages[pageIndex].OffPage();
        while (true)
        {
            if(animDone == true)
            {
                SetPreviousPage();
                animDone = false;
                yield break;
            }
            yield return null;
        }
    }
    public void CheckAnimEnd()
    {
        animDone = true;
    }

    private void SetNextPage()
    {
        if(pageIndex >= pages.Count - 1) { return; }
        pageIndex++;
        pages[pageIndex].OnPage();
    }
    private void SetPreviousPage()
    {
        if(pageIndex <= 0) { return; }
        pageIndex--;
        pages[pageIndex].OnPage();
    }
}
