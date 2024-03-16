using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameWordPuzzleManager : MonoBehaviour
{

    //타이머
    private UIManager manager_UI;
    [SerializeField]
    private float maxTime = 1000;
    private float timer = 0;
    private float speed = 1f;



    [SerializeField]
    private WordData[] wordDatas;

    private List<WordData> array = new List<WordData>();
    private List<WordData> question = new List<WordData>();    //문제
    private List<WordData> answer = new List<WordData>();      //답

    private bool isRight = false;

    private void Awake()
    {
        manager_UI = FindObjectOfType<UIManager>();

        array.AddRange(wordDatas);

        int count = array.Count;
        for(int i = 0; i < count; ++i)
        {
            WordData word = array[Random.Range(0, array.Count)];
            array.Remove(word);
            question.Add(word);
        }

        StartPuzzle();
    }

    public void StartPuzzle()
    {
        timer = maxTime;
        manager_UI.ActiveTimer();

        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while(true)
        {
            timer -= Time.deltaTime * speed;
            manager_UI.SetTimer(timer / maxTime);
            yield return null;
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

    //타이머
    public void OpenBook()
    {
        speed = 0.3f;
    }
    public void CloseBook()
    {
        speed = 1.0f;
    }
}
