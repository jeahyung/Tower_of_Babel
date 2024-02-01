using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBoxPuzzleManager : PuzzleManager
{
    public SelectWordData manager_Click;

    public GameObject grid; //선택된 그리드
    public bool isAct = false;
    protected override void Awake()
    {
        base.Awake();
        manager_Click = GetComponent<SelectWordData>();
    }
    void Update()
    {
        //퍼즐 풀이가 시작됐을 때 퍼즐 조각과 상호작용 가능
        if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }

        QuitPuzzle();

        if(isAct == true)
        {
            if (manager_UI.IsBookOpen() == false)
            {
                isAct = false;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isAct == false)
            {
                //조각 찾기
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
                foreach (var hit in hits)
                {
                    if (hit.collider.CompareTag("PuzzleGrid"))
                    {
                        isAct = true;
                        grid = hit.collider.gameObject;
                        manager_UI.OpenBook();//사전 오픈
                        break;
                    }
                }
            }
            else
            {
                WordData word = manager_Click.ClickWord();
                if (word != null)
                {
                    grid.GetComponent<MovingBoxGrid>().SetWord(word);
                    manager_UI.OpenBook();
                    isAct = false;
                }
            }

        }
    }

    //==============================================필수 기능
    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //퍼즐 시작

        player.SendMessage("StopMoving", false);
        manager_UI.ShowInteractMessage(false);

        //테스트용 단어 추가
        AddWord();
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

    //==============================================for Test
    public void AddWord()
    {
        manager_UI.AddWord(words);
        wordMeanings = new List<int>();
        foreach (var word in words)
        {
            wordMeanings.Add(word.wordId);
        }
        //manager_UI.AddMeaning(wordMeanings);
    }
}
