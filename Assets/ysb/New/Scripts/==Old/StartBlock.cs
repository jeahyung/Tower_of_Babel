using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlock : Block
{
    [SerializeField]
    private PlayerMovement target;
    [SerializeField]
    private TurnManager manager_Input;

    protected override void Start()
    {
        base.Start();
    }

    public void StartGame()
    {
        ShowMoveRange();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            target = other.GetComponent<PlayerMovement>();
            StartGame();
        }
    }
}
