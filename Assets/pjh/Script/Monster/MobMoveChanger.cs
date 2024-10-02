using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class MobMoveChanger : MonoBehaviour, Mob
{

    //x, y과 바꼈다. x = y축 / y = x축 / 1 = 오른쪽,위 / -1 = 왼쪽, 아래
    PatrolMobManager manager_Mob;

    public Map map;
    public Tile curTile;  //현재 위치한 타일
    private Tile startTile;
    public Vector2Int moveDir;     //움직일 방향  //프리팹에서 지정

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    public int moveCount = 2;   //움직이는 칸 수
    public int visualRange = 2;
    private int count;
    public bool isEnd = true;  //행동을 종료했는가?
    public bool isDone = false;

    public List<Tile> range;    //움직일 수 있는 범위
    public int leftRagne = 1;   //왼쪽으로 몇 칸까지?(아래)
    public int rightRange = 1;  //오른쪽으로 몇 칸까지?(위)   //오른쪽을 기준으로 잡는다(-1,1 동일)

    //[SerializeField] private float moveSpeed;

    public bool canAct = true;   //움직일 수 있는가?
    public bool isRope = false; //로프에 걸렸는가? 
    public bool isPatrol = true;

    public int[] arr = {1, -1 };

    private void Awake()
    {
        count = moveCount;
        manager_Mob = GetComponentInParent<PatrolMobManager>();
        map = FindObjectOfType<Map>();
    }

    private void Start() 
    {       
        curTile = map.GetTile(map.tiles[startX, startY].coord);
        startTile = curTile;
        curTile.tileType = TileType.impossible;
        curTile.mob = this.GetComponent<Mob>();
        isPatrol = true;

        Vector3 pos = new Vector3(curTile.GetPosition().x, curTile.GetPosition().y + 3, curTile.GetPosition().z);
        transform.position = pos;

        range = new List<Tile>();
        range.Add(curTile); //현재 칸
        for (int i = 0; i < leftRagne; ++i)
        {
            Vector2Int nextCoord = curTile.coord + -moveDir * (i + 1);
            Tile nextTile = map.GetTile(nextCoord);
            if (nextTile == null) { break; }
            range.Add(nextTile);
        }

        for (int i = 0; i < rightRange; ++i)
        {
            Vector2Int nextCoord = curTile.coord + moveDir * (i + 1);
            Tile nextTile = map.GetTile(nextCoord);
            if (nextTile == null) { break; }
            range.Add(nextTile);
        }
    }


    public void DontMove()
    {
        isRope = true;
    }
    public void Act()
    {
        if (isRope)
        {
            manager_Mob.CheckMobAction();
            isRope = false;
            return;
        }

        if (isEnd == false) { return; }
        isEnd = false;

        if (canAct == false)
        {
            canAct = true;
            return;
        }

        ChooseMobMoving();        
    }

    public void CheckRange()
    {        
        if ((visualRange >= (Mathf.Abs(startX - map.playerTile.coord.x))) && (visualRange >= (Mathf.Abs(startY - map.playerTile.coord.y))) )
        {
            isPatrol = false;
        } 
        else
            isPatrol = true;
    }

    public void ChooseMobMoving()
    {
        CheckRange();

//        if (isPatrol)
       // {
            FindNextTile(); // 순찰형
     //   }
        //  StartCoroutine(MoveMob(nextTile)); 이걸로 동작함
    }

    public void MoveBack(Tile tile)
    {
        Vector2Int dirCheck = startTile.coord - curTile.coord;
        if (dirCheck.x == 0 && dirCheck.y == 0)
        {
            manager_Mob.CheckMobAction();
            return;
        }

        Vector2Int nextCoord;



        if (Mathf.Abs(dirCheck.x) > Mathf.Abs(dirCheck.y))
        {
            curTile = tile;
            Tile nextTile;
            if (0 < dirCheck.x)
            {
                nextCoord = new Vector2Int(curTile.coord.x + 1, curTile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);
                return;
            }
            else if (0 > dirCheck.x)
            {
                nextCoord = new Vector2Int(curTile.coord.x - 1, curTile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);
                return;
            }

        }
        else
        {
            curTile = tile;
            Tile nextTile;
            if (0 < dirCheck.y)
            {
                nextCoord = new Vector2Int(curTile.coord.x, curTile.coord.y + 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);
                return;
            }
            else if (0 > dirCheck.y)
            {
                nextCoord = new Vector2Int(curTile.coord.x, curTile.coord.y - 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);
                return;
            }
        }


    }

    private void CheckStartPoint(Tile tile)
    {
        Vector2Int a, b;
        a = tile.coord;
        b = startTile.coord;

        if (a == b)
        {
            Debug.Log("!!!!!!!!!!!!sameTile!!!!!!!!!!!!!!!!!!!!");
            manager_Mob.CheckMobAction();
        }
    }

    public void FindMoveTile(int targetX, int targetY)
    {

    }

    public void PatrolMove(Tile tile)
    {
        curTile = tile;

        Vector2Int nextCoord = curTile.coord + moveDir;
        Tile nextTile = map.GetTile(nextCoord);

        if (nextTile == null || range.Contains(nextTile) == false)
        {
            moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
            nextCoord = curTile.coord + moveDir;
            nextTile = map.GetTile(nextCoord);
        }
        EffectManage.Instance.PlayEffect("Monster_Move", this.transform.position);
        transform.forward = new Vector3(moveDir.y, 0, moveDir.x);

        tile.tileType = TileType.possible;
        tile.mob = null;
        StartCoroutine(MoveMob(nextTile));
    }

    public void TraceModeMove(Tile tile)
    {
        Vector2Int dirCheck = map.playerTile.coord - curTile.coord;
        Vector2Int nextCoord;
        
        if (Mathf.Abs(dirCheck.x) > Mathf.Abs(dirCheck.y))
        {
            curTile = tile;
            Tile nextTile;
            if (0 < dirCheck.x)
            {
                nextCoord = new Vector2Int(curTile.coord.x + 1, curTile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null; 
                CheckTile(nextTile);

            }
            else if (0 > dirCheck.x)
            {
                nextCoord = new Vector2Int(curTile.coord.x - 1, curTile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);
            }

        }
        else
        {
            curTile = tile;
            Tile nextTile;
            if (0 < dirCheck.y)
            {
                nextCoord = new Vector2Int(curTile.coord.x, curTile.coord.y + 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);

            }
            else if (0 > dirCheck.y)
            {
                nextCoord = new Vector2Int(curTile.coord.x, curTile.coord.y - 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                CheckTile(nextTile);
            }
        }

    }

    /// <summary>
    
    /// </summary>
    public void FindNextTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f, 1 << LayerMask.NameToLayer("Tile")))
        {            
            if (hit.collider.TryGetComponent(out Tile tile))
            {
                if (isPatrol)
                {
                    PatrolMove(tile);
                }
                // 다른 움직임
                else
                {
                    TraceModeMove(tile);
                }
                
            }
        }
    }

    private void CheckTile(Tile nextTile)
    {
        if (nextTile == null || nextTile.tileType == TileType.impossible)
        {
            StartCoroutine(MoveMob(nextTile));
        }

       
    }

    private IEnumerator MoveMob(Tile nextTile)
    {
        if (nextTile == null)
        {
            count = moveCount;
            isEnd = true;
            isDone = true;
            curTile.tileType = TileType.impossible;

            manager_Mob.CheckMobAction();
            yield break;
        }

        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

        map.TakeDamage(nextTile);

        while (Vector3.Distance(transform.position, nextPos) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, 8f * Time.deltaTime);
            yield return null;
        }
        transform.position = nextPos;
        curTile = nextTile;
        nextTile.tileType = TileType.impossible;
        nextTile.mob = this.GetComponent<Mob>();

        yield return new WaitForSeconds(0.5f);

        if (--count > 0)
        {
            FindNextTile();
            yield break;
        }

        count = moveCount;
        isEnd = true;
        isDone = true;

        manager_Mob.CheckMobAction();
    }

    public Tile ShowTile()
    {
        return curTile;
    }


}
