using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dictionary : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //오픈 키

    //애니메이션
    [SerializeField]
    private Animator anim;
    private bool pageMoving = false;
    private bool animDone = false;

    //(온/오프)
    [SerializeField]
    private GameObject bookObj;
    [SerializeField]
    private Transform bookPanel;    //노트 패널(UI)

    //노트 데이터
    private List<WordData> wordDatas;       //획득한 단어 데이터
    private List<WordBookData> bookDatas;   //단어 노트 칸
    private int bookIndex = 0;              //단어 노트 칸 index

    private bool isOpen = false;
    public bool IsOpen { get { return isOpen; } set { isOpen = value; } }

    //페이지
    private int pageIndex = 0;
    private List<BookPage> pages;

    //퍼즐 풀이 중에 사용하는 변수들
    public bool isSolving = false;   //퍼즐 풀이 중인가?
    private bool isMoving = false;   //사전이 움직이고 있는 중인가?

    private float smoothTime = 0.3f;
    private float yVelocity = 5.0f;

    //사전 위치
    //private Vector3 basePos;       //기본 위치
    //private Quaternion baseRot;    //기본 회전값

    //
    private float baseYPos;    //올라오는 위치
    private float closeYPos;   //내려가는 위치
    public float offset = 0.0005f;

    //이벤트
    public UnityEvent onBookOpen;
    public UnityEvent onBookClose;

    private void Awake()
    {
        wordDatas = new List<WordData>();
        bookDatas = new List<WordBookData>();
        bookDatas.AddRange(bookPanel.GetComponentsInChildren<WordBookData>());
        if (isOpen == false) { ClosePanel(); }

        //페이지
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

    #region 사전 온/오프
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
    #endregion

    #region 퍼즐 풀이 중일때
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
        //if (isMoving) { return; }
        StopAllCoroutines();
        StartCoroutine(UPBook());
    }
    private IEnumerator UPBook()
    {
        isMoving = true;
        float yPos = bookPanel.transform.localPosition.y;
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
        //if (isMoving) { return; }
        StopAllCoroutines();
        StartCoroutine(DownBook());
    }
    private IEnumerator DownBook()
    {
        isMoving = true;
        float yPos = bookPanel.transform.localPosition.y;
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

    #endregion

    #region 페이지 넘기기
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
    #endregion

    #region 3스테이지 전용
    

    #endregion
}
