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
    private Cam2_Move cam2;

    [SerializeField]
    private Collider col;
    private bool isStart = false;
    private bool isPlayerEnter = false; //플레이어 들어옴?

    //private GameObject interactMessage;
    [SerializeField]
    private BoxCollider wall;   //진입 막는 벽

    private Player_Move player;

   // private HideTile hide;

    private void Awake()
    {
        col.enabled = true;
        //테스트 용도
        if (action == null)
            action = FindObjectOfType<SAManager>();
        if (turn == null)
            turn = FindObjectOfType<TurnManager>();

        if(cam2 == null) { cam2 = FindObjectOfType<Cam2_Move>(); }

       // interactMessage = GameObject.Find("Interact");
       //interactMessage.SetActive(false);
        player = FindObjectOfType<Player_Move>();
       // hide = FindObjectOfType<HideTile>();
    }
    private void Start()
    {
      //  GetComponent<Tile>().ShowArea();
    }

    private void OnEnable()
    {
        isStart = false;
        col.enabled = true;
        wall.enabled = true;

        //GetComponent<Tile>().ShowArea();
    }

    private void Update()
    {
        if(isPlayerEnter == false) { return; }

        //if(Input.GetKeyDown(KeyCode.F))
        //{
        //    wall.enabled = false;

        //    isPlayerEnter = false;
        //    isStart = true;

        //    Transform other = turn.transform;

        //    Vector3 target = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
        //    Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        //    other.GetComponent<PlayerMovement>().MoveToStartPoint(my);

        //    player.StopAni();
        //    player.RotateObject();
           


        //}
    }
    private void StartGame()
    {
        GetComponent<Tile>().HideArea();

        StageManager.instance.StartGame();
        UpgradeManager.instance.StartGame();    //보너스 턴 세팅
        ItemInventory.instance.StartGame();     //아이템 세팅

        action.SetAct();    //액션 세팅
        action.ActActionBtn(true);

        cam2.isMove = true;
        //turn.StartGame();   //게임 시작

        //interactMessage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isStart == true) { return; }
        if(other.CompareTag("Player"))
        {
            isPlayerEnter = true;
            //    interactMessage.SetActive(true);

            wall.enabled = false;

                isPlayerEnter = false;
                //isStart = true;

                Transform another = turn.transform;

                Vector3 target = new Vector3(another.transform.position.x, another.transform.position.y, another.transform.position.z);
                Vector3 my = new Vector3(transform.position.x, another.transform.position.y, transform.position.z);
            another.GetComponent<PlayerMovement>().MoveToStartPoint(my);

                player.StopAni();
                player.RotateObject();

            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vector3 my = new Vector3(transform.position.x, other.transform.position.y, transform.position.z);
        if (Vector3.Distance(other.transform.position, my) <= 0.05f)
        {
            col.enabled = false;

            if (!isStart)
            {
                StartGame();
                isStart = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isPlayerEnter = false;
        //interactMessage.SetActive(false);
    }
}
