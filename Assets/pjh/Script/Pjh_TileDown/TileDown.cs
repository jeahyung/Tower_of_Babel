using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TileDown : MonoBehaviour
{
    public GameObject startTile;
    public Tile tile;
    private Vector3 pos;
    private bool check = true;
    private Map map;
    private Vector2Int start;
    void Start()
    {
        tile = startTile.GetComponent<Tile>();
        check = true;
        pos = startTile.transform.position;
        map = FindObjectOfType<Map>();
        start = new Vector2Int(0, 0);
    }

    public void DownTile()
    {
        

        if ((check) && (map.nowTile.coord != start))
        {
           
            startTile.transform.DOMoveY(startTile.transform.position.y - 2f, 2f).SetEase(Ease.OutQuad);

            check = false;

            this.tile.tileType = TileType.impossible;
        }
        else
            return;

    }
    public void UpTile()
    {
        if (!check)
        {
            Debug.Log("uptile calling");
            startTile.transform.position = pos;

            ChageBool();

            this.tile.tileType = TileType.possible;
        }
        else
            return;
    }
    public void ChageBool()
    {
        if (!check)
        {
            check = !check;
        }
    }
}
