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
    //public string stageName;    //ù��° �������� �̸�

    [SerializeField] private List<GameObject> stages = new List<GameObject>(); //1é�� ����������
    private GameObject curStage = null;

    private void Start()
    {
        //�������� �߰�
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
    //���������� �����մϴ�.
    public void SettingStage()
    {
        SelectStage();
        ui_turn.SetStageInfo(chapterCount, stageCount);

        //�ε� ����
    }

    //���������� �����մϴ�.
    private void SelectStage()
    {
        //���� �������� ��Ȱ��ȭ
        if(curStage != null) { curStage.SetActive(false); }

        //�� ��������
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
        //�ε� ����
        stageCount++;
        //SceneManager.LoadScene("Prototype"); //üũ�� ����
        //SceneManager.LoadScene("Prototype " + (stageCount - 1).ToString());
    }
}
