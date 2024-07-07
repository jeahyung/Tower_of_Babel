using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TraceMonsterMovement : MonoBehaviour, Mob
{
    private ChaseMobManager mgr_Chase;
    private bool ch = true;

    [SerializeField] private GameObject monster;
   // [SerializeField]private GameObject player;

    [SerializeField] private Map map;
  

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    
    public List<Tile> allTiles = new List<Tile>();
    [SerializeField] private Tile tile = null;
   // private Tile tile1 = null;
    public Vector3 nextPos;

    public TurnManager manager_Turn;
    public bool minA;
    public bool minB;
  
    public Vector2Int pos; //현재 위치 보기용이라 지워도 됨
    
    public float smoothTime = 0.2f;

    public int moveRange = 1;
    public GameObject effectPrefab;

    public bool isRope = false; //로프에 걸렸는가?

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
        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);

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
     
        //pos = tile.coord;
    }

    public void MonsterSetting(Vector3 target)
    {
        if (target == Vector3.zero) { return; }


        transform.position = new Vector3(target.x, transform.position.y, target.z);

    }
  

    private void Think()
    {
        tile.tileType = TileType.possible;
        tile.mob = null;
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

        ch = true;
        Think();     
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

}
