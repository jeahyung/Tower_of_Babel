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
        //�׽�Ʈ �뵵
        if (action == null)
            action = FindObjectOfType<SAManager>();
        if (turn == null)
            turn = FindObjectOfType<TurnManager>();
    }

    private void OnEnable()
    {
        isStart = false;
        col.enabled = true;
    }

    private void StartGame()
    {
        StageManager.instance.StartGame();
        UpgradeManager.instance.StartGame();    //���ʽ� �� ����
        ItemInventory.instance.StartGame();     //������ ����

        action.SetAct();    //�׼� ����
        turn.StartGame();   //���� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isStart == true) { return; }
        if(other.CompareTag("Player"))
        {
            Vector3 target = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);

            other.GetComponent<PlayerMovement>().MoveToStartPoint(my);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        if (Vector3.Distance(other.transform.position, my) <= 0.05f)
        {
            col.enabled = false;
            isStart = true;

            StartGame();
        }
    }
}
