using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Book : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //���� Ű
    private List<WordData> wordDatas = new List<WordData>();    //ȹ���� �ܾ� ������

    private Transform bookObj;  //��Ʈ �г�
    private List<WordBookData> bookDatas = new List<WordBookData>();    //�ܾ� ��Ʈ ĭ
    private int bookIndex = 0;  //�ܾ� ��Ʈ ĭ index

    private bool isOpen = false;

    private void Awake()
    {
        bookObj = transform.GetChild(0);    //��Ʈ
        bookDatas.AddRange(bookObj.GetComponentsInChildren<WordBookData>());
        if(isOpen == false) { bookObj.localScale = new Vector3(0, 1, 1); }
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
            book.LoadData(bookData.words[i], bookData.memos[i]);
            i++;
        }
        Debug.Log("�ε� �Ϸ�");
    }
    private void Update()
    {
        //��Ʈ ����/Ŭ����
        if(Input.GetKeyDown(OpenKey))
        {
            isOpen = !isOpen;
            if (isOpen) { OpenPanel(); }
            else { ClosePanel(); }
        }
    }

    public void AddWordList(List<WordData> words)   //���� �� �߰�
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

    public void OpenPanel()
    {
        bookObj.localScale = new Vector3(1, 1, 1);
    }
    public void ClosePanel()
    {
        bookObj.localScale = new Vector3(0, 1, 1);
    }
   
}
