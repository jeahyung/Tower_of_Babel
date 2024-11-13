using Artngame.PDM.Kvant;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class TraceMonsterMovement : MonoBehaviour, Mob
{
    [SerializeField] private bool type = false;
    private ChaseMobManager mgr_Chase;
    private bool ch = true;
    private bool attackEnd = false;
    [SerializeField] private GameObject monster;
    // [SerializeField]private GameObject player;
    public Vector2Int moveDir;     //움직일 방향
    [SerializeField] private Map map;
    private Animator ani;

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    
    public List<Tile> allTiles = new List<Tile>();
    public List<Tile> burnedTile = new List<Tile>();
    public List<Tile> arr = new List<Tile>();
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
    private Tile next;
    private Tile pre;
    private List<Vector3> directions = new List<Vector3>();
    private Tile curTile;
    private List<Tile> newRange = new List<Tile>();
    public ParticleSystem lSpear;

    private void Awake()
    {
        map = FindObjectOfType<Map>();
        //  player = GameObject.FindWithTag("Player");
        manager_Turn = FindObjectOfType<TurnManager>();
        //mgr_Chase = GetComponentInParent<ChaseMobManager>();
       // mgr_Chase = FindObjectOfType<ChaseMobManager>();
        //tile = GetComponent<Tile>();
        Tile[] tiles = FindObjectsOfType<Tile>();
        //tile1 = FindObjectOfType<Tile>();
        allTiles.AddRange(tiles);
    

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DestoryMob();
        }
     
    }
    public void InitMob() 
    {
        //manager_Turn = FindObjectOfType<TurnManager>();
        mgr_Chase = GetComponentInParent<ChaseMobManager>();

        ani = GetComponent<Animator>();
        if (curTile == null) { curTile = map.GetTile(map.tiles[startX, startY].coord); }

        count = moveCount;

        Vector3 pos = new Vector3(curTile.GetPosition().x, curTile.GetPosition().y + 3, curTile.GetPosition().z);
        transform.position = pos;

        tile = curTile;
        tile.tileType = TileType.impossible;
        tile.mob = this.GetComponent<Mob>();
        HideEffect();
        FindTileWithCoords(startX, startY);
        MonsterSetting(nextPos);
        //type = false;
    }
    private void FindTileWithCoords(int targetX, int targetY)
    {

        // 조건을 만족하는 타일을 찾습니다.
        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == targetX && tile.coord.y == targetY)
            {
                //nextPos = tile.GetPosition();
                if (tile.tileType == TileType.impossible)
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
    public void MonsterSetting(Vector3 target)
    {
        if (target == Vector3.zero) { return; }


        transform.position = new Vector3(target.x, transform.position.y, target.z);

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
            other.GetComponent<CreatedObject>()?.DestroyObj();


            Debug.Log("Dia Find");
            
        }
        
    }

    public void Act()
    {
        if (isRope)
        {
            EffectManage.Instance.PlayEffect("Rope_Effect", this.transform.position);
            mgr_Chase.CheckMobAction();
            isRope = false;
            return;
        }

        if (gameObject.activeSelf)
        {
            Chase(tile);
        }
        else
        {
            mgr_Chase.CheckMobAction();
            return;
        }

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

 
    public Tile ShowTile()
    {
        return tile;
    }

    public void BurnOff()
    {

        if (type)
        {
            Debug.Log("BurnOff");
            //------------------------------------------------------------------------------------------------------
            //tile.TileBurnOff(burnedTile); //추가 필요
            tile.TileBurnOff(burnedTile);
            //------------------------------------------------------------------------------------------------------            
        }

    }

    public void ArrSet(Tile startTile)
    {
        //Vector2Int nextCoord;
        //Tile nextTile = null;
 

        foreach(Tile tile in allTiles) {
            if (tile.coord.x == startTile.coord.x+1 && tile.coord.y == startTile.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible)) 
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(0, 0, 1));
                   
                    //directions.Add (new Vector3(0, 0, 1));
                }
                
            }
            if (tile.coord.x == startTile.coord.x - 1 && tile.coord.y == startTile.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(0, 0, -1));
                    
                }
            }
            if (tile.coord.x == startTile.coord.x && tile.coord.y == startTile.coord.y + 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(1, 0, 0));
                    
                }
            }
            if (tile.coord.x == startTile.coord.x && tile.coord.y == startTile.coord.y - 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                    arr.Add(tile);
                    directions.Add(new Vector3(-1, 0, 0));
                    
                }
            }
        }
    }
    public void Chase(Tile tile)
    {
        ArrSet(tile);
        //------------------------------------------------------------------------------------------------------

        if (type)
        {
            tile.TileBurning(tile);//타일 내용 추가 필요
                                   //------------------------------------------------------------------------------------------------------
            burnedTile.Add(tile);
        }     
       

        FindAnyWay(tile);
       
    }

    public void FindAnyWay(Tile tile)
    {
      
        pre = tile;
        if(arr == null || arr.Count == 0)
        {
            Debug.Log("경로가 존재하지 않음");
            mgr_Chase.CheckMobAction();
            return;
        }

        int closestIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < arr.Count; i++) 
        {
            float distance = Vector2Int.Distance(map.playerTile.coord, arr[i].coord);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        
        StartCoroutine(MoveMob(arr[closestIndex]));
        transform.forward = directions[closestIndex];
    }


    private IEnumerator MoveMob(Tile nextTile)
    {
        //if(nextTile.tileType == TileType.impossible)
        //{
        //    Debug.Log("TileType.impossible choose Warring");
        //    yield break;
        //}
        next = nextTile;
        if (nextTile == null)
        {
            count = moveCount;
            isEnd = true;
            isDone = true;
            tile.tileType = TileType.impossible;

            mgr_Chase.CheckMobAction();
            arr.Clear();
            directions.Clear();
            yield break;
        }
        attackEnd = false;  
        if (nextTile.coord == map.playerTile.coord)
        {
            ani.SetTrigger("StartAttack");

            while (!attackEnd)
            {
                yield return null;
            }
            ChangTileType();
        }
        //map.TakeDamage(nextTile);
        ChangTileType();
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
        mgr_Chase.CheckMobAction();
    }

    private void CheckAni()
    {
        initialPosition = transform.position;
        isAnimationPlaying = true;

        ani.SetBool("Act", true);
    }

    public void Attack()
    {
        lSpear.Play();
        map.TakeDamage(next);
        //EffectManage.Instance.PlayEffect("Knight_Attack", lSpear.transform.position);
    }

    public void AttackEnd()
    {
        attackEnd = true;       
    }


    private void LateAni()
    {
        ani.SetBool("Act", false);
        isAnimationPlaying = false;
    }

    private void ChangTileType()
    {
        pre.tileType = TileType.possible;
    }

    public void DontMove()
    {
        isRope = true;
    }
    public List<Tile> ShowRange()
    {
        Debug.Log("Click mob");
        newRange.Clear();
        rangeSet(tile);
        return newRange;
    }


    public void SetStartPoint(Vector2Int sPoint, Tile cTile)
    {
        //MobData_P data = MobDataBase.instance.GetpMobData();

        //moveDir = new Vector2Int(data.moveX, data.moveY);


        map = FindObjectOfType<Map>();
        startX = sPoint.x;
        startY = sPoint.y;

        curTile = cTile;
    }

    public void DestoryMob()
    {
        curTile.tileType = TileType.possible;
        curTile.mob = null;
        EffectManage.Instance.PlayEffect("Monster_Destroy", transform.position);

        gameObject.SetActive(false);
    }

    public void rangeSet(Tile startTile)
    {
        Vector2Int nextCoord;
        Tile nextTile = null;


        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == startTile.coord.x + 1 && tile.coord.y == startTile.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                 
                    newRange.Add(tile);
                    //directions.Add (new Vector3(0, 0, 1));
                }

            }
            if (tile.coord.x == startTile.coord.x - 1 && tile.coord.y == startTile.coord.y)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                  
                    newRange.Add(tile);
                }
            }
            if (tile.coord.x == startTile.coord.x && tile.coord.y == startTile.coord.y + 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
                
                    newRange.Add(tile);
                }
            }
            if (tile.coord.x == startTile.coord.x && tile.coord.y == startTile.coord.y - 1)
            {
                if ((tile != null) && (tile.tileType == TileType.possible))
                {
              
                    newRange.Add(tile);
                }
            }
        }
    }

}
