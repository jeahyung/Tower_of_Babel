using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public TMP_Text sumScore;

    public int hIndex = -1;
    public TMP_Text[] h_Text;   //���� ����

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
        //���� ������ ã�´� + ������ ������ �� ���� �ø���.
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
        FindHistory().text = "�������� Ŭ���� +" + s.ToString();
    }
    public void GetItem(int s)
    {
        FindHistory().text = "������ ȹ�� +" + s.ToString();
    }
    public void UseItem(int s)
    {
        FindHistory().text = "������ ��� +" + s.ToString();
    }
    public void KillMob(int s)
    {
        FindHistory().text = "��ֹ� �ı� +" + s.ToString();
    }
    public void ActionCount(int s)
    {
        FindHistory().text = "�ܿ� Ư�� �ൿ Ƚ�� +" + s.ToString();
    }

    public void TurnScore(int s)
    {
        FindHistory().text = "Ŭ���� �� �̼� ���� +" + s.ToString();
    }

    //�����丮 ���߱�
    public void HideHistory()
    {
        showH.Remove(showH[0]);

        for(int i = 0; i < showH.Count; ++i)
        {
            showH[i].anchoredPosition = new Vector2(0, -10 + h * i);
        }
    }
}
