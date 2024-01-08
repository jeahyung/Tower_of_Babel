using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word", order = int.MaxValue)]
public class WordData : ScriptableObject
{
    public int wordId;  //문자 구분용
    public Sprite wordImg;
    public string wordMean;

    public List<string> piece;
}
