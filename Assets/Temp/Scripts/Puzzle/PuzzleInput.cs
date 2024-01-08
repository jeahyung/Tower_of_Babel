using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleInput : MonoBehaviour
{
    public GameObject pm;
    public List<int> id = new List<int>();

    public void SetWord(List<WordData> wd)
    {
        Debug.Log("단어를 받음");
        id.Clear();
        for(int i = 0; i < wd.Count; ++i)
        {
            id.Add(wd[i].wordId);
        }
        pm.SendMessage("SetAnswer", id);
    }
}
