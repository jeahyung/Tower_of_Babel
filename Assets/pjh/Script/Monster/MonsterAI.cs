using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{

    public class MonsterState
    {

        //private Vector3 startPosition;

        //x, y�� �ٲ���. x = y�� / y = x�� / 1 = ������,�� / -1 = ����, �Ʒ�
        //public PatrolMobManager manager_Mob;

        public Map map;
        public Tile curTile;  //���� ��ġ�� Ÿ��
        public Tile startTile;


        public int moveCount = 2;   //�����̴� ĭ ��
        public int visualRange = 2;

        public bool isEnd = true;  //�ൿ�� �����ߴ°�?
        public bool isDone = false;

        public List<Tile> range;    //������ �� �ִ� ����
        
    }
    //public Transform[] patrolPoints;
    //public Transform player;
    //public float chaseDistance = 20f;

    //  private MonsterState currentState;
    private Vector3 startPosition;
    public MonsterState ms;
    public PatrolMobManager manager_Mob;    //�̰� ����Ī ������ ���� ���� �ʿ�
    // Enum���� ���� ����
    public enum State
    {
        Patrol, // ����
        Chase,  // �߰�
        Return  // ����
    }

    // ���� ���¸� �����ϴ� ����
    private State currentState;

    //x, y�� �ٲ���. x = y�� / y = x�� / 1 = ������,�� / -1 = ����, �Ʒ�
    //PatrolMobManager manager_Mob;

   
    private Tile startTile;
    public Vector2Int moveDir;     //������ ����  //�����տ��� ����

    [Header("���� ������")] //�̰� ���� ������ �޾ƿ��� �������� ����
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    //public int moveCount = 2;   //�����̴� ĭ ��
    public int visualRange = 2;
    private int count;
    
    //[SerializeField] private float moveSpeed;

    public bool canAct = true;   //������ �� �ִ°�?
    public bool isRope = false; //������ �ɷȴ°�? 
    public bool isPatrol = true;
    public int leftRagne = 1;   //�������� �� ĭ����?(�Ʒ�)
    public int rightRange = 1;  //���������� �� ĭ����?(��)   //�������� �������� ��´�(-1,1 ����)
    public List<Tile> viewRange;

    private void Awake()
    {
        ms = new MonsterState();
        manager_Mob = GetComponentInParent<PatrolMobManager>();
        ms.map = FindObjectOfType<Map>();
        count = ms.moveCount;                
    }

    private void Start()
    {
        currentState = State.Patrol; // ó������ ���� ���·� ����

        MobSetting();

        //SetState(new PatrolState(this)); // ���� ���·� ����
    }

    private void Update()
    {
        // currentState?.Update();
        //switch (currentState)
        //{
        //    case State.Patrol:
        //        Patrol(curTile);
        //        break;
        //    case State.Chase:
        //        Chase(curTile);
        //        break;
        //    case State.Return:
        //        ReturnToStart(startTile);
        //        break;
        //}
    }

    private void MobSetting()
    {
        ms.curTile = ms.map.GetTile(ms.map.tiles[startX, startY].coord);
        startTile = ms.curTile;
        ms.curTile.tileType = TileType.impossible;
        ms.curTile.mob = this.GetComponent<Mob>();
        isPatrol = true;

        Vector3 pos = new Vector3(ms.curTile.GetPosition().x, ms.curTile.GetPosition().y + 3, ms.curTile.GetPosition().z);
        transform.position = pos;

        ms.range = new List<Tile>();
        viewRange = new List<Tile>();

        ms.range.Add(ms.curTile); //���� ĭ
        for (int i = 0; i < leftRagne; ++i)
        {
            Vector2Int nextCoord = ms.curTile.coord + -moveDir * (i + 1);
            Tile nextTile = ms.map.GetTile(nextCoord);
            if (nextTile == null) { break; }
            ms.range.Add(nextTile);
        }

        for (int i = 0; i < rightRange; ++i)
        {
            Vector2Int nextCoord = ms.curTile.coord + moveDir * (i + 1);
            Tile nextTile = ms.map.GetTile(nextCoord);
            if (nextTile == null) { break; }
            ms.range.Add(nextTile);
        }
    }

    public void CheckRange()
    {
        SetViewRange();
        bool a = false;

        foreach (Tile tile in viewRange)
        {
            if(tile.coord == ms.map.playerTile.coord)
            {
                currentState = State.Chase;
                Debug.Log("State =====> Chase");
                a = true;
                break;
            }
        }

        if(!a)
        {
            if (currentState == State.Chase)
            {
                Debug.Log("Chase =====> Return");

                currentState = State.Return;
            }
            else if(currentState == State.Patrol)
            {
                Debug.Log("Return =====> Patrol");
                currentState = State.Patrol;
            }
        }
    }

    public void SetViewRange()
    {
        viewRange.Clear();

        for (int i = 0; i < visualRange;  ++i)
        {
            Vector2Int addRange = ms.curTile.coord + moveDir * (i + 1);     // moveDir �̰� 1�̶� n�� ���ϱ� ���� i+1
            Tile findTile = ms.map.GetTile(addRange);
            if(findTile !=  null)
            {
                viewRange.Add(findTile);
            }
            //viewRange.Add();
        }
    }


    public void Patrol(Tile tile)
    {
        ms.curTile = tile;

        Vector2Int nextCoord = ms.curTile.coord + moveDir;
        Tile nextTile = ms.map.GetTile(nextCoord);

        if (nextTile == null || ms.range.Contains(nextTile) == false)
        {
            moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
            nextCoord = ms.curTile.coord + moveDir;
            nextTile = ms.map.GetTile(nextCoord);
        }
        EffectManage.Instance.PlayEffect("Monster_Move", this.transform.position);
        transform.forward = new Vector3(moveDir.y, 0, moveDir.x);

        tile.tileType = TileType.possible;
        tile.mob = null;
        StartCoroutine(MoveMob(nextTile));
    }

    public void Chase(Tile tile)
    {
        Vector2Int dirCheck = ms.map.playerTile.coord - ms.curTile.coord;
        Movepattern(dirCheck, tile);
    }

    public void Movepattern(Vector2Int dirCheck, Tile tile)
    {
        Vector2Int nextCoord;

        if (Mathf.Abs(dirCheck.x) > Mathf.Abs(dirCheck.y))
        {
            ms.curTile = tile;
            Tile nextTile;
            if (0 < dirCheck.x)
            {
                nextCoord = new Vector2Int(ms.curTile.coord.x + 1, ms.curTile.coord.y);
                nextTile = ms.map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));

            }
            else if (0 > dirCheck.x)
            {
                nextCoord = new Vector2Int(ms.curTile.coord.x - 1, ms.curTile.coord.y);
                nextTile = ms.map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
            }

        }
        else
        {
            ms.curTile = tile;
            Tile nextTile;
            if (0 < dirCheck.y)
            {
                nextCoord = new Vector2Int(ms.curTile.coord.x, ms.curTile.coord.y + 1);
                nextTile = ms.map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));

            }
            else if (0 > dirCheck.y)
            {
                nextCoord = new Vector2Int(ms.curTile.coord.x, ms.curTile.coord.y - 1);
                nextTile = ms.map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
            }
        }
    }

    public void ReturnToStart(Tile tile)
    {
        Vector2Int dirCheck = startTile.coord - ms.curTile.coord;
        if (dirCheck.x == 0 && dirCheck.y == 0)
        {
            Debug.Log("Return Turn Over");
            //manager_Mob.CheckMobAction();   //��⿡ �ɸ���  true false�� ���� ���;��ϴµ�,,?
            currentState = State.Patrol;    //���� ��ȭ�� 1�� �Ҹ��� �ٽ� ����
            Debug.Log("Return =====> Patrol");
            Act();
            return;
        }

        Movepattern(dirCheck, tile);
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
        //if(nextTile.tileType == TileType.impossible)
        //{
        //    Debug.Log("TileType.impossible choose Warring");
        //    yield break;
        //}
        if (nextTile == null)
        {
            count = ms.moveCount;
            ms.isEnd = true;
            ms.isDone = true;
            ms.curTile.tileType = TileType.impossible;

            manager_Mob.CheckMobAction();
            yield break;
        }

        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

        ms.map.TakeDamage(nextTile);

        while (Vector3.Distance(transform.position, nextPos) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, 8f * Time.deltaTime);
            yield return null;
        }
        transform.position = nextPos;
        ms.curTile = nextTile;
        nextTile.tileType = TileType.impossible;
        nextTile.mob = this.GetComponent<Mob>();

        yield return new WaitForSeconds(0.5f);

        if (--count > 0)
        {
            Act(); // ���� �ൿ
            yield break;
        }

        count = ms.moveCount;
        ms.isEnd = true;
        ms.isDone = true;

        manager_Mob.CheckMobAction();
    }

    public void DontMove()
    {
        isRope = true;
    }

    public void Act()
    {
        if(currentState != State.Return)
        {
            CheckRange();
        }       

        switch (currentState)
        {
            case State.Patrol:
                Patrol(ms.curTile);
                break;
            case State.Chase:
                Chase(ms.curTile);
                break;
            case State.Return:
                ReturnToStart(ms.curTile);
                break;
        }
    }

    public void SetState(MonsterState newState)
    {
        //currentState?.Exit(); // ���� ���� ����
        //currentState = newState;
        //currentState.Enter(); // ���ο� ���� ����
    }

    public void MoveTo(int patrolIndex)
    {
        // ���� �������� �̵��ϴ� ���� (NavMesh ��� ��)
        // e.g., NavMeshAgent.SetDestination(patrolPoints[patrolIndex].position);
    }

    public void MoveToStartPoint()
    {
        // ó�� ������ ��ġ�� ���ư��� ����
    }

    public void StartChasingPlayer()
    {
        // �÷��̾� �߰� ���� (NavMesh ���)
    }

    public void ChasePlayer()
    {
        // �÷��̾ �߰��ϴ� ���� (�÷��̾� ��ġ�� ��� �̵�)
    }

    public bool HasReachedDestination()
    {
        // ���� �������� �����ߴ��� Ȯ���ϴ� ����
        return true; // NavMeshAgent.remainingDistance üũ ��
    }

    public bool HasReachedStartPoint()
    {
        // ������ �� ó�� ���� ��ġ�� �����ߴ��� Ȯ��
        return Vector3.Distance(transform.position, startPosition) < 0.1f;
    }

    public Tile ShowTile()
    {
        return ms.curTile;
    }

}


public class ChasMode : MonoBehaviour
{


}
