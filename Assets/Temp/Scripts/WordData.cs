using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word", order = int.MaxValue)]
public class WordData : ScriptableObject
{
    public int wordId;  //���� ���п�
    public Sprite wordImg;
    public string wordMean;

    public List<string> piece;
}
