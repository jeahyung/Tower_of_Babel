using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    [SerializeField]
    private TurnManager turn;
    [SerializeField]
    private UpgradeController up;

    private bool isEnd = false;
    private Player_Move player;

    public GameObject wall = null;
    private void Awake()
    {
        if (turn == null)
            turn = FindObjectOfType<TurnManager>();
        if (up == null)
            up = FindObjectOfType<UpgradeController>();
        player = FindObjectOfType<Player_Move>();

        if(wall == null) { wall = GameObject.Find("wall_end"); }
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        isEnd = false;
        wall.SetActive(false);
        // GetComponent<Tile>().effectPrefab.SetActive(false);
    }
    private void EndGame()
    {
        ScoreManager.instance.CalculateScore();
        Camera.main.SendMessage("BackMainCam");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Stage_Clear);
    }

    private void OnTriggerStay(Collider other)
    {
        player.StartAni();
        if (isEnd == true) { return; }
        if (other.CompareTag("Player"))
        {
            Vector3 target = new Vector3(other.transform.position.x, 0, other.transform.position.z);
            Vector3 my = new Vector3(transform.position.x, 0, transform.position.z);

            if (Vector3.Distance(target, my) <= 0.05f)
            {
                other.transform.position = new Vector3(my.x, other.transform.position.y, my.z);

                EndGame();
                //업그레이드
                //up.SetSelectList();

                isEnd = true;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(isEnd == true && other.CompareTag("Player"))
        {
            wall.SetActive(true);
        }
    }
}
