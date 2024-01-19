using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPiece : MonoBehaviour
{
    private PieceManager manager_Piece;
    private int pieceId;

    public string word;
    private Sprite wordImg;
    private Image img;

    public bool isSelected = false;    //
    public int PieceId
    {
        get { return pieceId; }
        set { pieceId = value; }
    }


    public void onSelectPiece()
    {
        if(isSelected == true) { return; }
        manager_Piece.AddWord(wordImg, word, this);
    }
    //===================
    private void Awake()
    {
        img = GetComponent<Image>();
        wordImg = img.sprite;

        manager_Piece = GetComponentInParent<PieceManager>();
        this.GetComponent<Button>().onClick.AddListener(() => onSelectPiece());
    }
    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            isSelected = value;
            if (isSelected == false)
                img.color = new Color(1f, 1f, 1f);
            else
            {
                img.color = new Color(0.5f, 0.5f, 0.5f);
            }
        }
    }

    public string SetWordPieceMean()
    {
        isSelected = true;
        img.color = new Color(0.5f, 0.5f, 0.5f);
        return word;
    }


    public Sprite SetWordPieceImg()
    {
        return wordImg;
    }

    public void SetResult(int i)
    {
        switch (i)
        {
            case -1:
                img.color = new Color(255, 0, 0);
                break;
            case 0:
                img.color = new Color(0, 255, 0);
                break;
            case 1:
                img.color = new Color(0, 0, 255);
                break;
            default:
                img.color = new Color(255, 255, 255);
                break;
        }
    }
}