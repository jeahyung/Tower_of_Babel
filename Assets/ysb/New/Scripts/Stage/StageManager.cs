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
    //public string stageName;    //첫번째 스테이지 이름

    [SerializeField] private List<GameObject> stages = new List<GameObject>(); //1챕터 스테이지들
    private GameObject curStage = null;

    private void Start()
    {
        //스테이지 추가
        for(int i = 0; i < transform.childCount; ++i)
        {
            stages.Add(transform.GetChild(i).gameObject);
        }
    }
    void OnEnable()
    {
        manager_turn = FindObjectOfType<TurnManager>();
        if(ui_turn == null)
        {
            ui_turn = FindObjectOfType<UI_Turn>();
        }
        ui_turn.SetStageInfo(chapterCount, stageCount);
    }
    //스테이지를 세팅합니다.
    public void SettingStage()
    {
        SelectStage();
        ui_turn.SetStageInfo(chapterCount, stageCount);

        //로딩 종료
    }

    //스테이지를 선택합니다.
    private void SelectStage()
    {
        //현재 스테이지 비활성화
        if(curStage != null) { curStage.SetActive(false); }

        //새 스테이지
        int si = Random.Range(0, stages.Count);
        curStage = stages[si];
        curStage.SetActive(true);

        stages.Remove(stages[si]);
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
        if(chapterCount == 1 && stageCount == 1) { return true; }
        //if (SceneManager.GetActiveScene().name == stageName)
        //{
        //    return true;
        //}
        return false;
    }

    public void NextStage()
    {
        //로딩 시작
        stageCount++;
        //SceneManager.LoadScene("Prototype"); //체크를 위해
        //SceneManager.LoadScene("Prototype " + (stageCount - 1).ToString());
    }
}
