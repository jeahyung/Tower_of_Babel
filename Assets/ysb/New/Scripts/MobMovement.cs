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
    //x, y�� �ٲ���. x = y�� / y = x�� / 1 = ������,�� / -1 = ����, �Ʒ�
    protected PatrolMobManager manager_Mob;

    public Map map;
    public Tile curTile;  //���� ��ġ�� Ÿ��
    public Vector2Int moveDir;     //������ ����

    [Header("���� ������")] //�̰� ���� ������ �޾ƿ��� �������� ����
    [SerializeField] protected int startX;
    [SerializeField] protected int startY;

    public int moveCount = 2;   //�����̴� ĭ ��
    protected int count;
    protected bool isEnd = true;  //�ൿ�� �����ߴ°�?
    protected bool isDone = false;

    protected List<Tile> range;    //������ �� �ִ� ����
    public int leftRagne = 1;   //�������� �� ĭ����?(�Ʒ�)
    public int rightRange = 1;  //���������� �� ĭ����?(��)   //�������� �������� ��´�(-1,1 ����)

    //[SerializeField] private float moveSpeed;

    public bool canAct = true;   //������ �� �ִ°�?
    public bool isRope = false; //������ �ɷȴ°�?

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
        range.Add(curTile); //���� ĭ
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

        //�� ȸ��
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
        //range.Add(cTile); //���� ĭ
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
        ////�� ȸ��
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

        RotateMob(curTile);//ȸ��
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
