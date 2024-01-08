using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPuzzlePiece : MonoBehaviour
{
    [SerializeField]
    private Transform baseParent;   //기본 부모
    private Transform curParent;    //현재 부모
    private Vector3 basePosition;   //기본 위치

    private WordPiece wordPiece;
    [SerializeField]
    private GameObject grid = null;
    [SerializeField]
    private string piece;   //글자 조각
    public string Piece => wordPiece.meaning;

    private void Awake()
    {
        wordPiece = new WordPiece(piece);
        baseParent = this.transform.parent;
        basePosition = transform.position;
    }
    public void SetPos()
    {
        float distance = Camera.main.WorldToScreenPoint(transform.position).z;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position =objPos;
    }
    public void SetPos(float xPos, float zPos)
    {
        transform.position = new Vector3(xPos, transform.position.y, zPos);
    }
    public void FindGrid()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, 0.8f);
        foreach(Collider c in col)
        {
            if(c.gameObject == this.gameObject) { continue; }
            if(c.CompareTag("PuzzleGrid"))
            {
                grid = c.gameObject;

                CrossPuzzleGrid pGrid = grid.GetComponent<CrossPuzzleGrid>();
                if(pGrid == null) { continue; }

                bool isAdd = pGrid.SetWordPiece(piece, this.gameObject);
                if (isAdd == false) 
                {
                    ResetPuzzle();
                    break; 
                }
                //위치 설정
                curParent = c.transform;
                transform.SetParent(curParent);
                transform.localPosition = Vector3.zero;
                break;
                //SetPos(c.transform.position.x, c.transform.position.z);
            }
        }
    }
    public void ResetGrid()
    {
        if (grid == null) { return; }
        grid.SendMessage("ResetGrid");
        grid = null;

        transform.SetParent(baseParent);
    }
    public void ResetPuzzle()
    {
        SetPos(basePosition.x, basePosition.z);
        grid = null;
        transform.SetParent(baseParent);
    }
}
