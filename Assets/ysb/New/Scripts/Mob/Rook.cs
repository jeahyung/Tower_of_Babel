using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : MonoBehaviour
{
    public Map map;

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    public Tile curTile = null;
    private void Start()
    {
        map = FindObjectOfType<Map>();

        curTile = map.GetTile(map.tiles[startX, startY].coord);
        curTile.tileType = TileType.impossible;

        Vector3 pos = new Vector3(curTile.GetPosition().x, transform.position.y, curTile.GetPosition().z);
        transform.position = pos;

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f, 1 << LayerMask.NameToLayer("Tile")))
        //{
        //    if (hit.collider.TryGetComponent(out Tile tile))
        //    {
        //        curTile = tile;
        //        tile.tileType = TileType.impossible;
        //    }
        //}
    }

    public void OpenRook()
    {
        curTile.tileType = TileType.possible;
        gameObject.SetActive(false);
    }
}
