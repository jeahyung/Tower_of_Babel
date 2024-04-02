using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMovement : MonoBehaviour
{

    public Map map;
    public Vector2Int curTile;  //현재 위치한 타일
    public Vector2Int moveDir;     //움직일 방향

    public int moveCount = 2;   //움직이는 칸 수
    private int count;
    public bool isEnd = true;  //행동을 종료했는가?

    private void Awake()
    {
        count = moveCount;
    }

    public void Act()
    {
        if(isEnd == false) { return; }
        isEnd = false;

        FindNextTile();
    }
    public void FindNextTile()
    {
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 10f, 1 << LayerMask.NameToLayer("Tile")))
        {
            if(hit.collider.TryGetComponent(out Tile tile))
            {
                curTile = tile.coord;

                Vector2Int nextCoord = curTile + moveDir;
                Tile nextTile = map.GetTile(nextCoord);

                if(nextTile == null)
                {
                    moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
                    nextCoord = curTile + moveDir;
                    nextTile = map.GetTile(nextCoord);
                }
                transform.forward = new Vector3(moveDir.y, 0, moveDir.x);
                //Quaternion.LookRotation(Vector3.forward,);
                StartCoroutine(MoveMob(nextTile));
            }
        }
    }

    private IEnumerator MoveMob(Tile nextTile)
    {
        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);

        while (Vector3.Distance(transform.position, nextPos) >= 0.05f)
        {
            transform.position = Vector3.Slerp(transform.position, nextPos, 0.05f);
            yield return null;
        }
        transform.position = nextPos;

        yield return new WaitForSeconds(0.5f);

        if(--count > 0)
        {
            FindNextTile();
            yield break;
        }

        count = moveCount;
        isEnd = true;
    }
}
