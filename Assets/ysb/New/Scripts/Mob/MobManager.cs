using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    private TurnManager manager_Turn;
    //돌격형 매니저
    private ChaseMobManager manager_Chase;

    private List<Mob> mobs = new List<Mob>();

    private List<Rook> rooks = new List<Rook>();   //룩들

    //순찰형
    private PatrolMobManager manager_Patrol;

    private void Awake()
    {
        manager_Turn = FindObjectOfType<TurnManager>();
        manager_Patrol = GetComponentInChildren<PatrolMobManager>();
        manager_Chase = GetComponentInChildren<ChaseMobManager>();

        mobs.AddRange(GetComponentsInChildren<Mob>());
        rooks.AddRange(GetComponentsInChildren<Rook>());
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
        if(manager_Chase == null) {
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
}
