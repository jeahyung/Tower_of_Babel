using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { none = -1, possible = 0, impossible = 1, }
public class Tile : MonoBehaviour
{
    public TileType tileType;

    public Vector2Int coord;

    public SpriteRenderer rend;

    //public Map map;



    private void Start()
    {
        //map = FindObjectOfType<Map>();
        HideArea();
        if(tileType == TileType.none)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetTileCoord(int i, int j)
    {
        //coord = new Coord(i, j);
        coord = new Vector2Int(i, j);

        //x = coord.coord.x;
        //y = coord.coord.y;
    }

    public void ShowArea()
    {
        if(tileType != TileType.possible) { return; }
        rend.enabled = true;
    }
    public void HideArea()
    {
        rend.enabled = false;
    }

    public Vector3 GetPosition()
    {
        //if(canMove == false)
        //{
        //    return Vector3.zero;
        //}
        return new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void ChangeTileState(TileType tp)
    {
        tileType = tp;
    }

}
