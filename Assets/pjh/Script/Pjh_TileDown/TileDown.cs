using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileDown : MonoBehaviour
{
    public GameObject startTile;
    public Tile tile;

    private bool check = true;

    void Start()
    {
        tile = startTile.GetComponent<Tile>();
        check = true;
    }

    public void DownTile()
    {
        if (check)
        {
            startTile.transform.DOMoveY(startTile.transform.position.y - 2f, 2f).SetEase(Ease.OutQuad);

            check = false;

            this.tile.tileType = TileType.impossible;
        }
        else
            return;

    }
}
