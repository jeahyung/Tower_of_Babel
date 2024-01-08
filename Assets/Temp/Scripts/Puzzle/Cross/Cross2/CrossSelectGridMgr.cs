using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossSelectGridMgr : MonoBehaviour
{
    private List<CrossSelectGrid> grids = new List<CrossSelectGrid>();
    public List<string> word;   //만들어진 단어

    public List<WordData> correctWords; //정답들(이 그리드에서 만들 수 있는 모든 단어)
    public List<string> answer = new List<string>();        //정답 단어 분리

    public List<int> results;       //결과(-1 = 아웃 / 0 = 볼 / 1 = 스트라이크)

    private bool isComplete = false;    //완성됐는가?

    private void Awake()
    {
        grids.AddRange(GetComponentsInChildren<CrossSelectGrid>());

        for(int i = 0; i < grids.Count; ++i)
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

    //하나씩
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
                if (answer.Contains(w) == false) {
                    isComplete = false;
                }
                
            }
        }
        //결과 뿌리기
        for (int i = 0; i < grids.Count; ++i)
        {
            grids[i].SetResult(results[i]);
            if (isComplete == true)
            {
                grids[i].CompleteWord();    //단어 완성 표시
            }
        }
    }

    //동시에
    public void CheckWord()
    {
        if (word.Contains("")) { return; }
        //결과 초기화
        for (int i = 0; i < grids.Count; ++i)
        {
            results[i] = -1;
        }

        answer.Clear();
        foreach (var cw in correctWords)
        {
            if(cw.piece.Count != grids.Count) { continue; }
            if (results.Contains(0) || results.Contains(1)) { break; }

            //판별
            answer.AddRange(cw.piece);
            for (int i = 0; i < grids.Count; ++i)
            {
                //스트라이크 판별
                if (word[i] == answer[i])
                {
                    results[i] = 1;
                }
            }
            //볼 판별
            for (int i = 0; i < answer.Count; ++i)
            {
                if(word[i] != answer[i] && answer.Contains(word[i]))
                {
                    results[i] = 0;
                }
            }
        }
        Debug.Log(word[0]);
        Debug.Log(results[0]);

        //단어가 완성됐는가?
        if (!results.Contains(0) && !results.Contains(-1) && !results.Contains(-2))
        {
            isComplete = true;
        }

        //결과 뿌리기
        for (int i = 0; i < grids.Count; ++i)
        {
            grids[i].SetResult(results[i]);
            if(isComplete == true)
            {
                grids[i].CompleteWord();    //단어 완성 표시
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
    private void ResetGrids()   //모든 글자 초기화
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
            Debug.Log("무너진다!");
            gameObject.SetActive(false);
        }
    }
}
