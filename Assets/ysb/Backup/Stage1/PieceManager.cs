using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    private GameObject panel_Select;

    private List<SelectPiece> pieces;
    private CrossSelectGrid grid = null;

    private void Awake()
    {
        panel_Select = this.gameObject;

        pieces = new List<SelectPiece>();
        pieces.AddRange(GetComponentsInChildren<SelectPiece>());

        for(int i = 0; i < pieces.Count; ++i)
        {
            pieces[i].PieceId = i;
        }
    }

    public void OpenSelectPanel(CrossSelectGrid g)
    {
        panel_Select.transform.localScale = new Vector3(1, 1, 1);

        grid = null;
        grid = g;
    }

    public void AddWord(Sprite img, string mean, SelectPiece piece)
    {
        grid.AddWord(img, mean, piece);
        panel_Select.transform.localScale = new Vector3(0, 1, 1);
    }
}
