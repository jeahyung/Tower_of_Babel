using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour//Singleton<MobManager>
{
    private Map map;
    private TurnManager manager_Turn;
    //돌격형 매니저
    private ChaseMobManager manager_Chase;

    private List<Mob> mobs = new List<Mob>();

    private List<Rook> rooks = new List<Rook>();   //룩들

    //순찰형
    private PatrolMobManager manager_Patrol;

    private Queen boss1;

    //범위표시
    private Mob clickMob = null;

    public bool isKey = false;
    public bool isKey2 = false;

    public bool isUseItem = false;

    private void Start()
    {
        if(map == null) { map = FindObjectOfType<Map>(); }
        manager_Turn = FindObjectOfType<TurnManager>();
        manager_Patrol = GetComponentInChildren<PatrolMobManager>();
        manager_Chase = GetComponentInChildren<ChaseMobManager>();

        mobs.AddRange(GetComponentsInChildren<Mob>());
        rooks.AddRange(GetComponentsInChildren<Rook>());

        if(boss1 == null) { boss1 = GetComponentInChildren<Queen>(); }
    }

    private void Update()
    {
        //몹 범위 표시때 플레이어는 이동하지 못하는가?
        if (StageManager.instance.isPlaying == false) { return; }    //게임 시작 여부
        if (Input.GetMouseButtonDown(0))
        {
            if (clickMob != null) 
            { 
                HideMobRange(clickMob);
                if (!isUseItem) { ShowPlayerRange(); }

                clickMob = null; 
            }

            Mob mob = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
            foreach (var hit in hits)
            {
                if(isKey)
                {
                    if(hit.collider.GetComponent<Rook>() != null)
                    {
                        hit.collider.SendMessage("OpenRook");
                        isKey = false;
                        manager_Turn.gameObject.SendMessage("UseItem");
                        return;
                    }
                }

                if (hit.collider.GetComponent<Mob>() != null)
                {
                    mob = hit.collider.GetComponent<Mob>();
                    clickMob = mob;

                    if(isKey2 || isKey && hit.collider.GetComponent<MobMovement>() != null)
                    {
                        mob.DestoryMob();
                        manager_Patrol.RemoveMob(hit.collider.GetComponent<MobMovement>());
                        HideMobRange(clickMob);
                        clickMob = null;
                        isKey = false;
                        manager_Turn.gameObject.SendMessage("UseItem");
                        return;
                    }

                    if (isKey2 && hit.collider.GetComponent<TraceMonsterMovement>() != null)
                    {
                        mob.DestoryMob();
                        manager_Chase.RemoveMob(GetComponent<TraceMonsterMovement>());
                        HideMobRange(clickMob);
                        clickMob = null;
                        isKey2 = false;
                        manager_Turn.gameObject.SendMessage("UseItem");
                        return;
                    }

                    ShowMobRange(mob);
                    return;
                }
            }
        }
    }

    public void HideAllRange()
    {
        if (clickMob != null)
        {
            HideMobRange(clickMob);
            clickMob = null;
        }
    }
    void ShowMobRange(Mob mob)
    {
        map.ShowMobRange(mob.ShowRange());
    }
    void HideMobRange(Mob mob)
    {
        map.HideMobRange(mob.ShowRange());
    }
    void ShowPlayerRange()
    {
        map.AgainShowTile();
    }

    public void StartGame()
    {
        for(int i = 0; i < rooks.Count; ++i)
        {
            rooks[i].ResetMob();
        }
    }
    public void ActMob()
    {
        if (manager_Chase == null) {
            manager_Patrol.StartActMob();
            return;        
        }
        manager_Chase.StartActMob();
    }
    public void EndChase()
    {
        if(manager_Patrol == null)
        {
            EndPatrol();
            return;
        }
        manager_Patrol.StartActMob();
    }
    public void EndPatrol()
    {
        manager_Turn.EndEnemyTurn();
    }


    //룩 보여주기
    public List<Tile> ShowRook()
    {
        List<Tile> tiles = new List<Tile>();
        for(int i = 0; i < rooks.Count; ++i)
        {
            tiles.Add(rooks[i].ShowRookTile());
        }
        return tiles;
    }
    
    public List<Tile> ShowMob()
    {
        List<Tile> tiles = new List<Tile>();
        for(int i = 0; i < mobs.Count; ++i)
        {
            tiles.Add(mobs[i].ShowTile());
        }
        return tiles;
    }

    public float ActBoss()
    {
        if(boss1 != null)
        {
            boss1.ActBoss();
            return 1f;
        }
        return 0f;
    }


    public List<Tile> ShowMob_key1()
    {
        List<Tile> tiles = new List<Tile>();
        tiles.AddRange(manager_Patrol.ShowMobTile());
        tiles.AddRange(ShowRook());
        return tiles;
    }

    public List<Tile> ShowMob_key2()
    {
        List<Tile> tiles = new List<Tile>();
        tiles.AddRange(ShowMob_key1());
        tiles.AddRange(manager_Chase.ShowMobTile());
        return tiles;
    }


    public void RemoveMob_UseKey1(MobMovement m)
    {
        manager_Patrol.RemoveMob(m);
        mobs.Remove(m.GetComponent<Mob>());
    }

    public void DontMovePatrol()
    {
        List<Mob> pMob = new List<Mob>();
        pMob.AddRange(manager_Patrol.GetPatrol());

        foreach(Mob m in pMob)
        {
            m.DontMove();
        }
    }

    public void DontMoveChase()
    {
        List<Mob> cMob = new List<Mob>();
        cMob.AddRange(manager_Chase.GetChase());

        foreach (Mob m in cMob)
        {
            m.DontMove();
        }
    }
    
}
