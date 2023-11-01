using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordBookData : MonoBehaviour
{
    private WordData wordData = null;
    private Image wordImage;    //이미지
    private TMP_InputField memoText;    //메모

    //private Image meaningImage;
    //private bool meaningFind = false;   //뜻을 찾았나?
    //public bool Meaning => meaningFind;
    public WordData WordData => wordData;
    public string memo => memoText.text;

    private void Awake()
    {
        wordImage = transform.GetChild(0).GetComponent<Image>();
        memoText = transform.GetChild(1).GetComponent<TMP_InputField>();

        //meaningImage = transform.GetChild(1).GetComponent<Image>();
    }

    //단어 추가
    public void AddWord(WordData word)
    {
        if(wordData != null) { return; }
        wordData = word;
        wordImage.sprite = wordData.wordImg;
    }

    //뜻 추가
    //public void AddMeaning(int id)
    //{
    //    if(wordData == null || wordData.wordId != id) { return; }
    //    meaningImage.sprite = wordData.wordMeaning; //뜻 추가(현재는 이미지)
    //    meaningFind = true;
    //}

    //메모 추가
    public void AddMemo(string m)
    {
        memoText.text = m;
    }

    //로드
    public void LoadData(WordData word, string memo)
    {
        AddWord(word);
        AddMemo(memo);
        //if(meaning == true) { AddMeaning(word.wordId); }
    }
}
