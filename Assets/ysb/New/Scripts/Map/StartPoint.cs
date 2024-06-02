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
    private bool isPlayerEnter = false; //플레이어 들어옴?

    private GameObject interactMessage;
    [SerializeField]
    private BoxCollider wall;   //진입 막는 벽
    private void Awake()
    {
        col.enabled = true;
        //테스트 용도
        if (action == null)
            action = FindObjectOfType<SAManager>();
        if (turn == null)
            turn = FindObjectOfType<TurnManager>();

        interactMessage = GameObject.Find("Interact");
        interactMessage.SetActive(false);
    }
    private void Start()
    {
        GetComponent<Tile>().ShowArea();
    }

    private void OnEnable()
    {
        isStart = false;
        col.enabled = true;
        wall.enabled = true;

        GetComponent<Tile>().ShowArea();
    }

    private void Update()
    {
        if(isPlayerEnter == false) { return; }

        if(Input.GetKeyDown(KeyCode.F))
        {
            wall.enabled = false;

            isPlayerEnter = false;
            isStart = true;

            Transform other = turn.transform;

            Vector3 target = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
            Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
            other.GetComponent<PlayerMovement>().MoveToStartPoint(my);
        }
    }
    private void StartGame()
    {
        GetComponent<Tile>().HideArea();

        StageManager.instance.StartGame();
        UpgradeManager.instance.StartGame();    //보너스 턴 세팅
        ItemInventory.instance.StartGame();     //아이템 세팅

        action.SetAct();    //액션 세팅
        turn.StartGame();   //게임 시작

        interactMessage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isStart == true) { return; }
        if(other.CompareTag("Player"))
        {
            isPlayerEnter = true;
            interactMessage.SetActive(true);
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
    private void OnTriggerExit(Collider other)
    {
        isPlayerEnter = false;
        interactMessage.SetActive(false);
    }
}
