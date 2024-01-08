using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBook : MonoBehaviour
{
    private Animator anim;
    private Transform bookPanel;    //��Ʈ �г�(UI)

    private List<WordData> wordDatas = new List<WordData>();    //ȹ���� �ܾ� ������
    private List<WordBookData> bookDatas = new List<WordBookData>();    //�ܾ� ��Ʈ ĭ
    private int bookIndex = 0;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        bookPanel = transform.GetChild(0);
        bookDatas.AddRange(bookPanel.GetComponentsInChildren<WordBookData>());
    }

    public void AddWordList(List<WordData> words)   //���� �� �߰�
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
