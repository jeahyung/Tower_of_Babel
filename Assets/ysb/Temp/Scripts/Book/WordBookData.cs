using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordBookData : MonoBehaviour
{
    private WordData wordData = null;
    private Image wordImage;    //�̹���
    private TMP_InputField memoText;    //�޸�
    private TMP_Text meaningText;   //��

    //private Image meaningImage;
    private bool meaningFind = false;   //���� ã�ҳ�?
    public bool Meaning => meaningFind;
    public WordData WordData => wordData;
    public string Memo => memoText.text;

    private void Awake()
    {
        wordImage = transform.GetChild(0).GetComponent<Image>();
        memoText = transform.GetChild(1).GetComponent<TMP_InputField>();
        meaningText = transform.GetChild(2).GetComponent<TMP_Text>();

        meaningText.gameObject.SetActive(false);
        memoText.enabled = false;

        //meaningImage = transform.GetChild(1).GetComponent<Image>();
    }

    //�ܾ� �߰�
    public void AddWord(WordData word)
    {
        if(wordData != null) { return; }
        wordData = word;
        wordImage.sprite = wordData.wordImg;
        memoText.enabled = true;
    }

    public void AddMeaning(int id)
    {
        if (wordData == null || wordData.wordId != id) { return; }
        meaningText.text = wordData.wordMean;
        meaningFind = true;
        memoText.gameObject.SetActive(false);
    }

    public void AddMeaning(List<int> id)
    {
        if(wordData == null) { return; }
        foreach(int i in id)
        {
            if (wordData.wordId != i) { continue; }
            meaningText.gameObject.SetActive(true);
            meaningText.text = wordData.wordMean;
            meaningFind = true;
        }

        //�޸� ����� �ƿ�
        memoText.gameObject.SetActive(false);
    }

    //�� �߰�
    //public void AddMeaning(int id)
    //{
    //    if(wordData == null || wordData.wordId != id) { return; }
    //    meaningImage.sprite = wordData.wordMeaning; //�� �߰�(����� �̹���)
    //    meaningFind = true;
    //}

    //�޸� �߰�
    public void AddMemo(string m)
    {
        memoText.text = m;
    }

    //�ε�
    public void LoadData(WordData word, string memo, bool meaning)
    {
        AddWord(word);
        AddMemo(memo);
        if(meaning == true) { AddMeaning(word.wordId); }
    }
}
