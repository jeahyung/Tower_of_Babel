using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPuzzleGrid : MonoBehaviour
{
    private CrossPuzzleGridMgr mgr_Grid;

    [SerializeField]
    private int gridIndex;
    [SerializeField]
    private string wordPiece = "";
    [SerializeField]
    private GameObject puzzlePiece = null; //퍼즐 조각 오브젝트

private void Awake()
    {
        mgr_Grid = GetComponentInParent<CrossPuzzleGridMgr>();
    }
    public bool SetWordPiece(string piece, GameObject objPiece)
    {
        if(puzzlePiece != null) { return false; }

        wordPiece = piece;
        puzzlePiece = objPiece;
        mgr_Grid.AddWordPiece(gridIndex, wordPiece);
        return true;
    }

    public void ResetGrid()
    {
        if(puzzlePiece == null) { return; }
        wordPiece = "";
        mgr_Grid.ResetGrid(gridIndex);
        puzzlePiece = null;
    }
}
