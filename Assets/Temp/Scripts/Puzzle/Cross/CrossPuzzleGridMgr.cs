using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrossPuzzleGridMgr : MonoBehaviour
{
    private List<CrossPuzzleGrid> grids = new List<CrossPuzzleGrid>();

    [SerializeField]
    private string[] makeWord;
    [SerializeField]
    private WordData wordData;
    private string word;

    [SerializeField]
    private bool isComplete = false;    //�ϼ��ƴ°�?

    private void Awake()
    {
        grids.AddRange(GetComponentsInChildren<CrossPuzzleGrid>());

        makeWord = new string[transform.childCount];
        ResetGrids();
    }

    public void ResetPuzzle()
    {
        foreach(var g in grids)
        {
            g.ResetGrid();
        }
    }
    private void ResetGrids()   //��� ���� �ʱ�ȭ
    {
        for (int i = 0; i < makeWord.Length; ++i)
        {
            makeWord[i] = "";
        }
        isComplete = false;
    }

    public void ResetGrid(int i) //�� ���ڸ�
    {
        makeWord[i] = "";
        word = "";
        isComplete = false;
    }

    public void AddWordPiece(int i, string piece)
    {
        makeWord[i] = piece;

        //üũ
        foreach(string grid in makeWord) { if (grid == "") return; }
        foreach (string grid in makeWord)
        {
            word += grid;
        }
        Debug.Log(word);
        if(word == wordData.wordMean)
        {
            Debug.Log("���� ����");
            isComplete = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isComplete == false)
        {
            Debug.Log("��������!");
            gameObject.SetActive(false);
        }
    }
}
