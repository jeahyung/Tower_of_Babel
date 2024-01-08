using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePuzzleManager : PuzzleManager
{
    public Vector3 bookPos;     // y값 : 적당히 / z값 : 퍼즐 z값 - 1.0f
    public float upPos;
    public float objSize;

    private List<int> AnswerSheet = new List<int>();

    public Transform[] points;
    public Transform moveObj;

    public bool isMoving = false;

    public GameObject resetBtn;

    public Transform book;
    public RectTransform book_canvas;

    public GameObject puzzle_book;
    public GameObject puzzle_canvas;


    protected override void Awake()
    {
        
    }

    protected void Update()
    {
        if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //퍼즐 풀이가 시작됐을 때 퍼즐 조각과 상호작용 가능
        QuitPuzzle();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 15);
            foreach (var hit in hits)
            {
                if(hit.collider.gameObject == resetBtn)
                {
                    StartResetPuzzle();
                    break;
                }
            }
        }
    }
    public override void StartPuzzleSolving()
    {
        if (isSolvingPuzzle == true || solvedPuzzle == true || player == null) { return; }
        isSolvingPuzzle = true;             //퍼즐 시작
        player.SendMessage("StopMoving", false); //플레이어 움직임 제한

        //UI
        manager_UI.AddWord(words);          //문자 추가
        manager_UI.ShowInteractMessage(false);


        //수정해야 함
        puzzle_book.SetActive(true);
        puzzle_canvas.SetActive(true);
        //manager_UI.StartPuzzleAndBookOpen(bookPos, upPos, objSize);
    }
    public override void EndPuzzleSolving()
    {
        if (isSolvingPuzzle == false || player == null) { return; }
        player.SendMessage("StopMoving", true); //움직임 제한 해제

        //퍼즐풀이가 완료 전이라면
        if (solvedPuzzle == false)
        {
            manager_UI.ShowInteractMessage(true);
        }
        manager_UI.EndPuzzleAndBookClose();
        isSolvingPuzzle = false;

        //수정해야 함
        puzzle_book.SetActive(false);
        puzzle_canvas.SetActive(false);
    }

    public void SetPuzzleAnswer(List<int> answer)
    {
        AnswerSheet.Clear();
        AnswerSheet = answer;

        StartMovePuzzle();
    }

    public void onAn(int i)//임시
    {
        List<int> newlist = new List<int>();
        newlist.Add(i); newlist.Add(3);

        //SetAnswer(newlist);
    }


    //----------------------------------------리셋
    public void StartResetPuzzle()
    {
        if (isMoving == true) { return; }
        StartCoroutine(MovePuzzle(points[0].position));
    }

    //----------------------------------------퍼즐 이동
    public void StartMovePuzzle()
    {
        if(isMoving == true) { return; }
        //완벽한 오답
        if(AnswerSheet.Contains(words[0].wordId) == false)
        {
            Debug.Log("오답");
            return;
        }
        //작은 움직임
        if (AnswerSheet.Contains(words[0].wordId) == true && AnswerSheet.Contains(words[1].wordId) == false)
        {
            Debug.Log("작은 움직임");
            StartCoroutine(MovePuzzle(points[1].position));
            return;
        }
        //큰 움직임
        if (AnswerSheet.Contains(words[0].wordId) == true && AnswerSheet.Contains(words[1].wordId) == true)
        {
            Debug.Log("큰 움직임");
            StartCoroutine(MovePuzzle(points[2].position));
            return;
        }
    }
    private IEnumerator MovePuzzle(Vector3 pos)
    {
        isMoving = true;
        while(true)
        {
            moveObj.position = Vector3.MoveTowards(moveObj.position, pos, 1 * Time.deltaTime);
            if(moveObj.position == pos)
            {
                isMoving = false;
                yield break;
            }
            yield return null;
        }
    }

    public override void MoveBook()
    {
        book_canvas.localPosition = new Vector2(0, -870f);
        book.localPosition = new Vector2(transform.localPosition.x, -0.75f);
    }
}
