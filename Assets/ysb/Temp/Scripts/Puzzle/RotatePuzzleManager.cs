using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class RotatePuzzleManager : PuzzleManager
{
    [SerializeField] private string fileName;
    private int maxCount = 3;
    private int[] PuzzleAnswer;
    private int[] AnswerSheet;

    private List<RotatePuzzlePiece> pieces;
    [SerializeField]
    private List<WordData> words;
    private List<int> wordMeanings;

    public Vector3 bookPos;     // y�� : ������ / z�� : ���� ķ�� z�� + 2.5f
    public float upPos;
    public float objSize;

    protected override void Awake()
    {
        base.Awake();

        PuzzleData puzzleData = SaveAndLoad.LoadPuzzleData(fileName + ".json");
        maxCount = puzzleData.maxCount;

        PuzzleAnswer = new int[maxCount];
        AnswerSheet = new int[maxCount];

        for (int i = 0; i < maxCount; ++i)
        {
            PuzzleAnswer[i] = puzzleData.Answer[i];
        }

        pieces = new List<RotatePuzzlePiece>();
        pieces.AddRange(GetComponentsInChildren<RotatePuzzlePiece>());

        int id = 0;
        foreach(var piece in pieces)
        {
            piece.SetPiece(this, id, 0);
            id++;
        }
    }
    private void Update()
    {
        if(isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //���� Ǯ�̰� ���۵��� �� ���� ������ ��ȣ�ۿ� ����
        if(Input.GetKeyDown(endkey))    // if (Input.GetKeyDown(KeyCode.Escape))
        {
            onEndPuzzle.Invoke();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 15);
            foreach (var hit in hits)
            {
                RotatePuzzlePiece piece = hit.collider.GetComponent<RotatePuzzlePiece>();
                if(piece != null)
                {
                    piece.StartRotate();
                    break;
                }
            }
        }
    }

    //���� Ǯ�� ����
    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //���� ����
        player.SendMessage("StopMoving", false); //�÷��̾� ������ ����

        //UI
        manager_UI.AddWord(words);          //���� �߰�
        manager_UI.ShowInteractMessage(false);
        manager_UI.StartPuzzleAndBookOpen(bookPos, upPos, objSize);

        //manager_UI.StartPuzzleAndBookOpen();
    }

    public override void EndPuzzleSolving()
    {
        if(isSolvingPuzzle == false || player == null) { return; }

        //�÷��̾� ������ ���� ����
        player.SendMessage("StopMoving", true);
        //����Ǯ�̰� �Ϸ� ���̶��
        if (solvedPuzzle == false)
        {
            manager_UI.ShowInteractMessage(true);
        }
        manager_UI.EndPuzzleAndBookClose();
        isSolvingPuzzle = false;
    }

    public void SetPuzzleAnswer(int aindex, int i)
    {
        AnswerSheet[aindex] = i;
        if(Enumerable.SequenceEqual(PuzzleAnswer, AnswerSheet)) 
        {
            solvedPuzzle = true;
            //���� �߰�
            wordMeanings = new List<int>();
            foreach(var word in words)
            {
                wordMeanings.Add(word.wordId);
            }
            manager_UI.AddMeaning(wordMeanings);    //�� �߰�
            onEndPuzzle.Invoke();
        }
    }
}
