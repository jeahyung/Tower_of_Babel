using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSelectPuzzleManager : PuzzleManager
{
    [SerializeField]
    private CrossSelectGrid grid = null;
    public bool isAct = false;
    //리셋을 위한
    private List<CrossSelectGridMgr> gridMgrs = new List<CrossSelectGridMgr>();
    //private List<CrossPuzzlePiece> pieces = new List<CrossPuzzlePiece>();
    private void Start()
    {
        gridMgrs.AddRange(GetComponentsInChildren<CrossSelectGridMgr>());
        //pieces.AddRange(GetComponentsInChildren<CrossPuzzlePiece>());
    }


    //==============테스트
    public void SelectGrid(CrossSelectGrid g)
    {
        //if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }
        //if (isAct == true) { return; }
        //grid = g;
        //grid.SelectPiece();
    }

    //=============테스트
    void Update()
    {
        if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //퍼즐 풀이가 시작됐을 때 퍼즐 조각과 상호작용 가능
        QuitPuzzle();
        if (Input.GetMouseButtonDown(0))
        {
            if (isAct == true) { return; }
            //조각 찾기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("PuzzleGrid"))
                {
                    //piece = hit.collider.GetComponent<CrossPuzzlePiece>();
                    //piece.ResetGrid();
                    grid = hit.collider.GetComponent<CrossSelectGrid>();
                    grid.SelectPiece();
                    break;
                }
            }
        }
    }

    public void ResetPuzzle()
    {
        foreach (var gm in gridMgrs)
        {
            gm.gameObject.SetActive(true);
            gm.ResetPuzzle();
        }
        //foreach (var p in pieces)
        //{
        //    p.ResetPuzzle();
        //}
    }

    //필수 기능
    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //퍼즐 시작

        player.SendMessage("StopMoving", false);
        manager_UI.ShowInteractMessage(false);
    }
    public override void EndPuzzleSolving()
    {
        if (player == null) { return; }
        isSolvingPuzzle = false;

        player.SendMessage("StopMoving", true);
        if (solvedPuzzle == false)
        {
            manager_UI.ShowInteractMessage(true);
        }
    }

    //퍼즐이 완성됐나? - 이것도 매니저쪽으로 빼는게 좋겠는데..
    public void CompletePuzzle()
    {
        manager_UI.AddWord(words);
        wordMeanings = new List<int>();
        foreach (var word in words)
        {
            wordMeanings.Add(word.wordId);
        }
        manager_UI.AddMeaning(wordMeanings);

        onEndPuzzle.Invoke();
        solvedPuzzle = true;
    }
}
