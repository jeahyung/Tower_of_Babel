using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBook : MonoBehaviour
{
    private Animator anim;
    private Transform bookPanel;    //노트 패널(UI)

    private List<WordData> wordDatas = new List<WordData>();    //획득한 단어 데이터
    private List<WordBookData> bookDatas = new List<WordBookData>();    //단어 노트 칸
    private int bookIndex = 0;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        bookPanel = transform.GetChild(0);
        bookDatas.AddRange(bookPanel.GetComponentsInChildren<WordBookData>());
    }

    public void AddWordList(List<WordData> words)   //여러 개 추가
    {
        foreach (WordData word in words)
        {
            if (bookIndex >= bookDatas.Count) { break; }
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
}
