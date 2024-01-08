using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePuzzlePiece : MonoBehaviour
{
    private RotatePuzzleManager manager_Puzzle;
    private int PieceId;         //조각의 넘버
    private int PieceSideCount;  //조각의 변 개수
    private int PieceDegree;     //조각이 회전하는 각도
    private int PieceNumber;     //조각의 현재 답

    private float targetDegree = 0f;
    [SerializeField]
    private float rotateSpeed = 2f;
    private bool isRotate = false;
    public void SetPiece(RotatePuzzleManager PM, int id, int num)
    {
        manager_Puzzle = PM;
        PieceId = id;
        PieceSideCount = 3;
        PieceNumber = num;

        PieceDegree = 360/PieceSideCount;
    }
    public void StartRotate()
    {
        if(isRotate == true) { return; }

        PieceNumber = PieceNumber < 2 ? PieceNumber + 1 : 0;
        targetDegree = PieceDegree * PieceNumber; //목표 각도
        isRotate = true;
        StartCoroutine(RotatePiece());  //조각 회전
    }

    //실제 회전
    private IEnumerator RotatePiece()
    {
        float degree = 0f;
        while (Mathf.Abs(transform.eulerAngles.y - targetDegree) > 1)
        {
            degree = Mathf.LerpAngle(transform.eulerAngles.y, targetDegree, Time.deltaTime * rotateSpeed);
            if (Mathf.Abs(targetDegree - degree) <= 2){ degree = targetDegree; }
            transform.rotation = Quaternion.Euler(0, degree, 0);
            yield return null;
        }
        isRotate = false;
        manager_Puzzle.SetPuzzleAnswer(PieceId, PieceNumber);
    }
}
