using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField]
    private SpecialActionManager action;
    [SerializeField]
    private TurnManager turn;

    private bool isStart = false;

    private void Awake()
    {
        //테스트 용도
        if (action == null)
            action = FindObjectOfType<SpecialActionManager>();
        if (turn == null)
            turn = FindObjectOfType<TurnManager>();
    }

    private void SetAction()
    {
        action.SetAct();
    }
    private void StartGame()
    {
        turn.StartGame();
    }

    private void OnTriggerStay(Collider other)
    {
        if(isStart == true) { return; }
        if(other.CompareTag("Player"))
        {
            Vector3 target = new Vector3(other.transform.position.x, 0, other.transform.position.z);
            Vector3 my = new Vector3(transform.position.x, 0, transform.position.z);

            if(Vector3.Distance(target, my) <= 0.05f)
            {
                SetAction();
                StartGame();

                other.transform.position = new Vector3(my.x, other.transform.position.y, my.z);
                isStart = true;
            }

        }
    }
}
