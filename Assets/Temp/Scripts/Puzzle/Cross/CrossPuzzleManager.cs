using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPiece
{
    public string meaning;
    public WordPiece(string m)
    {
        meaning = m;
    }
}
public class CrossPuzzleManager : PuzzleManager
{
    [SerializeField]
    private CrossPuzzlePiece piece = null;

    //리셋을 위한
    private List<CrossPuzzleGridMgr> gridMgrs = new List<CrossPuzzleGridMgr>();
    private List<CrossPuzzlePiece> pieces = new List<CrossPuzzlePiece>();
    private void Start()
    {
        gridMgrs.AddRange(GetComponentsInChildren<CrossPuzzleGridMgr>());
        pieces.AddRange(GetComponentsInChildren<CrossPuzzlePiece>());
    }
    void Update()
    {
        if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //퍼즐 풀이가 시작됐을 때 퍼즐 조각과 상호작용 가능
        QuitPuzzle();
        if (Input.GetMouseButtonDown(0))
        {
            //조각 찾기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 30);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("PuzzlePiece"))
                {
                    piece = hit.collider.GetComponent<CrossPuzzlePiece>();
                    piece.ResetGrid();
                    break;
                }
            }
        }
        if (piece != null)
        {
            piece.SetPos();
            if (Input.GetMouseButtonUp(0))
            {
                piece.FindGrid();
                piece = null;
            }
        }
    }

    public void ResetPuzzle()
    {
        foreach(var gm in gridMgrs)
        {
            gm.gameObject.SetActive(true);
            gm.ResetPuzzle();
        }
        foreach(var p in pieces)
        {
            p.ResetPuzzle();
        }
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

    //퍼즐이 완성됐나?
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
