using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSelectGridMgr : MonoBehaviour
{
    private List<CrossSelectGrid> grids = new List<CrossSelectGrid>();
    public List<string> word;   

    public List<WordData> correctWords; 
    public List<string> answer = new List<string>();        

    public List<int> results;    

    private bool isComplete = false;   

    private void Awake()
    {
        grids.AddRange(GetComponentsInChildren<CrossSelectGrid>());

        for (int i = 0; i < grids.Count; ++i)
        {
            word.Add("");
            results.Add(-1);
        }
    }

    public void AddWordPiece(int id, string piece)
    {
        word[id] = "";
        word[id] = piece;

        CheckWord(id, piece);
        //CheckWord();
    }


    public void CheckWord(int id, string piece)
    {
        isComplete = false;
        foreach (var cw in correctWords)
        {
            answer.Clear();
            answer.AddRange(cw.piece);

            if (word[id] == answer[id])
            {
                results[id] = 1;
                break;
            }
            else if (word[id] != answer[id] && answer.Contains(word[id]))
            {
                results[id] = 0;
                break;
            }
            else
            {
                results[id] = -1;
            }
        }

        if (!results.Contains(0) && !results.Contains(-1) && !results.Contains(-2))
        {
            isComplete = true;
            foreach (string w in word)
            {
                if (answer.Contains(w) == false)
                {
                    isComplete = false;
                }

            }
        }
        for (int i = 0; i < grids.Count; ++i)
        {
            grids[i].SetResult(results[i]);
            if (isComplete == true)
            {
                grids[i].CompleteWord();    
            }
        }
    }

    public void CheckWord()
    {
        if (word.Contains("")) { return; }
        for (int i = 0; i < grids.Count; ++i)
        {
            results[i] = -1;
        }

        answer.Clear();
        foreach (var cw in correctWords)
        {
            if (cw.piece.Count != grids.Count) { continue; }
            if (results.Contains(0) || results.Contains(1)) { break; }

            answer.AddRange(cw.piece);
            for (int i = 0; i < grids.Count; ++i)
            {
                if (word[i] == answer[i])
                {
                    results[i] = 1;
                }
            }
            for (int i = 0; i < answer.Count; ++i)
            {
                if (word[i] != answer[i] && answer.Contains(word[i]))
                {
                    results[i] = 0;
                }
            }
        }
        //Debug.Log(word[0]);
        //Debug.Log(results[0]);

        if (!results.Contains(0) && !results.Contains(-1) && !results.Contains(-2))
        {
            isComplete = true;
        }

        for (int i = 0; i < grids.Count; ++i)
        {
            grids[i].SetResult(results[i]);
            if (isComplete == true)
            {
                grids[i].CompleteWord(); 
            }
        }


        //for (int i = 0; i < word.Count; ++i)
        //{
        //    if (results[i] != 1)
        //    {
        //        return;
        //    }
        //}
    }


    //---------------------------------------------Reset
    public void ResetPuzzle()
    {
        ResetGrids();
        foreach (var g in grids)
        {
            g.ResetGrid();
        }
    }
    private void ResetGrids()   
    {
        for (int i = 0; i < word.Count; ++i)
        {
            word[i] = "";
        }
        isComplete = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isComplete == false)
        {
            gameObject.SetActive(false);
        }
    }
}