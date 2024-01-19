using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dictionary : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //오픈 키

    [SerializeField]
    private Transform mainCam;
    [SerializeField]
    private Animator anim;
    private bool pageMoving = false;
    private bool animDone = false;

    //(온/오프)
    [SerializeField]
    private GameObject bookObj;
    [SerializeField]
    private Transform bookPanel;    //노트 패널(UI)

    private List<WordData> wordDatas = new List<WordData>();    //획득한 단어 데이터
    private List<WordBookData> bookDatas = new List<WordBookData>();    //단어 노트 칸
    private int bookIndex = 0;    //단어 노트 칸 index

    private bool isOpen = false;

    //페이지
    private int pageIndex = 0;
    private List<BookPage> pages;

    //이벤트
    public UnityEvent onBookOpen;
    public UnityEvent onBookClose;

    //퍼즐 풀이 중에 사용하는 변수들
    public bool isSolving = false; //퍼즐 풀이 중인가?
    private bool isMoving = false;  //사전이 움직이고 있는 중인가?

    public float smoothTime = 0.3F;
    private float yVelocity = 5.0F;

    //사전 위치
    private Vector3 basePos;       //기본 위치
    private Quaternion baseRot;    //기본 회전값

    //
    private float upPos;    //올라오는 위치
    private float downPos;  //내려가는 위치
    public float offsetTime = 0.0005f;

    private void Awake()
    {
        bookDatas.AddRange(bookPanel.GetComponentsInChildren<WordBookData>());
        if (isOpen == false) { ClosePanel(); }

        //페이지
        pages = new List<BookPage>();
        pages.AddRange(bookPanel.GetComponentsInChildren<BookPage>());
        pages[0].OnPage();

        //사전 위치
        basePos = mainCam.position + new Vector3(-2.5f, -2f, 2.5f);     //초기 위치
        baseRot = mainCam.rotation;
    }
    private void Start()
    {
        //Load
        LoadBookData();
    }
    private void Update()
    {
        //노트 오픈/클로즈
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
        Debug.Log("로드 완료");
    }

    //====================================== 문자 추가
    public void AddWordList(List<WordData> words)   //여러 개 추가
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

    //====================================== 사전 온/오프
    public void OpenOrClose()
    {
        isOpen = !isOpen;
        if (isSolving)
        {
            //if (isOpen) { UpBookObject(); }
            //else { DownBookObject(); }
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

    //====================================== 퍼즐 풀이 중
    //public void SetBookObject_Open(float objSize, Vector3 pos, float upos = 0)
    //{
    //    onBookOpen.Invoke();
    //    isOpen = false;
    //    isSolving = true;

    //    //사전 세팅
    //    bookObj.position = pos;
    //    bookObj.eulerAngles = new Vector3(0, 0, 0);
    //    bookObj.localScale = new Vector3(objSize, objSize, objSize);
    //    upPos = upos;       //올라올 위치
    //    downPos = pos.y;    //내려갈 위치
    //}

    //public void SetBookObject_Close()
    //{
    //    //사전
    //    bookObj.rotation = baseRot;
    //    bookObj.localScale = new Vector3(1f, 1f, 1f);

    //    isSolving = false;
    //    ClosePanel();
    //}

    //====================================== 업
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

    //====================================== 다운
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

    //====================================== 페이지 넘김
    public void onNextPage()
    {
        if (!isOpen || pageMoving != false || pageIndex >= pages.Count - 1) { return; }
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

    public void PageNext()
    {
        anim.SetTrigger("isNext");
    }

    public void onPreviousPage()
    {
        if (!isOpen || pageMoving != false || pageIndex <= 0) { return; }
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
