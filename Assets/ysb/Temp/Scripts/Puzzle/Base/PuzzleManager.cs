using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class PuzzleManager : MonoBehaviour
{
    protected KeyCode endkey = KeyCode.Escape;

    protected GameObject player;
    protected UIManager manager_UI;
    //protected Book manager_Book;
    protected bool isSolvingPuzzle = false;
    protected bool solvedPuzzle = false;
    public bool SolvedPuzzle => solvedPuzzle;//∆€¡Ò «ÿ∞·

    public UnityEvent onStartPuzzle;
    public UnityEvent onEndPuzzle;

    protected virtual void Awake()
    {
        manager_UI = GameObject.Find("Canvas").GetComponent<UIManager>();
        //manager_Book = manager_UI.gameObject.GetComponentInChildren<Book>();
    }
    protected virtual void Start()
    {
        onStartPuzzle.AddListener(StartPuzzleSolving);
        onEndPuzzle.AddListener(EndPuzzleSolving);
    }
    public abstract void StartPuzzleSolving();  //∆€¡Ò «Æ¿Ã Ω√¿€
    public abstract void EndPuzzleSolving();    //∆€¡Ò «Æ¿Ã ¡æ∑·

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
