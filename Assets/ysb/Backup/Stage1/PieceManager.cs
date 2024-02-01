using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    private GameObject panel_Select;

    private List<SelectPiece> pieces;
    private CrossSelectGrid grid = null;

    public GameObject closeBtn;

    private void Awake()
    {
        panel_Select = this.gameObject;

        pieces = new List<SelectPiece>();
        pieces.AddRange(GetComponentsInChildren<SelectPiece>());

        for(int i = 0; i < pieces.Count; ++i)
        {
            pieces[i].PieceId = i;
        }

        CloseSelectPanel();
    }

    public void OpenSelectPanel(CrossSelectGrid g)
    {
        panel_Select.transform.localScale = new Vector3(1, 1, 1);

        grid = null;
        grid = g;

        closeBtn.SetActive(true);
    }
    public void CloseSelectPanel()
    {
        panel_Select.transform.localScale = new Vector3(0, 1, 1);
        closeBtn.SetActive(false);
    }

    public void AddWord(Sprite img, string mean, SelectPiece piece)
    {
        grid.AddWord(img, mean, piece);
        CloseSelectPanel();
    }

    //===================================================Reset
    public void ResetPanel()
    {
        for (int i = 0; i < pieces.Count; ++i)
        {
            pieces[i].IsSelected = false;
        }
    }
}
