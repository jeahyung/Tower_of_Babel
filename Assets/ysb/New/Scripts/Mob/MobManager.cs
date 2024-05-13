using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    private TurnManager manager_Turn;
    //������ �Ŵ���

    //������
    private PatrolMobManager manager_Patrol;



    private void Awake()
    {
        manager_Turn = FindObjectOfType<TurnManager>();
        manager_Patrol = GetComponentInChildren<PatrolMobManager>();        
    }

    public void ActMob()
    {
        manager_Patrol.StartActMob();
    }



    public void EndPatrol()
    {
        manager_Turn.EndEnemyTurn();
    }
}
