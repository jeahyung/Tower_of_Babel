using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    private TurnManager manager_Turn;
    //돌격형 매니저
    private ChaseMobManager manager_Chase;

    //순찰형
    private PatrolMobManager manager_Patrol;



    private void Awake()
    {
        manager_Turn = FindObjectOfType<TurnManager>();
        manager_Patrol = GetComponentInChildren<PatrolMobManager>();
        manager_Chase = GetComponentInChildren<ChaseMobManager>();
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
}
