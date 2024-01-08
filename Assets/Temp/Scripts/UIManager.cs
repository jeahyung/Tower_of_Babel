using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject InteractMessage;
    //private Book manager_Book;
    private Tmp_Book manager_Book;
    private void Awake()
    {
        InteractMessage = GameObject.Find("InteractMessage");
        InteractMessage.SetActive(false);

        manager_Book = GameObject.Find("Book").GetComponent<Tmp_Book>();//GameObject.Find("Book").GetComponent<Book>();
    }
    public void ShowInteractMessage(bool active)
    {
        InteractMessage.SetActive(active);
    }

    ///////
    //»çÀü
    ///////
    public void StartPuzzleAndBookOpen()
    {
        OpenBook();
        //manager_Book.MoveBookObject(-900f);
    }
    public void EndPuzzleAndBookClose()
    {
        CloseBook();
        //manager_Book.MoveBookObject(0f);
        //manager_Book.BtnOff();
    }
    public void OpenBook()
    {
        manager_Book.OpenPanel();
    }
    public void CloseBook()
    {
        manager_Book.ClosePanel();
    }
    public void AddWord(List<WordData> words)
    {
        manager_Book.AddWordList(words);
    }
    public void AddMeaning(List<int> meanings)
    {
        manager_Book.AddWordMeaning(meanings);
    }
}
