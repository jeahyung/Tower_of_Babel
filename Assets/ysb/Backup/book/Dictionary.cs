using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dictionary : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //���� Ű

    //�ִϸ��̼�
    [SerializeField]
    private Animator anim;
    private bool pageMoving = false;
    private bool animDone = false;

    //(��/����)
    [SerializeField]
    private GameObject bookObj;
    [SerializeField]
    private Transform bookPanel;    //��Ʈ �г�(UI)

    //��Ʈ ������
    private List<WordData> wordDatas;       //ȹ���� �ܾ� ������
    private List<WordBookData> bookDatas;   //�ܾ� ��Ʈ ĭ
    private int bookIndex = 0;              //�ܾ� ��Ʈ ĭ index

    private bool isOpen = false;

    //������
    private int pageIndex = 0;
    private List<BookPage> pages;

    //���� Ǯ�� �߿� ����ϴ� ������
    public bool isSolving = false;   //���� Ǯ�� ���ΰ�?
    private bool isMoving = false;   //������ �����̰� �ִ� ���ΰ�?

    private float smoothTime = 0.3f;
    private float yVelocity = 5.0f;

    //���� ��ġ
    //private Vector3 basePos;       //�⺻ ��ġ
    //private Quaternion baseRot;    //�⺻ ȸ����

    //
    private float baseYPos;    //�ö���� ��ġ
    private float closeYPos;   //�������� ��ġ
    public float offset = 0.0005f;

    //�̺�Ʈ
    public UnityEvent onBookOpen;
    public UnityEvent onBookClose;

    private void Awake()
    {
        wordDatas = new List<WordData>();
        bookDatas = new List<WordBookData>();
        bookDatas.AddRange(bookPanel.GetComponentsInChildren<WordBookData>());
        if (isOpen == false) { ClosePanel(); }

        //������
        pages = new List<BookPage>();
        pages.AddRange(bookPanel.GetComponentsInChildren<BookPage>());
    }
    private void Start()
    {
        //Load
        //LoadBookData();

        pages[0].OnPage();
    }
    private void Update()
    {
        //��Ʈ ����/Ŭ����
        if (Input.GetKeyDown(OpenKey))
        {
            OpenOrClose();
        }
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
        if (isSolving)
        {
            if (isOpen) { UpBookPanel(); }
            else { DownBookPanel(); }
        }
        else
        {
            if (isOpen) { OpenPanel(); }
            else { ClosePanel(); }
        }
    }
    public void OpenPanel()
    {
        if (!isOpen) { return; }
        isOpen = true;
        bookPanel.localScale = new Vector3(1, 1, 1);
        onBookOpen.Invoke();
    }
    public void ClosePanel()
    {
        if (isOpen) { return; }
        isOpen = false;
        bookPanel.localScale = new Vector3(0, 1, 1);
        onBookClose.Invoke();
    }

    //====================================== ���� Ǯ�� ��
    public void SetBookForPuzzle()
    {
        isOpen = false;
        bookPanel.localScale = new Vector3(1, 1, 1);

        isSolving = true;

        baseYPos = 0;
        closeYPos = -Screen.height + (Screen.height / 5);

        bookPanel.transform.localPosition = new Vector3(0, closeYPos, 0);
    }
    
    //up
    public void UpBookPanel()
    {
        if (isMoving) { return; }
        StartCoroutine(UPBook());
    }
    private IEnumerator UPBook()
    {
        isMoving = true;
        float yPos = closeYPos;
        while (true)
        {
            yPos = Mathf.SmoothDamp(yPos, baseYPos, ref yVelocity, smoothTime);
            if (yPos >= (baseYPos - offset))
            {
                isMoving = false;
                yPos = baseYPos;
                bookPanel.transform.localPosition = new Vector3(0, baseYPos, 0);
                yield break;
            }
            bookPanel.transform.localPosition = new Vector3(0, yPos, 0);
            yield return null;
        }
    }

    public void DownBookPanel()
    {
        if (isMoving) { return; }
        StartCoroutine(DownBook());
    }
    private IEnumerator DownBook()
    {
        isMoving = true;
        float yPos = baseYPos;
        while (true)
        {
            yPos = Mathf.SmoothDamp(yPos, closeYPos, ref yVelocity, smoothTime);
            if (yPos <= (closeYPos + offset))
            {
                isMoving = false;
                yPos = baseYPos;
                bookPanel.transform.localPosition = new Vector3(0, closeYPos, 0);
                yield break;
            }
            bookPanel.transform.localPosition = new Vector3(0, yPos, 0);
            yield return null;
        }
    }


    //public void SetBookObject_Open(float objSize, Vector3 pos, float upos = 0)
    //{
    //    onBookOpen.Invoke();
    //    isOpen = false;
    //    isSolving = true;

    //    //���� ����
    //    bookObj.position = pos;
    //    bookObj.eulerAngles = new Vector3(0, 0, 0);
    //    bookObj.localScale = new Vector3(objSize, objSize, objSize);
    //    upPos = upos;       //�ö�� ��ġ
    //    downPos = pos.y;    //������ ��ġ
    //}

    //public void SetBookObject_Close()
    //{
    //    //����
    //    bookObj.rotation = baseRot;
    //    bookObj.localScale = new Vector3(1f, 1f, 1f);

    //    isSolving = false;
    //    ClosePanel();
    //}

    //====================================== ��
    //public void UpBookObject()
    //{
    //    if (isMoving) { return; }
    //    StartCoroutine(UPBook());
    //}
    //private IEnumerator UPBook()
    //{
    //    isMoving = true;
    //    while (true)
    //    {
    //        float yPos = Mathf.SmoothDamp(bookObj.position.y, upPos, ref yVelocity, smoothTime);
    //        bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
    //        if (yPos >= (upPos - offsetTime))
    //        {
    //            yPos = upPos;
    //            bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
    //            isMoving = false;
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}

    //====================================== �ٿ�
    //public void DownBookObject()
    //{
    //    if (isMoving) { return; }
    //    StartCoroutine(DownBook());
    //}
    //private IEnumerator DownBook()
    //{
    //    isMoving = true;
    //    while (true)
    //    {
    //        float yPos = Mathf.SmoothDamp(bookObj.position.y, downPos, ref yVelocity, smoothTime);
    //        bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
    //        if (yPos <= (downPos + offsetTime))
    //        {
    //            yPos = downPos;
    //            bookObj.position = new Vector3(bookObj.position.x, yPos, bookObj.position.z);
    //            isMoving = false;
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}

    //====================================== ������ �ѱ�
    public void onNextPage()
    {
        if (isOpen == false || pageMoving != false || pageIndex >= pages.Count - 1) { return; }
        anim.SetTrigger("isNext");
        StartCoroutine(NextPage());
    }
    private IEnumerator NextPage()
    {
        pageMoving = true;
        pages[pageIndex].OffPage();
        while (true)
        {
            if (animDone == true)
            {
                SetNextPage();
                animDone = false;
                pageMoving = false;
                yield break;
            }
            yield return null;
        }
    }
    public void onPreviousPage()
    {
        if (isOpen == false || pageMoving != false || pageIndex <= 0) { return; }
        anim.SetTrigger("isPre");
        StartCoroutine(PreviousPage());
    }
    private IEnumerator PreviousPage()
    {
        pageMoving = true;
        pages[pageIndex].OffPage();
        while (true)
        {
            if (animDone == true)
            {
                SetPreviousPage();
                animDone = false;
                pageMoving = false;
                yield break;
            }
            yield return null;
        }
    }
    public void CheckAnimEnd()
    {
        Debug.Log("end");
        animDone = true;
    }

    private void SetNextPage()
    {
        if (pageIndex >= pages.Count - 1) { return; }
        pageIndex++;
        pages[pageIndex].OnPage();
    }
    private void SetPreviousPage()
    {
        if (pageIndex <= 0) { return; }
        pageIndex--;
        pages[pageIndex].OnPage();
    }
}