using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    [SerializeField]
    PlayerMovement player;
    public bool canJump = true;
    public bool CanJump
    {
        get { return canJump; }
        set { canJump = value; }
    }

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
    }
    public void PlayerMove()
    {
        player.StartMove();
    }

    public void PlayerRotate()
    {
        player.Rotate_Phys();
    }



    public void PlayerJump()
    {
        //if(canJump == false) //������ �� ���ٸ� �� ���� - ���� �� �ǵ��ƿ� �� �ߵ�
        //{
        //    player.EndPlayerTurn();
        //    canJump = true;
        //    return;
        //}
        ////������ �� �ִٸ� �����Ѵ�
        //player.StartJump();
        //canJump = false;
    }

    public void GameOver()
    {
        player.GameOver();
    }
}
