using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{

    public class MonsterState
    {

        //private Vector3 startPosition;

        //x, y과 바꼈다. x = y축 / y = x축 / 1 = 오른쪽,위 / -1 = 왼쪽, 아래
        //public PatrolMobManager manager_Mob;

        public Map map;
        public Tile curTile;  //현재 위치한 타일
        public Tile startTile;


        public int moveCount = 2;   //움직이는 칸 수
        public int visualRange = 2;

        public bool isEnd = true;  //행동을 종료했는가?
        public bool isDone = false;

        public List<Tile> range;    //움직일 수 있는 범위
        
    }
    //public Transform[] patrolPoints;
    //public Transform player;
    //public float chaseDistance = 20f;

    //  private MonsterState currentState;
    private Vector3 startPosition;
    public MonsterState ms;
    public PatrolMobManager manager_Mob;    //이거 스위칭 용으로 별도 제작 필요
    // Enum으로 상태 정의
    public enum State
    {
        Patrol, // 순찰
        Chase,  // 추격
        Return  // 복귀
    }

    // 현재 상태를 저장하는 변수
    private State currentState;

    //x, y과 바꼈다. x = y축 / y = x축 / 1 = 오른쪽,위 / -1 = 왼쪽, 아래
    //PatrolMobManager manager_Mob;

   
    private Tile startTile;
    public Vector2Int moveDir;     //움직일 방향  //프리팹에서 지정

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    //public int moveCount = 2;   //움직이는 칸 수
    public int visualRange = 2;
    private int count;
    
    //[SerializeField] private float moveSpeed;

    public bool canAct = true;   //움직일 수 있는가?
    public bool isRope = false; //로프에 걸렸는가? 
    public bool isPatrol = true;
    public int leftRagne = 1;   //왼쪽으로 몇 칸까지?(아래)
    public int rightRange = 1;  //오른쪽으로 몇 칸까지?(위)   //오른쪽을 기준으로 잡는다(-1,1 동일)
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
        currentState = State.Patrol; // 처음에는 순찰 상태로 시작

        MobSetting();

        //SetState(new PatrolState(this)); // 순찰 상태로 시작
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

        ms.range.Add(ms.curTile); //현재 칸
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
            Vector2Int addRange = ms.curTile.coord + moveDir * (i + 1);     // moveDir 이게 1이라 n번 더하기 위해 i+1
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
            //manager_Mob.CheckMobAction();   //요기에 걸리네  true false로 거짓 나와야하는데,,?
            currentState = State.Patrol;    //상태 변화에 1턴 소모후 다시 순찰
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
            Act(); // 다음 행동
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
        //currentState?.Exit(); // 현재 상태 종료
        //currentState = newState;
        //currentState.Enter(); // 새로운 상태 시작
    }

    public void MoveTo(int patrolIndex)
    {
        // 순찰 지점으로 이동하는 로직 (NavMesh 사용 등)
        // e.g., NavMeshAgent.SetDestination(patrolPoints[patrolIndex].position);
    }

    public void MoveToStartPoint()
    {
        // 처음 시작한 위치로 돌아가는 로직
    }

    public void StartChasingPlayer()
    {
        // 플레이어 추격 시작 (NavMesh 사용)
    }

    public void ChasePlayer()
    {
        // 플레이어를 추격하는 로직 (플레이어 위치로 계속 이동)
    }

    public bool HasReachedDestination()
    {
        // 현재 목적지에 도달했는지 확인하는 로직
        return true; // NavMeshAgent.remainingDistance 체크 등
    }

    public bool HasReachedStartPoint()
    {
        // 복귀할 때 처음 시작 위치에 도달했는지 확인
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
