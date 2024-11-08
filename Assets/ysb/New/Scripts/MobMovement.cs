using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Mob
{
    public void DontMove();
    public List<Tile> ShowRange();
    public Tile ShowTile();
    //public void SetStartPoint(Vector2Int sPoint);
    public void SetStartPoint(Vector2Int sPoint, Tile curTile);
    public void DestoryMob();
}

public class MobMovement : MonoBehaviour, Mob
{
    //x, y과 바꼈다. x = y축 / y = x축 / 1 = 오른쪽,위 / -1 = 왼쪽, 아래
    protected PatrolMobManager manager_Mob;

    public Map map;
    public Tile curTile;  //현재 위치한 타일
    public Vector2Int moveDir;     //움직일 방향

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] protected int startX;
    [SerializeField] protected int startY;

    public int moveCount = 2;   //움직이는 칸 수
    protected int count;
    protected bool isEnd = true;  //행동을 종료했는가?
    protected bool isDone = false;

    protected List<Tile> range;    //움직일 수 있는 범위
    public int leftRagne = 1;   //왼쪽으로 몇 칸까지?(아래)
    public int rightRange = 1;  //오른쪽으로 몇 칸까지?(위)   //오른쪽을 기준으로 잡는다(-1,1 동일)

    //[SerializeField] private float moveSpeed;

    public bool canAct = true;   //움직일 수 있는가?
    public bool isRope = false; //로프에 걸렸는가?

    Animator anim;
    Tile attackTile = null;
    bool attackEnd = false;
    private void Awake()
    {
        count = moveCount;
        manager_Mob = GetComponentInParent<PatrolMobManager>();
        map = FindObjectOfType<Map>();
        anim = GetComponentInChildren<Animator>();
    }

    public virtual void InitMob()
    {
        curTile = map.GetTile(map.tiles[startX, startY].coord);
        if(curTile == null) { curTile = map.GetTileForSpawn(map.tiles[startX, startY].coord); }
        curTile.tileType = TileType.impossible;
        curTile.mob = this.GetComponent<Mob>();

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

        //몹 회전
        RotateMob(curTile);
    }

    public void DestoryMob()
    {
        curTile.tileType = TileType.possible;
        curTile.mob = null;

        gameObject.SetActive(false);
    }


    public void SetStartPoint(Vector2Int sPoint, Tile cTile)
    {
        MobData_P data = MobDataBase.instance.GetpMobData();

        moveDir = new Vector2Int(data.moveX, data.moveY);
        rightRange = data.rangeR;
        leftRagne = data.rangeL;

        map = FindObjectOfType<Map>();
        startX = sPoint.x; 
        startY = sPoint.y;
        //Vector3 pos = new Vector3(cTile.GetPosition().x, cTile.GetPosition().y + 4, cTile.GetPosition().z);
        //transform.position = pos;

        //range = new List<Tile>();
        //range.Clear();
        //range.Add(cTile); //현재 칸
        //for (int i = 0; i < leftRagne; ++i)
        //{
        //    Vector2Int nextCoord = cTile.coord + -moveDir * (i + 1);
        //    Tile nextTile = map.GetTile(nextCoord);
        //    if (nextTile == null) { break; }
        //    range.Add(nextTile);
        //}

        //for (int i = 0; i < rightRange; ++i)
        //{
        //    Vector2Int nextCoord = cTile.coord + moveDir * (i + 1);
        //    Tile nextTile = map.GetTile(nextCoord);
        //    if (nextTile == null) { break; }
        //    range.Add(nextTile);
        //}
        ////몹 회전
        //RotateMob(cTile);
    }

    public void DontMove()
    {
        isRope = true;
    }
    public void Act()
    {
        if(isRope)
        {
            if (manager_Mob == null) { manager_Mob = GetComponentInParent<PatrolMobManager>(); }
            manager_Mob.CheckMobAction();
            isRope = false;
            return;
        }

        if(isEnd == false) { return; }
        isEnd = false;

        if(canAct == false)
        {
            canAct = true;
            return;
        }
        FindNextTile();
    }
    public virtual void FindNextTile()
    {        
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 50f, 1 << LayerMask.NameToLayer("Tile")))
        {
            if (hit.collider.TryGetComponent(out Tile tile))
            {
                curTile = tile;

                Vector2Int nextCoord = curTile.coord + moveDir;
                Tile nextTile = map.GetTile(nextCoord);

                if(nextTile == null || range.Contains(nextTile) == false)
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

    protected virtual void RotateMob(Tile tile)
    {
        if(tile == null) { return; }
        Vector2Int nextCoord = tile.coord + moveDir;
        Tile nextTile = map.GetTile(nextCoord);

        if (nextTile == null || range.Contains(nextTile) == false)
        {
            moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
            nextCoord = tile.coord + moveDir;
            nextTile = map.GetTile(nextCoord);
        }
        transform.forward = new Vector3(moveDir.y, 0, moveDir.x);
    }

    protected virtual IEnumerator MoveMob(Tile nextTile)
    {
        if(nextTile == null)
        {
            count = moveCount;
            isEnd = true;
            isDone = true;
            curTile.tileType = TileType.impossible;

            if (manager_Mob == null) { manager_Mob = GetComponentInParent<PatrolMobManager>(); }
            manager_Mob.CheckMobAction();
            yield break;
        }
        attackEnd = false;
        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

        if(map.nowTile == nextTile)
        {
            attackTile = nextTile;
            anim.SetTrigger("isAttack");
            while (!attackEnd)
            {
                yield return null;
            }
        }

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

        if(--count > 0)
        {
            FindNextTile();
            yield break;
        }

        count = moveCount;
        isEnd = true;
        isDone = true;

        RotateMob(curTile);//회전
        if(manager_Mob == null) { manager_Mob = GetComponentInParent<PatrolMobManager>(); }
        manager_Mob.CheckMobAction();
    }

    public Tile ShowTile()
    {
        return curTile;
    }

    public List<Tile> ShowRange()
    {
        Debug.Log("Click mob");
        return range;
    }

    public void Attack()
    {
        map.TakeDamage(attackTile);
        attackTile = null;
    }
    public void AttackEnd()
    {
        attackEnd = true;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        other.SendMessage("TakeDamage");
    //    }
    //}
}
