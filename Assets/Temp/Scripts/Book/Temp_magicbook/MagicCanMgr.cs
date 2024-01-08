using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCanMgr : MonoBehaviour
{
    public int wordCount;
    public List<WordData> words = new List<WordData>();

    public bool isMax = false;

    private void Start()
    {
        wordCount = transform.childCount;
        for(int i = 0; i < wordCount; ++i)
        {
            words.Add(null);
        }
    }

    public void AddWord(int i, WordData wdata)
    {
        if(wdata == null || i >= wordCount) { return; }
        words[i] = wdata;

        foreach(WordData wd in words)
        {
            if(wd == null) { return; }
            isMax = true;
        }
    }

    public List<WordData> GetWord()
    {
        return words;
    }

    public void SetPos()
    {
        transform.localPosition = new Vector3(0, -440f, 0f);
    }
}
