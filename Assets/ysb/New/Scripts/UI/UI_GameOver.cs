using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_GameOver : MonoBehaviour
{
    public GameObject back;
    public GameObject panel;

    public TMP_Text text_Score;

    private void Awake()
    {
        back.SetActive(false);
        panel.SetActive(false);
    }
    public void ShowResult()
    {
        back.SetActive(true);
        panel.SetActive(true);

        text_Score.text = ScoreManager.instance.TotalScore.ToString();
    }

    public void HideResult()
    {
        text_Score.text = "";

        back.SetActive(false);
        panel.SetActive(false);

        //æ¿ ¿Ãµø
        StageManager.instance.ResetData();
    }
}
