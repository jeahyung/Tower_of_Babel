using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //오픈 키

    private GameObject InteractMessage;

    //사전
    [SerializeField]
    private GameObject BookObj;
    private Tmp_Book manager_Book;

    [SerializeField] private int bookPos_down;  //하단에 있을 때
    [SerializeField] private int bookPos_up;    //사전을 불러왔을 때
    //private Book manager_Book;

    private bool isSolving = false;

    private void Awake()
    {
        InteractMessage = GameObject.Find("InteractMessage");
        InteractMessage.SetActive(false);

        manager_Book = BookObj.GetComponentInChildren<Tmp_Book>();
        //manager_Book = BookObj.GetComponentInChildren<Book>();
        //manager_Book = GameObject.Find("Book").GetComponent<Book>();
    }
    private void Update()
    {
        //노트 오픈/클로즈
        if (Input.GetKeyDown(OpenKey))
        {
            if(isSolving == true) { return; }
            manager_Book.OpenOrClose();
        }
    }

    public void ShowInteractMessage(bool active)
    {
        InteractMessage.SetActive(active);
    }

    ///////
    //사전
    ///////
    
    //퍼즐 풀이 중
    public void StartPuzzleAndBookOpen(Vector3 pos, float upos, float objSize)
    {
        manager_Book.SetBookObject_Open(objSize, pos, upos);
        isSolving = true;
        //OpenBook();
        //manager_Book.MoveBookObject(bookPos_down, true);
        //manager_Book.MoveBookObject(-900f);
    }
    public void EndPuzzleAndBookClose()
    {
        manager_Book.SetBookObject_Close();
        isSolving = false;
        //CloseBook();
        //manager_Book.MoveBookObject(bookPos_up, false);
        //manager_Book.MoveBookObject(0f);
    }
    //퍼즐 종료
    public void AddWord(List<WordData> words)
    {
        manager_Book.AddWordList(words);
    }
    public void AddMeaning(List<int> meanings)
    {
        manager_Book.AddWordMeaning(meanings);
    }
}
