using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    private TurnManager manager_turn;
    [SerializeField] private UI_Turn ui_turn;
    public bool isPlaying = false;

    public static int chapterCount = 1;
    public static int stageCount = 1;
    public string stageName;    //첫번째 스테이지 이름
    void OnEnable()
    {
        manager_turn = FindObjectOfType<TurnManager>();
        if(ui_turn == null)
        {
            ui_turn = FindObjectOfType<UI_Turn>();
        }
        ui_turn.SetStageInfo(chapterCount, stageCount);
    }

    public int GetStageCount => stageCount;
    public int GetChapterCount => chapterCount;


    public void StartGame()
    {
        UpgradeDatabase.instance.SetData();

        isPlaying = true;
        manager_turn.StartGame();
    }

    public void EndGame()
    {
        isPlaying = false;
        manager_turn.EndGame();
    }

    public bool CheckStage()
    {
        if (SceneManager.GetActiveScene().name == stageName)
        {
            return true;
        }
        return false;
    }

    public void NextStage()
    {
        stageCount++;
        SceneManager.LoadScene("Prototype " + (stageCount - 1).ToString());
    }
}
