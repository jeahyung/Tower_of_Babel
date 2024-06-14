using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_GameOver : MonoBehaviour
{
    public GameObject back;
    public GameObject panel;


    List<string> textContent = new List<string>();
    List<int> score = new List<int>();
    private List<ClearScore> sText = new List<ClearScore>();
    public TMP_Text total;

    private void Awake()
    {
        sText.AddRange(GetComponentsInChildren<ClearScore>());
        foreach (var t in sText)
        {
            t.gameObject.SetActive(false);
        }
        back.SetActive(false);
        panel.SetActive(false);
    }
    private void SetText()
    {
        textContent.Clear();
        textContent.Add("스테이지 클리어  :  " + score[0].ToString());
        textContent.Add("남은 특수이동 횟수  :  " + score[1].ToString());
        textContent.Add("아이템 획득  :  " + score[2].ToString());
        textContent.Add("아이템 사용  :  " + score[3].ToString());
        textContent.Add("몬스터 파괴  :  " + score[4].ToString());
    }

    public void ShowResult()
    {
        back.SetActive(true);

        score.Clear();
        score.AddRange(ScoreManager.instance.CalculateScore_GameOver());
        SetText();  //문구 설정
        for(int i = 0; i < textContent.Count; ++i)
        {
            sText[i].SetText(textContent[i]);
        }

        total.text = "ToTal Score  :  " + score[score.Count - 1].ToString();
        panel.SetActive(true);
        ShowScore();
    }
    public void ShowScore()
    {
        total.enabled = true;
        total.text = score[score.Count - 2].ToString();

        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        int sIndex = 0;
        int height = -50;
        //AudioManager.instance.PlaySfx(AudioManager.Sfx.Game_Over);
        while (sIndex < 5)
        {
            float h = height * (sIndex - 2);//(sCount - sIndex - 2);
            sText[sIndex].gameObject.SetActive(true);
            sText[sIndex].SetText(textContent[sIndex]);
            sText[sIndex].SetTarget(h);
            sIndex++;
            yield return new WaitForSeconds(0.5f);

        }
        yield return new WaitForSeconds(0.6f);
        
        StartCoroutine(Count(score[score.Count - 1], score[score.Count - 2]));
    }

    IEnumerator Count(float target, float current)
    {
        float duration = 0.5f; // 카운팅에 걸리는 시간 설정. 
        float offset = (target - current) / duration;

        while (current < target)
        {
            current += offset * Time.deltaTime;
            total.text = "ToTal Score  :  " + ((int)current).ToString();
            yield return null;
        }

        current = target;
        total.text = "ToTal Score  :  " + ((int)current).ToString();
    }



    public void HideResult()
    {
        total.text = "";

        back.SetActive(false);
        panel.SetActive(false);

        //씬 이동
        StageManager.instance.ResetData();
    }
}
