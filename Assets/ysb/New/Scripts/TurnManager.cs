using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Map manager_map;
    private PlayerMovement player;

    public bool isPlayerTurn = false;
    public bool isEnemyTurn = true;

    public bool isDone = true;

    public bool IsMyTurn => isPlayerTurn;

    private void Start()
    {
        manager_map = FindObjectOfType<Map>();
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    public void StartPlayerTurn()
    {
        if(isEnemyTurn == true) { return; }
        isPlayerTurn = true;
        isDone = true;
        manager_map.StartPlayerTurn(player.moveRange);

        Debug.Log("player turn");
    }

    public void EndPlayerTurn()
    {
        Debug.Log("player turn end");
        isPlayerTurn = false;
        isDone = true;

        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        if(isPlayerTurn == true) { return; }
        isEnemyTurn = true;
        Debug.Log("몬스터 턴");

        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        isEnemyTurn = false;
        Debug.Log("몬스터 턴 end");
        StartPlayerTurn();
    }

}
