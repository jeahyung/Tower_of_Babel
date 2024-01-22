using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject InteractMessage;
    [SerializeField]
    private Slider timer;
    //private Book manager_Book;
    //private Tmp_Book manager_Book;  //삭제할 예정

    private Dictionary manager_Dictionary;
    private void Awake()
    {
        InteractMessage = GameObject.Find("InteractMessage");
        InteractMessage.SetActive(false);

        //manager_Book = GameObject.Find("Book").GetComponent<Tmp_Book>();//GameObject.Find("Book").GetComponent<Book>();

        manager_Dictionary = GetComponentInChildren<Dictionary>();
    }
    public void ShowInteractMessage(bool active)
    {
        InteractMessage.SetActive(active);
    }

    ///////
    //사전
    ///////
    public void StartPuzzleAndBookOpen()
    {
        manager_Dictionary.SetBookForPuzzle();

        //manager_Book.MoveBookObject(-900f);
    }
    public void EndPuzzleAndBookClose()
    {
        manager_Dictionary.isSolving = false;
        manager_Dictionary.ClosePanel();

      
        //manager_Book.MoveBookObject(0f);
        //manager_Book.BtnOff();
    }
    public void OpenBook()
    {
        manager_Dictionary.OpenOrClose();
        //manager_Book.OpenPanel();
    }
    public void CloseBook()
    {
        //manager_Book.ClosePanel();
    }
    public void AddWord(List<WordData> words)
    {
        //manager_Book.AddWordList(words);
        manager_Dictionary.AddWordList(words);
    }
    public void AddMeaning(List<int> meanings)
    {
        //manager_Book.AddWordMeaning(meanings);

        manager_Dictionary.AddWordMeaning(meanings);
    }


    //==================================================stage3
    public void ActiveTimer()
    {
        timer.gameObject.SetActive(true);
        timer.value = 1f;
    }
    public void SetTimer(float t)
    {
        timer.value = t;
    }
}
