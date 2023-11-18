using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private KeyCode OpenKey = KeyCode.B;    //���� Ű

    private GameObject InteractMessage;

    //����
    [SerializeField]
    private GameObject BookObj;
    private Tmp_Book manager_Book;

    [SerializeField] private int bookPos_down;  //�ϴܿ� ���� ��
    [SerializeField] private int bookPos_up;    //������ �ҷ����� ��
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
        //��Ʈ ����/Ŭ����
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
    //����
    ///////
    
    //���� Ǯ�� ��
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
    //���� ����
    public void AddWord(List<WordData> words)
    {
        manager_Book.AddWordList(words);
    }
    public void AddMeaning(List<int> meanings)
    {
        manager_Book.AddWordMeaning(meanings);
    }
}
