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

    public Vector3 bookPos;     // y값 : 적당히 / z값 : 퍼즐 캠의 z값 + 2.5f
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
        if(isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //퍼즐 풀이가 시작됐을 때 퍼즐 조각과 상호작용 가능
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

    //퍼즐 풀이 시작
    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //퍼즐 시작
        player.SendMessage("StopMoving", false); //플레이어 움직임 제한

        //UI
        manager_UI.AddWord(words);          //문자 추가
        manager_UI.ShowInteractMessage(false);
        manager_UI.StartPuzzleAndBookOpen(bookPos, upPos, objSize);

        //manager_UI.StartPuzzleAndBookOpen();
    }

    public override void EndPuzzleSolving()
    {
        if(isSolvingPuzzle == false || player == null) { return; }

        //플레이어 움직임 제한 해제
        player.SendMessage("StopMoving", true);
        //퍼즐풀이가 완료 전이라면
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
            //뜻을 추가
            wordMeanings = new List<int>();
            foreach(var word in words)
            {
                wordMeanings.Add(word.wordId);
            }
            manager_UI.AddMeaning(wordMeanings);    //뜻 추가
            onEndPuzzle.Invoke();
        }
    }
}
