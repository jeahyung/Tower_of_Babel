using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPiece : MonoBehaviour
{
    [SerializeField]
    private int pieceId;

    public string word;
    public Sprite wordImg;
    public Image img;

    public bool isSelected = false;    //선택됐는가? 선택됐으면 선택 불가
    public int PieceId => pieceId;

    public bool IsSelected
    {
        get { return isSelected; }
        set { 
            isSelected = value;
        if(isSelected == false)
                img.color = new Color(1f, 1f, 1f);
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
        switch(i)
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
