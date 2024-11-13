using Artngame.PDM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour, Mob
{

   
        //private Vector3 startPosition;

    //x, y과 바꼈다. x = y축 / y = x축 / 1 = 오른쪽,위 / -1 = 왼쪽, 아래
    //public PatrolMobManager manager_Mob;

        public Map map;
        public Tile curTile;  //현재 위치한 타일
        //public Tile startTile;


        public int moveCount = 2;   //움직이는 칸 수
        public int visualRange = 2;

        public bool isEnd = true;  //행동을 종료했는가?
        public bool isDone = false;

        public List<Tile> range;    //움직일 수 있는 범위
        
    
   // private Animator ani;
    //변경된 추격 로직을 위한 변수
    private List<Vector3> directions = new List<Vector3>();
    public List<Tile> arr = new List<Tile>();
    private List<Tile> allTiles = new List<Tile>();
    private Tile pre;
    private bool attackEnd = false;
    //----------------------------------------------------------------------------------


    private BishopManager bishopManager;
    private Vector3 startPosition;
    public GameObject[] stateEffect;

    //public PatrolMobManager manager_Mob;    //이거 스위칭 용으로 별도 제작 필요
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
 

   
    private Tile startTile;
    public Vector2Int moveDir;     //움직일 방향  //프리팹에서 지정

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    //public int moveCount = 2;   //움직이는 칸 수
    private int count;
    
   

    public bool canAct = true;   //움직일 수 있는가?
    public bool isRope = false; //로프에 걸렸는가? 
    public bool isPatrol = true;
    public int leftRagne = 1;   //왼쪽으로 몇 칸까지?(아래)
    public int rightRange = 1;  //오른쪽으로 몇 칸까지?(위)   //오른쪽을 기준으로 잡는다(-1,1 동일)
    public List<Tile> viewRange;
    
    private void Awake()
    {
      
       // manager_Mob = GetComponentInParent<PatrolMobManager>();
        map = FindObjectOfType<Map>();
        count = moveCount;
        Tile[] tiles = FindObjectsOfType<Tile>();
        //tile1 = FindObjectOfType<Tile>();
        allTiles.AddRange(tiles);
        AllEffectOff();
    }
    private void Start()
    {
        stateEffect[0].SetActive(true);        
    }
    public void InitMob()
    {
        //ani = GetComponent<Animator>();
        currentState = State.Patrol; // 처음에는 순찰 상태로 시작
        bishopManager = GetComponentInParent<BishopManager>();
        MobSetting();

        //SetState(new PatrolState(this)); // 순찰 상태로 시작
    }

    public void AllEffectOff()
    {
        for(int i = 0; i < stateEffect.Length; i++)
        {
            stateEffect[i].SetActive(false);
        }
       
    }
    private void MobSetting()
    {

        if (curTile == null)
        {
            curTile = map.GetTile(map.tiles[startX, startY].coord);
        }//curTile = map.GetTileForSpawn(map.tiles[startX, startY].coord); }
        startTile = curTile;
        curTile.tileType = TileType.impossible;
        curTile.mob = this.GetComponent<Mob>();
        isPatrol = true;

        Vector3 pos = new Vector3(curTile.GetPosition().x, curTile.GetPosition().y + 3, curTile.GetPosition().z);
        transform.position = pos;

        range = new List<Tile>();
        viewRange = new List<Tile>();

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

    public void CheckRange()
    {
        SetViewRange(curTile);
        bool a = false;

        foreach (Tile tile in viewRange)
        {
            if(tile.coord == map.playerTile.coord)
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

    public void SetViewRange(Tile now) //now는 ms.curTile
    {
        viewRange.Clear();

        //for (int i = 0; i < visualRange;  ++i)
        //{
        //    Vector2Int addRange = ms.curTile.coord + moveDir * (i + 1);     // moveDir 이게 1이라 n번 더하기 위해 i+1
        //    Tile findTile = ms.map.GetTile(addRange);
        //    if(findTile !=  null)
        //    {
        //        viewRange.Add(findTile);
        //    }
        //    //viewRange.Add();
        //}

        Vector2Int nextCoord;
        Tile nextTile = null;


        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == now.coord.x + 1 && tile.coord.y == now.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);                    
                }

            }
            if (tile.coord.x == now.coord.x - 1 && tile.coord.y == now.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            if (tile.coord.x == now.coord.x && tile.coord.y == now.coord.y + 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);                    
                }
            }
            if (tile.coord.x == now.coord.x && tile.coord.y == now.coord.y - 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            //----------------------------1칸 대각선-----------------------------------
            if (tile.coord.x == now.coord.x + 1 && tile.coord.y == now.coord.y+1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }

            }
            if (tile.coord.x == now.coord.x - 1 && tile.coord.y == now.coord.y-1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            if (tile.coord.x == now.coord.x-1 && tile.coord.y == now.coord.y + 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            if (tile.coord.x == now.coord.x+1 && tile.coord.y == now.coord.y - 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            //-----------------------------2칸 상하좌우---------------------------------
            if (tile.coord.x == now.coord.x + 2 && tile.coord.y == now.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }

            }
            if (tile.coord.x == now.coord.x - 2 && tile.coord.y == now.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            if (tile.coord.x == now.coord.x && tile.coord.y == now.coord.y + 2)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }
            if (tile.coord.x == now.coord.x && tile.coord.y == now.coord.y - 2)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    viewRange.Add(tile);
                }
            }

        }
    }


    public void Patrol(Tile tile)
    {
        

        curTile = tile;
        pre = tile;
        Vector2Int nextCoord = curTile.coord + moveDir;
        Tile nextTile = map.GetTile(nextCoord);

        if (nextTile == null || range.Contains(nextTile) == false)
        {
            moveDir = new Vector2Int(-moveDir.x, -moveDir.y);
            nextCoord = curTile.coord + moveDir;
            nextTile = map.GetTile(nextCoord);
        }
       // EffectManage.Instance.PlayEffect("Monster_Move", this.transform.position);
        transform.forward = new Vector3(moveDir.y, 0, moveDir.x);

        tile.tileType = TileType.possible;
        tile.mob = null;
        StartCoroutine(MoveMob(nextTile));
    }

    public void Chase(Tile tile)
    {
        ArrSet(tile);
       

        FindAnyWay(tile, map.nowTile);
    }

    public void Movepattern(Vector2Int dirCheck, Tile tile)
    {
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
                StartCoroutine(MoveMob(nextTile));

            }
            else if (0 > dirCheck.x)
            {
                nextCoord = new Vector2Int(curTile.coord.x - 1, curTile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
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
                StartCoroutine(MoveMob(nextTile));

            }
            else if (0 > dirCheck.y)
            {
                nextCoord = new Vector2Int(curTile.coord.x, curTile.coord.y - 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
            }
        }
    }

    public void ReturnToStart(Tile tile)
    {
       

        Vector2Int dirCheck = startTile.coord - curTile.coord;
        if (dirCheck.x == 0 && dirCheck.y == 0)
        {
            Debug.Log("Return Turn Over");
            //manager_Mob.CheckMobAction();   //요기에 걸리네  true false로 거짓 나와야하는데,,?
            currentState = State.Patrol;    //상태 변화에 1턴 소모후 다시 순찰
            Debug.Log("Return =====> Patrol");
            Act();
            return;
        }

        arr.Clear();
        directions.Clear();

        ArrSet(tile);

        FindAnyWay(tile, startTile);
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
            count = moveCount;
            isEnd = true;
            isDone = true;
            curTile.tileType = TileType.impossible;

            bishopManager.CheckMobAction();
            arr.Clear();
            directions.Clear();
            yield break;
        }
        attackEnd = false;
        if (nextTile.coord == map.playerTile.coord)
        {
            //ani.SetTrigger("Attack");
            map.TakeDamage(nextTile);
            EffectManage.Instance.PlayEffect("Bishop_Attack", nextTile.GetPosition());
            while (!attackEnd)
            {
                Invoke("EndAttack", 0.5f);
                yield return null;
            }
            ChangTileType();
        }
        ChangTileType();
        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

       // ms.map.TakeDamage(nextTile);

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
            arr.Clear();
            directions.Clear();
            Act(); // 다음 행동
            yield break;
        }

        count = moveCount;
        isEnd = true;
        isDone = true;
        arr.Clear();
        directions.Clear();
        bishopManager.CheckMobAction();
    }

    private void EndAttack()
    {
        if(attackEnd == false)
        {
            attackEnd = !attackEnd;
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
            EffectManage.Instance.PlayEffect("Rope_Effect", this.transform.position);
            if(bishopManager == null) { bishopManager = GetComponentInParent<BishopManager>(); }
            bishopManager.CheckMobAction();
            isRope = false;
            return;
        }

        if(currentState != State.Return)
        {
            CheckRange();
        }


        
        switch (currentState)
        {
            case State.Patrol:
                {
                    stateEffect[0].SetActive(true);
                    stateEffect[1].SetActive(false);
                    stateEffect[2].SetActive(false);
                    Patrol(curTile);
                    break;
                }                
            case State.Chase:
                {
                    stateEffect[1].SetActive(true);
                    stateEffect[0].SetActive(false);
                    stateEffect[2].SetActive(false);
                    Chase(curTile);
                    break;
                }              
            case State.Return:
                {
                    stateEffect[2].SetActive(true);
                    stateEffect[1].SetActive(false);
                    stateEffect[0].SetActive(false);
                    ReturnToStart(curTile);
                    break;
                }              
        }
    }
    public void ArrSet(Tile startTiles)
    {
        Vector2Int nextCoord;
        Tile nextTile = null;


        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == startTiles.coord.x + 1 && tile.coord.y == startTiles.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(0, 0, 1));
                    
                   
                }

            }
            if (tile.coord.x == startTiles.coord.x - 1 && tile.coord.y == startTiles.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(0, 0, -1));
                    
                }
            }
            if (tile.coord.x == startTiles.coord.x && tile.coord.y == startTiles.coord.y + 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(1, 0, 0));
                   
                }
            }
            if (tile.coord.x == startTiles.coord.x && tile.coord.y == startTiles.coord.y - 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(-1, 0, 0));
                  
                }
            }
        }
    }
    private void ChangTileType()
    {
        pre.tileType = TileType.possible;
    }

    public void FindAnyWay(Tile tile, Tile goToThere)
    {

        pre = tile;
        if (arr == null || arr.Count == 0)
        {
            Debug.Log("경로가 존재하지 않음");
            //mgr_Chase.CheckMobAction();
            return;
        }

        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < arr.Count; i++)
        {
            float distance = Vector2Int.Distance(goToThere.coord, arr[i].coord);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        StartCoroutine(MoveMob(arr[closestIndex]));
        transform.forward = directions[closestIndex];
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
        return curTile;
    }

    public List<Tile> ShowRange()
    {
        return range;
    }

    public void SetStartPoint(Vector2Int sPoint, Tile curTile)
    {
        MobData_P data = MobDataBase.instance.GetpMobData();

        moveDir = new Vector2Int(data.moveX, data.moveY);
        rightRange = data.rangeR;
        leftRagne = data.rangeL;

        map = FindObjectOfType<Map>();
        startX = sPoint.x;
        startY = sPoint.y;

        this.curTile = curTile;
    }

    public void DestoryMob()
    {
        curTile.tileType = TileType.possible;
        curTile.mob = null;
        EffectManage.Instance.PlayEffect("Monster_Destroy", transform.position);

        gameObject.SetActive(false);
    }
}


public class ChasMode : MonoBehaviour
{


}
