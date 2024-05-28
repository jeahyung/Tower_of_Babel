using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class TraceMonsterMovement : MonoBehaviour
{
    private ChaseMobManager mgr_Chase;
    [SerializeField] private GameObject monster;
   // [SerializeField]private GameObject player;

    [SerializeField] private Map map;
  

    [Header("몬스터 시작점")] //이건 추후 데이터 받아오는 형식으로 수정
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    
    public List<Tile> allTiles = new List<Tile>();
    [SerializeField] private Tile tile = null;
    public Vector3 nextPos;

    public TurnManager manager_Turn;

  
    public Vector2Int pos; //현재 위치 보기용이라 지워도 됨
    
    public float smoothTime = 0.2f;

    public int moveRange = 1;


    private void Awake()
    {

        //  player = GameObject.FindWithTag("Player");

        mgr_Chase = GetComponentInParent<ChaseMobManager>();
        //tile = GetComponent<Tile>();
        Tile[] tiles = FindObjectsOfType<Tile>();
        allTiles.AddRange(tiles);
    }
    private void Start()
    {
        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);
        tile = curTile;

        FindTileWithCoords(startX, startY);
        MonsterSetting(nextPos);
    }
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Think();
        }
        //pos = tile.coord;
    }

    public void MonsterSetting(Vector3 target)
    {
        if (target == Vector3.zero) { return; }


        transform.position = new Vector3(target.x, transform.position.y, target.z);

    }
  

    private void Think()
    {
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
        bool minA = nowTile.coord.x - tile.coord.x < 0;
        bool minB = nowTile.coord.y - tile.coord.y < 0;

        if(a > b)
        {
            if(minA)
            {
                FindTileWithCoords(i - 1, j);
            }
            else
            {
                FindTileWithCoords(i+1, j);
            }

        }
        else
        {
            if(minB)
            {
                FindTileWithCoords(i, j - 1);
            }
            else
            {
               FindTileWithCoords(i, j + 1);
            }
        }


        SetPosition(nextPos);

      //  AudioManager.instance.PlaySfx(AudioManager.Sfx.Monster_Move);

    }

    private void FindTileWithCoords(int targetX, int targetY)
    {
        // 조건을 만족하는 타일을 찾습니다.
        foreach (Tile tile in allTiles)
        {
            if (tile.coord.x == targetX && tile.coord.y == targetY)
            {
                nextPos = tile.GetPosition();
            }
        }
    }

   

    //이동
    public void SetPosition(Vector3 target)
    {
        if (target == Vector3.zero) { return; }

        Vector3 pos1 = new Vector3(target.x, this.transform.position.y, target.z);
        //manager_Turn.isDone = false;
        StartCoroutine(MonsterMove(pos1));
    }

    private IEnumerator MonsterMove(Vector3 target)
    {
        
        while (Vector3.Distance(transform.position, target) >= 0.05f)
        {
            Vector3 direction = target - transform.position;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref direction, smoothTime);
            yield return null;
        }
        
        transform.position = target;
        CheckTile();
      //  mgr_Chase.CheckMobAction();

        // manager_Turn.EndEnemyTurn();
        Debug.Log("Dddddd");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tile"))
        {
            tile = other.GetComponent<Tile>();
        }
    }

    public void Act()
    {
        tile.tileType = TileType.possible;
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
            }
        }
    }
}
