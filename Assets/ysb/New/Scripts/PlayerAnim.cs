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
        //if(canJump == false) //점프할 수 없다면 턴 종료 - 착지 후 되돌아올 때 발동
        //{
        //    player.EndPlayerTurn();
        //    canJump = true;
        //    return;
        //}
        ////점프할 수 있다면 점프한다
        //player.StartJump();
        //canJump = false;
    }

    public void GameOver()
    {
        player.GameOver();
    }
}
