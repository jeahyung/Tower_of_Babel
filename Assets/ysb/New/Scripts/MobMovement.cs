using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface Mob
{
    public void DontMove();
    public Tile ShowTile();
}

public class MobMovement : MonoBehaviour, Mob
{
    //x, y�� �ٲ���. x = y�� / y = x�� / 1 = ������,�� / -1 = ����, �Ʒ�
    PatrolMobManager manager_Mob;

    public Map map;
    public Tile curTile;  //���� ��ġ�� Ÿ��
    public Vector2Int moveDir;     //������ ����

    [Header("���� ������")] //�̰� ���� ������ �޾ƿ��� �������� ����
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    public int moveCount = 2;   //�����̴� ĭ ��
    private int count;
    public bool isEnd = true;  //�ൿ�� �����ߴ°�?
    public bool isDone = false;

    public List<Tile> range;    //������ �� �ִ� ����
    public int leftRagne = 1;   //�������� �� ĭ����?(�Ʒ�)
    public int rightRange = 1;  //���������� �� ĭ����?(��)   //�������� �������� ��´�(-1,1 ����)

    //[SerializeField] private float moveSpeed;

    public bool canAct = true;   //������ �� �ִ°�?
    public bool isRope = false; //������ �ɷȴ°�?

    private void Awake()
    {
        count = moveCount;
        manager_Mob = GetComponentInParent<PatrolMobManager>();
        map = FindObjectOfType<Map>();
    }

    private void Start()
    {
        curTile = map.GetTile(map.tiles[startX, startY].coord);
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
    }


    public void DontMove()
    {
        isRope = true;
    }
    public void Act()
    {
        if(isRope)
        {
            manager_Mob.CheckMobAction();
            EffectManage.Instance.PlayEffect("Rope_Effect", this.transform.position);
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
    public void FindNextTile()
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
                EffectManage.Instance.PlayEffect("Monster_Move", this.transform.position);
                transform.forward = new Vector3(moveDir.y, 0, moveDir.x);

                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
            }
        }
    }

    private IEnumerator MoveMob(Tile nextTile)
    {
        if(nextTile == null)
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

        if(--count > 0)
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        other.SendMessage("TakeDamage");
    //    }
    //}
}
