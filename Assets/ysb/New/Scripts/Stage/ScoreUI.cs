using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text sumScore;

    public int hIndex = -1;
    public TMP_Text[] h_Text;   //점수 내역

    public ScoreHistroy[] history;

    public List<RectTransform> showH = new List<RectTransform>();
    public float h = 0;

    private void Awake()
    {
        history = GetComponentsInChildren<ScoreHistroy>();
        h_Text = new TMP_Text[history.Length];
        for(int i = 0; i < history.Length; ++i)
        {
            h_Text[i] = history[i].GetComponentInChildren<TMP_Text>();
            h_Text[i].text = "";
            history[i].gameObject.SetActive(false);
        }
        h = -history[0].GetComponent<RectTransform>().rect.height;

        //h_Text = GetComponentsInChildren<TMP_Text>();
        //for(int i = 0; i < h_Text.Length; ++i)
        //{
        //    h_Text[i].text = "";
        //    h_Text[i].gameObject.SetActive(false);
        //}
        //h = -h_Text[0].GetComponentInParent<RectTransform>().rect.height;
        //sumScore.text = "";
    }
    public TMP_Text FindHistory()
    {
        //다음 내역을 찾는다 + 갱신한 내역을 맨 위로 올린다.
        hIndex++;
        if(hIndex >= history.Length)
        {
            hIndex = 0;
        }
        history[hIndex].gameObject.SetActive(true);
        showH.Add(history[hIndex].GetComponent<RectTransform>());
        showH[showH.Count - 1].anchoredPosition = new Vector2(300, -10 + h * (showH.Count - 1));

        history[hIndex].GetComponent<ScoreHistroy>().Move();

        //if (hIndex >= h_Text.Length)
        //{
        //    hIndex = 0;
        //}
        //h_Text[hIndex].gameObject.SetActive(true);
        //showH.Add(h_Text[hIndex].GetComponentInParent<RectTransform>());
        //showH[showH.Count - 1].anchoredPosition = new Vector2(300, -10 + h * (showH.Count - 1));

        //h_Text[hIndex].GetComponentInParent<ScoreHistroy>().Move();

        return h_Text[hIndex];
    }
    public void SetSumSocre(int s)
    {
        sumScore.text = s.ToString();
    }

    public void StageClear(int s)
    {
        FindHistory().text = "스테이지 클리어 +" + s.ToString();
    }
    public void GetItem(int s)
    {
        FindHistory().text = "아이템 획득 +" + s.ToString();
    }
    public void UseItem(int s)
    {
        FindHistory().text = "아이템 사용 +" + s.ToString();
    }
    public void KillMob(int s)
    {
        FindHistory().text = "장애물 파괴 +" + s.ToString();
    }
    public void ActionCount(int s)
    {
        FindHistory().text = "잔여 특수 행동 횟수 +" + s.ToString();
    }

    public void TurnScore(int s)
    {
        FindHistory().text = "클리어 턴 미션 성공 +" + s.ToString();
    }

    //히스토리 감추기
    public void HideHistory()
    {
        showH.Remove(showH[0]);

        for(int i = 0; i < showH.Count; ++i)
        {
            showH[i].anchoredPosition = new Vector2(0, -10 + h * i);
        }
    }
}
