using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    [SerializeField]
    private SAManager action;
    [SerializeField]
    private TurnManager turn;

    [SerializeField]
    private Collider col;
    private bool isStart = false;

    private void Awake()
    {
        col.enabled = true;
        //테스트 용도
        if (action == null)
            action = FindObjectOfType<SAManager>();
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

    private void OnTriggerEnter(Collider other)
    {
        if(isStart == true) { return; }
        if(other.CompareTag("Player"))
        {
            Vector3 target = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);

            other.GetComponent<PlayerMovement>().MoveToStartPoint(my);

            //if(Vector3.Distance(target, my) <= 0.05f)
            //{
            //    SetAction();
            //    StartGame();
            //    col.enabled = false;
            //    //other.transform.position = new Vector3(my.x, other.transform.position.y, my.z);
            //    isStart = true;
            //}
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        if (Vector3.Distance(other.transform.position, my) <= 0.05f)
        {
            SetAction();
            //StartGame();
            StageManager.instance.StartGame();
            col.enabled = false;
            //other.transform.position = new Vector3(my.x, other.transform.position.y, my.z);
            isStart = true;
        }
    }
}
