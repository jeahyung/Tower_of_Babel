using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePuzzleManager : PuzzleManager
{
    public Vector3 bookPos;     // y�� : ������ / z�� : ���� z�� - 1.0f
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
        if (isSolvingPuzzle == false || solvedPuzzle == true) { return; }  //���� Ǯ�̰� ���۵��� �� ���� ������ ��ȣ�ۿ� ����
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
        isSolvingPuzzle = true;             //���� ����
        player.SendMessage("StopMoving", false); //�÷��̾� ������ ����

        //UI
        manager_UI.AddWord(words);          //���� �߰�
        manager_UI.ShowInteractMessage(false);


        //�����ؾ� ��
        puzzle_book.SetActive(true);
        puzzle_canvas.SetActive(true);
        //manager_UI.StartPuzzleAndBookOpen(bookPos, upPos, objSize);
    }
    public override void EndPuzzleSolving()
    {
        if (isSolvingPuzzle == false || player == null) { return; }
        player.SendMessage("StopMoving", true); //������ ���� ����

        //����Ǯ�̰� �Ϸ� ���̶��
        if (solvedPuzzle == false)
        {
            manager_UI.ShowInteractMessage(true);
        }
        manager_UI.EndPuzzleAndBookClose();
        isSolvingPuzzle = false;

        //�����ؾ� ��
        puzzle_book.SetActive(false);
        puzzle_canvas.SetActive(false);
    }

    public void SetPuzzleAnswer(List<int> answer)
    {
        AnswerSheet.Clear();
        AnswerSheet = answer;

        StartMovePuzzle();
    }

    public void onAn(int i)//�ӽ�
    {
        List<int> newlist = new List<int>();
        newlist.Add(i); newlist.Add(3);

        //SetAnswer(newlist);
    }


    //----------------------------------------����
    public void StartResetPuzzle()
    {
        if (isMoving == true) { return; }
        StartCoroutine(MovePuzzle(points[0].position));
    }

    //----------------------------------------���� �̵�
    public void StartMovePuzzle()
    {
        if(isMoving == true) { return; }
        //�Ϻ��� ����
        if(AnswerSheet.Contains(words[0].wordId) == false)
        {
            Debug.Log("����");
            return;
        }
        //���� ������
        if (AnswerSheet.Contains(words[0].wordId) == true && AnswerSheet.Contains(words[1].wordId) == false)
        {
            Debug.Log("���� ������");
            StartCoroutine(MovePuzzle(points[1].position));
            return;
        }
        //ū ������
        if (AnswerSheet.Contains(words[0].wordId) == true && AnswerSheet.Contains(words[1].wordId) == true)
        {
            Debug.Log("ū ������");
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
