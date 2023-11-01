using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : PuzzleManager
{
    private void Update()
    {
        if (Input.GetKeyDown(endkey))    // if (Input.GetKeyDown(KeyCode.Escape))
        {
            onEndPuzzle.Invoke();
            return;
        }
    }

    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //퍼즐 시작 //요길 내 퍼즐로 수정
       
        player.SendMessage("StopMoving", false); //플레이어 움직임 제한
        manager_UI.ShowInteractMessage(false);
    }
    public override void EndPuzzleSolving()
    {
        if (isSolvingPuzzle == false || player == null) { return; }
        player.SendMessage("StopMoving", true); //플레이어 움직임 제한 해제
        if (solvedPuzzle == false)
            manager_UI.ShowInteractMessage(true);
        isSolvingPuzzle = false;
    }

}
