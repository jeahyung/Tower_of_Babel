using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Book : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //오픈 키
    private List<WordData> wordDatas = new List<WordData>();    //획득한 단어 데이터

    private Transform bookObj;  //노트 패널
    private List<WordBookData> bookDatas = new List<WordBookData>();    //단어 노트 칸
    private int bookIndex = 0;  //단어 노트 칸 index

    private bool isOpen = false;

    public UnityEvent onBookOpen;
    public UnityEvent onBookClose;

    //퍼즐 풀이 중에 사용하는 변수들
    private GameObject openBtn;
    private GameObject closeBtn;

    private RectTransform panelPos;
    public float smoothTime = 0.3F;
    private float yVelocity = 1.0F;

    private void Awake()
    {
        bookObj = transform.GetChild(0);    //노트
        bookDatas.AddRange(bookObj.GetComponentsInChildren<WordBookData>());
        if(isOpen == false) { bookObj.localScale = new Vector3(0, 1, 1); }

        panelPos = bookObj.GetComponent<RectTransform>();

        openBtn = bookObj.transform.GetChild(2).gameObject;
        closeBtn = bookObj.transform.GetChild(3).gameObject;
        openBtn.SetActive(false);
        closeBtn.SetActive(false);
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
        if(bookData == null) { return; }

        int i = 0;
        foreach(var book in bookDatas)
        {
            if(bookData.words[i] == null) { break; }
            book.LoadData(bookData.words[i], bookData.memos[i], bookData.meanings[i]);
            i++;
        }
        Debug.Log("로드 완료");
    }
    private void Update()
    {
        //노트 오픈/클로즈
        if(Input.GetKeyDown(OpenKey))
        {
            isOpen = !isOpen;
            if (isOpen) { OpenPanel(); }
            else { ClosePanel(); }
        }
    }

    public void AddWordList(List<WordData> words)   //여러 개 추가
    {
        foreach(WordData word in words)
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
        foreach(WordBookData word in bookDatas)
        {
            word.AddMeaning(meanings);
        }
    }

    public void OpenPanel()
    {
        bookObj.localScale = new Vector3(1, 1, 1);
        onBookOpen.Invoke();
    }
    public void ClosePanel()
    {
        bookObj.localScale = new Vector3(0, 1, 1);
        onBookClose.Invoke();
    }

    /// <summary>
    /// 퍼즐 풀이 중
    /// </summary>
    public void MoveBookObject(float pos)   //사전 위치를 하단으로 이동시킴
    {
        openBtn.SetActive(true);
        panelPos.localPosition = new Vector3(0, pos, 0);
    }

    public void BtnOff()    //버튼 off
    {
        openBtn.SetActive(false);
        closeBtn.SetActive(false);
    }

    //업
    public void UpBookObject()
    {
        openBtn.SetActive(false);
        StartCoroutine(UPBook());
    }
    private IEnumerator UPBook()
    {
        while(true)
        {
            float yPos = Mathf.SmoothDamp(panelPos.localPosition.y, 0, ref yVelocity, smoothTime);
            panelPos.localPosition = new Vector3(panelPos.localPosition.x, yPos, panelPos.localPosition.z);
            if(yPos >= -1)
            {
                yPos = 0;
                panelPos.localPosition = new Vector3(panelPos.localPosition.x, yPos, panelPos.localPosition.z);
                closeBtn.SetActive(true);
                yield break;
            }
            yield return null;
        }
    }

    //다운
    public void DownBookObject()
    {
        closeBtn.SetActive(false);
        StartCoroutine(DownBook());
    }
    private IEnumerator DownBook()
    {
        while (true)
        {
            float yPos = Mathf.SmoothDamp(panelPos.localPosition.y, -900, ref yVelocity, smoothTime);
            panelPos.localPosition = new Vector3(panelPos.localPosition.x, yPos, panelPos.localPosition.z);
            if (yPos <= -899)
            {
                yPos = -900;
                panelPos.localPosition = new Vector3(panelPos.localPosition.x, yPos, panelPos.localPosition.z);
                openBtn.SetActive(true);
                yield break;
            }
            yield return null;
        }
    }
}
