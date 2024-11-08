using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour
{
    private Map map;
    private Animator anim;
    public PatrolMobManager pMgr;
    public ChaseMobManager cMgr;

    [Header("���� ������")]
    [SerializeField] private int startX;
    [SerializeField] private int startY;

    [Header("����")]
    [SerializeField] private int range = 1;   //����
    [SerializeField] private int mobCount = 5;  //��ȯ ����

    List<Vector2Int> pos = new List<Vector2Int>();
    List<Tile> SpawnTile = new List<Tile>();    //��ȯ�� Ÿ��

    [Header("����")]
    [SerializeField] private int[] SpawnLimitCount;
    [SerializeField] private List<GameObject> mobList = new List<GameObject>(); //���� �� �ִ� ��

    private int[] SpawnCount;

    private List<List<GameObject>> mobs = new List<List<GameObject>>();
    private List<GameObject> createMob = new List<GameObject>();    //���� ��

    [SerializeField] private float[] createPer; //���� ����
    [SerializeField] private float[] countPer;  //ü�̽� ���� Ȯ��
    [SerializeField] private float[] countPer_NoneChace;    //ü�̽� ���� Ȯ��

    [SerializeField] private int ChaceLimit = 0;    //�߰��� ����
    private int chaceCount = 0; //������� �߰���

    private void Awake()
    {
        map = FindObjectOfType<Map>();
        pMgr = FindObjectOfType<PatrolMobManager>();
        cMgr = FindObjectOfType<ChaseMobManager>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Tile curTile = map.GetTile(map.tiles[startX, startY].coord);
        Vector3 pos = new Vector3(curTile.GetPosition().x, curTile.GetPosition().y + 2, curTile.GetPosition().z);
        transform.position = pos;
        curTile.tileType = TileType.impossible;

        SpawnCount = new int[SpawnLimitCount.Length];
        for(int i = 0; i < SpawnCount.Length; ++i) { SpawnCount[i] = 0; }

        CreateMob();
        SetPositionData();
    }

    void CreateMob()
    {
        for(int i = 0; i < mobList.Count; ++i)
        {
            mobs.Add(new List<GameObject>());
            for (int j = 0; j < SpawnLimitCount[SpawnLimitCount.Length - 1]; ++j)
            {
                GameObject tmob = Instantiate(mobList[i]);
                tmob.SetActive(false);
                mobs[i].Add(tmob);
            }
            Debug.Log(mobs[i].Count);
        }
    }

    void SetPositionData()
    {
        pos.Clear();
        int count = range * 2 + 1;
        for (int i = 0; i < count; ++i)
        {
            for(int j = 0; j < count; ++j)
            {
                pos.Add(new Vector2Int(startX + j - 1, startY + i - 1));
            }
        }
        pos.Remove(new Vector2Int(startX, startY));

        Tile playerTile = map.nowTile;
        if (pos.Contains(playerTile.coord)) { pos.Remove(playerTile.coord); }

        Tile curTile = map.GetTile(map.tiles[startX, startY].coord); 
        if(curTile == null) { return; }
        curTile.tileType = TileType.impossible;
    }

    //Act
    public void ActBoss()
    {
        anim.SetTrigger("isAttack");
    }
    public void Attack()
    {
        SpawnMob();
    }

    public void DestroyMob()
    {
        //Ÿ�� �ʱ�ȭ
        for(int i = 0; i < SpawnTile.Count; ++i) { SpawnTile[i].InitTile(); }
        SpawnTile.Clear();
        //�� ����
        for (int i = 0; i < createMob.Count; ++i)
        {
            createMob[i].transform.position = transform.position;
            createMob[i].GetComponent<Mob>().DestoryMob();
        }
        createMob.Clear();
        for (int i = 0; i < SpawnCount.Length; ++i) { SpawnCount[i] = 0; }

        //�Ŵ���
        pMgr.InitMob();
        cMgr.InitMob();
    }

    public void SpawnMob()
    {
        DestroyMob();
        SetPositionData();

        //� �����?
        int count = 0;
        float rand = Random.value;
        if (rand <= createPer[createPer.Length - 1]) { count = SpawnLimitCount[createPer.Length - 1]; }
        else if (rand <= createPer[createPer.Length - 1] + createPer[createPer.Length - 2]) { count = SpawnLimitCount[createPer.Length - 2]; }
        else { count = SpawnLimitCount[0]; }
        mobCount = count;

        //�� ����
        for (int i = 0; i < mobCount; ++i)
        {
            GameObject mob = SelectMob();
            if(mob != null)
            {
                createMob.Add(mob);

                Vector2Int pos = SetStartPoint();
                Tile tile = map.GetTile(pos);
                SpawnTile.Add(tile);
                mob.SetActive(true);
                mob.GetComponent<Mob>().SetStartPoint(pos, tile);

                tile.tileType = TileType.impossible;
                tile.mob = mob.GetComponent<Mob>();

                pMgr.AddMob(mob.GetComponent<MobMovement>());
                cMgr.AddMob(mob.GetComponent<TraceMonsterMovement>());
            }
        }
    }

    GameObject CreateMobNoneChase()
    {
        float rand = Random.value;
        if (chaceCount < ChaceLimit)
        {
            if (rand <= countPer[0])
            {
                for (int j = 0; j < mobs[0].Count; ++j)
                {
                    if (mobs[0][j].activeSelf == false)
                    {
                        chaceCount++;
                        return mobs[0][j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < mobs[1].Count; ++j)
                {
                    if (mobs[1][j].activeSelf == false)
                    {
                        return mobs[1][j];
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < mobs[1].Count; ++j)
            {
                if (mobs[1][j].activeSelf == false)
                {
                    return mobs[1][j];
                }
            }
        }
        return null;
    }
    GameObject SelectMob()
    {
        float rand = Random.value;

        if(countPer.Length == 2)
        {
            return CreateMobNoneChase();
        }

        if(chaceCount < ChaceLimit)
        {
            if (rand <= countPer[0])
            {
                for (int j = 0; j < mobs[0].Count; ++j)
                {
                    if (mobs[0][j].activeSelf == false)
                    {
                        chaceCount++;
                        return mobs[0][j];
                    }
                }
            }
            else if (rand <= countPer[0] + countPer[1])
            {
                for (int j = 0; j < mobs[1].Count; ++j)
                {
                    if (mobs[1][j].activeSelf == false)
                    {
                        return mobs[1][j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < mobs[2].Count; ++j)
                {
                    if (mobs[2][j].activeSelf == false)
                    {
                        return mobs[2][j];
                    }
                }
            }
        }
        else
        {
            if(rand <= countPer_NoneChace[1])
            {
                for (int j = 0; j < mobs[1].Count; ++j)
                {
                    if (mobs[1][j].activeSelf == false)
                    {
                        return mobs[1][j];
                    }
                }
            }
            else
            {
                for (int j = 0; j < mobs[2].Count; ++j)
                {
                    if (mobs[2][j].activeSelf == false)
                    {
                        return mobs[2][j];
                    }
                }
            }
        }
        return null;

        //for (int i = 0; i < mobList.Count; ++i)
        //{
        //    if(SpawnCount[i] >= SpawnLimitCount[i]) { continue; }

        //    if(i == mobList.Count - 1)
        //    {
        //        for (int j = 0; j < mobs[i].Count; ++j)
        //        {
        //            if (mobs[i][j].activeSelf == false) { 
        //                SpawnCount[i]++; 
        //                return mobs[i][j]; }

        //        }
        //    }

        //    int canSpawn = Random.Range(0, 2);
        //    if(canSpawn == 0)
        //    {
        //        for (int j = 0; j < mobs[i].Count; ++j)
        //        {
        //            if (mobs[i][j].activeSelf == false) { 
        //                SpawnCount[i]++; 
        //                return mobs[i][j]; }

        //        }
        //    }
        //}
        //return null;
    }

    Vector2Int SetStartPoint()
    {
        Vector2Int sPoint;
        sPoint = pos[Random.Range(0, pos.Count)];
        pos.Remove(sPoint);
        return sPoint;
    }
}
