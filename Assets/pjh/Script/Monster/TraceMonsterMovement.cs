using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TraceMonsterMovement : MonoBehaviour, Mob
{
    private ChaseMobManager mgr_Chase;
    private bool ch = true;

    [SerializeField] private GameObject monster;
   // [SerializeField]private GameObject player;

    [SerializeField] private Map map;
    private Animator ani;

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    
    public List<Tile> allTiles = new List<Tile>();
    public List<Tile> burnedTile = new List<Tile>();
    [SerializeField] private Tile tile = null;
   // private Tile tile1 = null;
    public Vector3 nextPos;

    public TurnManager manager_Turn;
    public bool minA;
    public bool minB;
    public bool isEnd = true;  //행동을 종료했는가?
    public bool isDone = false;

    public Vector2Int pos; //현재 위치 보기용이라 지워도 됨
    
    public float smoothTime = 0.2f;

    public int moveRange = 1;
    public GameObject effectPrefab;

    public bool isRope = false; //로프에 걸렸는가?

    public bool isUpgradeMob = false;
    public int moveCount = 2;
    private int count = 0;

    private Vector3 initialPosition;
    private bool isAnimationPlaying = false;


    private void Awake()
    {

        //  player = GameObject.FindWithTag("Player");
        manager_Turn = FindObjectOfType<TurnManager>();
        mgr_Chase = GetComponentInParent<ChaseMobManager>();
       // mgr_Chase = FindObjectOfType<ChaseMobManager>();
        //tile = GetComponent<Tile>();
        Tile[] tiles = FindObjectsOfType<Tile>();
        //tile1 = FindObjectOfType<Tile>();
        allTiles.AddRange(tiles);
    }
    private void Start() 
    {
        ani = GetComponent<Animator>();
        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);
        count = moveCount;

        Vector3 pos = new Vector3(curTile.GetPosition().x, curTile.GetPosition().y + 3, curTile.GetPosition().z);
        transform.position = pos;

        tile = curTile;
        tile.mob = this.GetComponent<Mob>();
        HideEffect();
        FindTileWithCoords(startX, startY);
        MonsterSetting(nextPos);
    }
    void Update()
    {
        //if (!ani.IsInTransition(0))
        //{
        //    AnimatorStateInfo stateInfo = ani.GetCurrentAnimatorStateInfo(0);
        //    if (stateInfo.IsName("Take 001") && stateInfo.normalizedTime >= 1f)
        //    {
        //      //  Debug.Log("Run 애니메이션이 끝났습니다.");
        //        ani.SetBool("Act", false); // Idle 상태로 돌아가기
        //        Chase(tile);
        //    }
        //}
        //pos = tile.coord;

        if (isAnimationPlaying)
        {
            transform.position = initialPosition;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            CheckAni();
        }
    }

    public void MonsterSetting(Vector3 target)
    {
        if (target == Vector3.zero) { return; }


        transform.position = new Vector3(target.x, transform.position.y, target.z);

    }
  

    private void Think()
    {
        tile.tileType = TileType.possible;
        tile.TileBurning(tile);
        burnedTile.Add(tile);

        tile.mob = null;
     //   BurnOff(tile);

        //tiles.coord = new Vector2Int(tiles.coord.x + 1, tiles.coord.y);
        //SetPosition(tiles.coord.GetPosition());
        int i = tile.coord.x;
        int j = tile.coord.y;


        //좌우가 x -> 같은 라인 coor y변화
        //상하가 z ->라인 변화 coor x변화

        Tile nowTile = map.playerTile;

        //타일 라인 변화는 coor x 변화
        //같은 라인 좌우는 coor y 변화
        int a = Mathf.Abs(nowTile.coord.x - tile.coord.x);
        int b = Mathf.Abs(nowTile.coord.y - tile.coord.y);
        minA = nowTile.coord.x - tile.coord.x < 0;
        minB = nowTile.coord.y - tile.coord.y < 0;
        //x축 접근
        if(a > b)
        {
            if (minA)
            {
                FindTileWithCoordsX(i, j);
            }
            else
            {
                FindTileWithCoordsX(i, j);
            }

        }
        else
        {
            if(minB) //y축 접근
            {
                FindTileWithCoordsY(i, j);
            }
            else
            {
               FindTileWithCoordsY(i, j);
            }
        }

        
         SetPosition(nextPos);
         ShowEffect();
         AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);
       

    }
    

    private void FindTileWithCoords(int targetX, int targetY)
    {
       
        // 조건을 만족하는 타일을 찾습니다.
        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == targetX && tile.coord.y == targetY)
            {
                //nextPos = tile.GetPosition();
                if(tile.tileType == TileType.impossible)
                {
                   // FindTileWithCoords(targetX, targetY, !root);
                }
                else
                {
                    nextPos = tile.GetPosition();
                    // tile1 = tile;
                }
                   
            }
        }
    }

    private void FindTileWithCoordsX(int targetX, int targetY)
    {
        int a = 0;
        if (minA)
        {
            a = targetX - 1;
            transform.forward = new Vector3(0, 0, -1);
        }
        else
        {
            a = targetX + 1;
            transform.forward = new Vector3(0, 0, 1);
        }
        // 조건을 만족하는 타일을 찾습니다.
        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == a && tile.coord.y == targetY)
            {

                if (tile.tileType == TileType.impossible)
                {
                    FindTileWithCoordsY(targetX, targetY);
                }
                else
                {
                    map.TakeDamage(tile);
                    nextPos = tile.GetPosition();
                }

            }
        }
    }
    private void FindTileWithCoordsY(int targetX, int targetY)
    {
        int a = 0;
        if (minB)
        {
            a = targetY- 1;
            transform.forward = new Vector3(-1, 0, 0);
        }
        else
        {
            a = targetY + 1;
            transform.forward = new Vector3(1, 0, 0);

        }
        // 조건을 만족하는 타일을 찾습니다.
        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == targetX && tile.coord.y == a)
            {
                if (tile.tileType == TileType.impossible)
                {
                    FindTileWithCoordsX(targetX, targetY);
                }
                else
                {
                    map.TakeDamage(tile);
                    nextPos = tile.GetPosition();
                }

            }
        }
    }

    //이동
    public void SetPosition(Vector3 target)
    {
        if (target == Vector3.zero) { return; }


        Vector3 pos1 = new Vector3(target.x, this.transform.position.y, target.z);
        manager_Turn.isDone = false;
        StartCoroutine(MonsterMove(pos1));
    }

    private IEnumerator MonsterMove(Vector3 target)
    {
        
        while (Vector3.Distance(transform.position, target) >= 0.05f)
        {
            Vector3 direction = target - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref direction, 0.3f*smoothTime);
            yield return null;
        }
       
        transform.position = target;
        CheckTile();
        if (ch)
        {
            ch = false;
            Think();
            yield break;
        }       
     //   manager_Turn.EndEnemyTurn();
      //  Debug.Log("Dddddd");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            tile = other.GetComponent<Tile>();
        }
        else if (other.CompareTag("Dia"))
        {
            map.RestPlayerTile();
            other.gameObject.SetActive(false);
            
            Debug.Log("Dia Find");
            
        }
        
    }

    public void Act()
    {
        if (isRope)
        {
            mgr_Chase.CheckMobAction();
            isRope = false;
            return;
        }
        Chase(tile);  
    }
    private void CheckTile()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 50f, 1 << LayerMask.NameToLayer("Tile")))
        {
            if (hit.collider.TryGetComponent(out Tile t))
            {
                tile = t;
                tile.tileType = TileType.impossible;
                tile.mob = this.GetComponent<Mob>();
            }
        }
        if (!ch)
        {

            if (mgr_Chase != null)
            {
                mgr_Chase.CheckMobAction();
            }

        }

        HideEffect();
    }

    private void ShowEffect()
    {
        if (effectPrefab != null)
        {
            effectPrefab.SetActive(true);
        }
    }


    private void HideEffect()
    {
        if (effectPrefab != null)
        {
            effectPrefab.SetActive(false);
        }
    }

    public void DontMove()
    {
        isRope = true;
    }
    public Tile ShowTile()
    {
        return tile;
    }

    public void BurnOff()
    {
        /*
        for (int i = 0; i < burnedTile.Count; i++)
        {
            burnedTile[i].flameHP--;
            
            if (burnedTile[i].flameHP <= 0)
            {
              //  burnedTilep[i].TileBurnOff(burnedTilep[i]);
            }
        }
        */
        Debug.Log("BurnOff");
        tile.TileBurnOff(burnedTile);
        
    }


    public void Chase(Tile tile)
    {
        Vector2Int dirCheck = map.playerTile.coord - tile.coord;
        Movepattern(dirCheck, tile);
    }

    public void Movepattern(Vector2Int dirCheck, Tile nowtile)
    {
        Vector2Int nextCoord;

        if (Mathf.Abs(dirCheck.x) > Mathf.Abs(dirCheck.y))
        {
            tile = nowtile;
            Tile nextTile;
            if (0 < dirCheck.x)
            {
                nextCoord = new Vector2Int(tile.coord.x + 1, tile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
                //CheckAni(nextTile);
            }
            else if (0 > dirCheck.x)
            {
                nextCoord = new Vector2Int(tile.coord.x - 1, tile.coord.y);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
                //CheckAni(nextTile);
            }

        }
        else
        {
            tile = nowtile;
            Tile nextTile;
            if (0 < dirCheck.y)
            {
                nextCoord = new Vector2Int(tile.coord.x, tile.coord.y + 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
                //CheckAni(nextTile);

            }
            else if (0 > dirCheck.y)
            {
                nextCoord = new Vector2Int(tile.coord.x, tile.coord.y - 1);
                nextTile = map.GetTile(nextCoord);
                tile.tileType = TileType.possible;
                tile.mob = null;
                StartCoroutine(MoveMob(nextTile));
                //CheckAni(nextTile);
            }
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
            tile.tileType = TileType.impossible;

            mgr_Chase.CheckMobAction();
            yield break;
        }

        if (nextTile.coord == map.playerTile.coord)
        {
            ani.SetTrigger("StartAttack");

            yield return new WaitForSeconds(1.2f);
            
        }
        map.TakeDamage(nextTile);
        ani.SetTrigger("StartIdle");
        float ypos = transform.position.y;
        Vector3 nextPos = new Vector3(nextTile.transform.position.x, ypos, nextTile.transform.position.z);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

        //map.TakeDamage(nextTile);

        while (Vector3.Distance(transform.position, nextPos) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, nextPos, 8f * Time.deltaTime);
            yield return null;
        }
        transform.position = nextPos;
        tile = nextTile;
        nextTile.tileType = TileType.impossible;
        nextTile.mob = this.GetComponent<Mob>();

        yield return new WaitForSeconds(0.5f);

        if (--count > 0)
        {
            Act(); // 다음 행동
            yield break;
        }

        count = moveCount;
        isEnd = true;
        isDone = true;

        mgr_Chase.CheckMobAction();
    }

    private void CheckAni()
    {
        initialPosition = transform.position;
        isAnimationPlaying = true;

        ani.SetBool("Act", true);

        Invoke("LateAni", 2f);
    }

    private void LateAni()
    {
        ani.SetBool("Act", false);
        isAnimationPlaying = false;
    }
}
