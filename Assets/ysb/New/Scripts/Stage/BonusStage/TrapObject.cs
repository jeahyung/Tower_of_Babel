using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour
{
    public Map map;
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    private void Start()
    {
        map = FindObjectOfType<Map>();

        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);

        Vector3 pos = new Vector3(curTile.GetPosition().x,
            curTile.GetPosition().y + 2, curTile.GetPosition().z);
        transform.position = pos;
    }
}
