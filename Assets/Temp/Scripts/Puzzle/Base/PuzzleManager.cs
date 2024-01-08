using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class PuzzleManager : MonoBehaviour
{
    protected KeyCode endkey = KeyCode.Escape;

    protected GameObject player;
    [SerializeField]
    protected UIManager manager_UI;

    [SerializeField]
    protected List<WordData> words;
    protected List<int> wordMeanings;

    protected bool isSolvingPuzzle = false;
    protected bool solvedPuzzle = false;
    public bool SolvedPuzzle => solvedPuzzle;//퍼즐 해결

    public UnityEvent onStartPuzzle;
    public UnityEvent onEndPuzzle;

    protected virtual void Awake()
    {
        manager_UI = FindObjectOfType<UIManager>();
        //manager_Book = manager_UI.gameObject.GetComponentInChildren<Book>();

        onStartPuzzle.AddListener(StartPuzzleSolving);
        onEndPuzzle.AddListener(EndPuzzleSolving);
    }

    public void QuitPuzzle()
    {
        if (Input.GetKeyDown(endkey))    // if (Input.GetKeyDown(KeyCode.Escape))
        {
            onEndPuzzle.Invoke();
            return;
        }
    }
    public abstract void StartPuzzleSolving();  //퍼즐 풀이 시작
    public abstract void EndPuzzleSolving();    //퍼즐 풀이 종료

    //임시
    public virtual void MoveBook()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") == true)
        {
            player = other.gameObject;
            if (solvedPuzzle == true) { return; }
            manager_UI.ShowInteractMessage(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (player == null || solvedPuzzle == true) { return; }
            onStartPuzzle.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        onEndPuzzle.Invoke();
        manager_UI.ShowInteractMessage(false);
        player = null;
    }
}
