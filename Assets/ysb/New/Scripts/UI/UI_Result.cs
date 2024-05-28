using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Result : MonoBehaviour
{
    private List<ClearScore> sText = new List<ClearScore>();
    public TMP_Text total;

    public GameObject back;
    public GameObject panel;

    public TMP_Text text_Stage; //몇 챕터, 몇 스테이지?
    List<string> textContent = new List<string>();
    List<int> score = new List<int>();

    private int sIndex = 0;
    private int sCount = 0;     //내역 수
    private float height = 0;     //텍스트 크기(높이)

    private int totalScore = 0; //올릴 점수
    private int curScore = 0;

    private bool isClickOk = false;

    private void Start()
    {
        sText.AddRange(GetComponentsInChildren<ClearScore>());
        foreach(var t in sText)
        {
            t.enabled = false;
        }
        height = 50;

        back.SetActive(false);
        panel.SetActive(false);

        text_Stage = panel.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    private void SetText()
    {
        textContent.Clear();
        textContent.Add("스테이지 클리어 : ");
        textContent.Add("제한 턴 수 내로 클리어 : ");
        textContent.Add("잔여 특수 행동 횟수 : ");

        for(int i = 0; i < score.Count; ++i)
        {
            textContent[i] += score[i].ToString();
        }
    }
    public void ShowResult(int sc, int cur, int target, List<int> scoreList)
    {
        score = scoreList;
        SetText();  //문구 설정

        sCount = sc;
        sIndex = 0;

        curScore = cur;
        totalScore = target;

        back.SetActive(true);
        StartCoroutine(Result());
    }

    private IEnumerator Result()
    {
        text_Stage.text = StageManager.instance.GetChapterCount.ToString() + " - "
            + StageManager.instance.GetStageCount.ToString() + " Stage Clear";

        yield return new WaitForSeconds(0.2f);
        panel.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        ShowScore();
    }
    public void ShowScore()
    {
        total.enabled = true;
        total.text = curScore.ToString();

        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        while(sIndex < sCount)
        {
            float h = height * (sCount - sIndex - 2);
            sText[sIndex].SetText(textContent[sIndex]);
            sText[sIndex].SetTarget(h);
            sIndex++;
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.6f);

        StartCoroutine(Count(totalScore, curScore));
    }

    IEnumerator Count(float target, float current)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            total.text = "ToTal : " + ((int)current).ToString();
            yield return null;
        }

        current = target;
        total.text = "ToTal : " + ((int)current).ToString();

        StageManager.instance.isPlaying = false;
        isClickOk = true;
    }


    public void HidePanel()
    {
        if(isClickOk == false) { return; }
        back.SetActive(false);
        panel.SetActive(false);

        for(int i = 0; i < sCount; ++i)
        {
            sText[i].HideText();
        }

        StageManager.instance.EndGame();
    }
}
