using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SameWordPuzzleManager : MonoBehaviour
{
    [SerializeField]
    private WordData[] wordDatas;

    private List<WordData> array = new List<WordData>();
    private List<WordData> question = new List<WordData>();    //¹®Á¦
    private List<WordData> answer = new List<WordData>();      //´ä

    private bool isRight = false;

    private void Awake()
    {
        array.AddRange(wordDatas);

        int count = array.Count;
        for(int i = 0; i < count; ++i)
        {
            WordData word = array[Random.Range(0, array.Count)];
            array.Remove(word);
            question.Add(word);
        }
    }

    public void TakeWord(WordData word)
    {
        if(word == null) { return; }
        answer.Add(word);

        if(answer.Count == question.Count)
        {
            CheckAnswer();
        }
    }

    private void CheckAnswer()
    {
        for(int i = 0; i < answer.Count; ++i)
        {
            if(answer[i] != question[i])
            {
                isRight = false;
                ResetPuzzle();
                return;
            }
        }
        isRight = true;
    }

    private void ResetPuzzle()
    {
        answer.Clear();
    }
}
