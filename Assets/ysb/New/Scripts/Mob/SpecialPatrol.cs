using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialPatrol : MobMovement
{
    [Header("반환점")]
    [SerializeField] private int rotX;  
    [SerializeField] private int rotY;
    private Tile rotTile;

    private Vector2Int baseDir; //원래 이동 방향
    [SerializeField] private Vector2Int moveDirAfterRot;     //움직일 방향(회전 후)

    [SerializeField] private int rangeAfterRot = 2;   //반환점부터 몇 칸?

    public override void InitMob()
    {
        base.InitMob();
        baseDir = moveDir;

        rotTile = map.GetTile(map.tiles[rotX, rotY].coord);
        for (int i = 0; i < rangeAfterRot; ++i)
        {
            Vector2Int nextCoord = rotTile.coord + moveDirAfterRot * (i + 1);
            Tile nextTile = map.GetTile(nextCoord);
            if (nextTile == null) { break; }
            range.Add(nextTile);
        }
    }

    public override void FindNextTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f, 1 << LayerMask.NameToLayer("Tile")))
        {
            if (hit.collider.TryGetComponent(out Tile tile))
            {
                if (tile == rotTile)
                {
                    if (moveDir == baseDir || moveDir == -baseDir) { moveDir = moveDirAfterRot; }
                    else { moveDir = baseDir; }
                }

                curTile = tile;
                Vector2Int nextCoord = curTile.coord + moveDir;
                Tile nextTile = map.GetTile(nextCoord);

                if (nextTile == null || range.Contains(nextTile) == false)
                {
                    moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
                    nextCoord = curTile.coord + moveDir;
                    nextTile = map.GetTile(nextCoord);
                }
                transform.forward = new Vector3(moveDir.y, 0, moveDir.x);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
            }
        }
    }

    protected override void RotateMob(Tile tile)
    {
        if (tile == null) { return; }

        Vector2Int dir = baseDir;
        if (tile == rotTile)
        {
            if (moveDir == baseDir || moveDir == -baseDir) { dir = moveDirAfterRot; }
            else { dir = baseDir; }
        }
        Vector2Int nextCoord = tile.coord + dir;
        Tile nextTile = map.GetTile(nextCoord);

        if (nextTile == null || range.Contains(nextTile) == false)
        {
            dir = new Vector2Int(-dir.x, -dir.y);
            nextCoord = tile.coord + dir;
            nextTile = map.GetTile(nextCoord);
        }
        transform.forward = new Vector3(dir.y, 0, dir.x);
    }
}
