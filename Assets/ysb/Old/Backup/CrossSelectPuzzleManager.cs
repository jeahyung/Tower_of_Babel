using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSelectPuzzleManager : PuzzleManager
{
    [SerializeField]
    private CrossSelectGrid grid = null;
    public bool isAct = false;
    //������ ����
    private List<CrossSelectGridMgr> manager_Grids;
    private PieceManager manager_Piece;
    private void Start()
    {
        manager_Grids = new List<CrossSelectGridMgr>();
        manager_Grids.AddRange(GetComponentsInChildren<CrossSelectGridMgr>());

        manager_Piece = FindObjectOfType<PieceManager>();
    }
    void Update()
    {
        if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //���� Ǯ�̰� ���۵��� �� ���� ������ ��ȣ�ۿ� ����
        QuitPuzzle();
        if (Input.GetMouseButtonDown(0))
        {
            if (isAct == true) { return; }
            //���� ã��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("PuzzleGrid"))
                {
                    grid = hit.collider.GetComponent<CrossSelectGrid>();
                    grid.SelectPiece();
                    break;
                }
            }
        }
    }

    public void ResetPuzzle()
    {
        foreach (var gm in manager_Grids)
        {
            gm.gameObject.SetActive(true);
            gm.ResetPuzzle();
        }
        manager_Piece.ResetPanel();
    }

    //==============================================�ʼ� ���
    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //���� ����

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

        if(grid != null)
        {
            EndSelect();
        }
    }

    //������ �ϼ��Ƴ�? - �̰͵� �Ŵ��������� ���°� ���ڴµ�..
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

    public void EndSelect()
    {
        grid.ClosePanel();
        isAct = false;
    }
}
