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
        isSolvingPuzzle = true;             //���� ���� //��� �� ����� ����
       
        player.SendMessage("StopMoving", false); //�÷��̾� ������ ����
        manager_UI.ShowInteractMessage(false);
    }
    public override void EndPuzzleSolving()
    {
        if (isSolvingPuzzle == false || player == null) { return; }
        player.SendMessage("StopMoving", true); //�÷��̾� ������ ���� ����
        if (solvedPuzzle == false)
            manager_UI.ShowInteractMessage(true);
        isSolvingPuzzle = false;
    }

}
