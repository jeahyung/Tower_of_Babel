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

    private void Awake()
    {
        if (turn == null)
            turn = FindObjectOfType<TurnManager>();
        if (up == null)
            up = FindObjectOfType<UpgradeController>();
    }
    private void EndGame()
    {
        turn.StartPlayerTurn();
    }

    private void OnTriggerStay(Collider other)
    {
        if(isEnd == true) { return; }
        if (other.CompareTag("Player"))
        {
            Vector3 target = new Vector3(other.transform.position.x, 0, other.transform.position.z);
            Vector3 my = new Vector3(transform.position.x, 0, transform.position.z);

            if (Vector3.Distance(target, my) <= 0.05f)
            {
                other.transform.position = new Vector3(my.x, other.transform.position.y, my.z);
                EndGame();
                up.SetSelectList();

                isEnd = true;
            }

        }
    }
}
