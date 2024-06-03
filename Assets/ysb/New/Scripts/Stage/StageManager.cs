using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    private TurnManager manager_turn;
    [SerializeField] private Map map;
    [SerializeField] private UI_Turn ui_turn;
    [SerializeField] private UI_GameOver ui_gameover;
    public bool isPlaying = false;

    public static int chapterCount = 1;
    public static int stageCount = 1;
    public int index_Upgrade = 5;   //���׷��̵� �����ϴ� ��

    [SerializeField] private List<GameObject> stages = new List<GameObject>(); //1é�� ����������
    private GameObject curStage = null;
    public GameObject spawnPoint;
    public MobManager mob;

    public GameObject Img_loading;
    public float waitTime = 1f;

    private bool isGameOver = false;

    private void Start()
    {
        //�������� �߰�
        for(int i = 0; i < transform.childCount; ++i)
        {
            stages.Add(transform.GetChild(i).gameObject);
            stages[i].SetActive(false);
        }
        //ù ��° �������� Ȱ��ȭ
        SelectStage();
    }
    void OnEnable()
    {
        manager_turn = FindObjectOfType<TurnManager>();
        if(ui_turn == null)
        {
            ui_turn = FindObjectOfType<UI_Turn>();
        }
        if(ui_gameover == null) { ui_gameover = FindObjectOfType<UI_GameOver>(); }
        ui_turn.SetStageInfo(chapterCount, stageCount);
    }
    //���������� �����մϴ�.
    public void SettingStage()
    {
        SelectStage();
        ui_turn.SetStageInfo(chapterCount, stageCount);

        //�ϸŴ����� ���� �������� �� �Ŵ��� �Ҵ�
        manager_turn.SetMobManager(curStage.GetComponentInChildren<MobManager>());
        spawnPoint.SetActive(false);
        spawnPoint.SetActive(true);
        //�ε� ����
    }

    //���������� �����մϴ�.
    private void SelectStage()
    {
        //���� �������� ��Ȱ��ȭ
        if(curStage != null) { curStage.SetActive(false); }

        //�� ��������
        int si = Random.Range(0, stages.Count);    
        //int si = 0;
        curStage = stages[si];
        curStage.SetActive(true);
        mob = curStage.GetComponentInChildren<MobManager>();

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

        //����ü �ִ� �������� Ŭ������?
        ShowUpgrade();
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

    public void ShowUpgrade()
    {
        //5�������� ����
        if(stageCount % index_Upgrade == 0)
        {
            UpgradeDatabase.instance.OpenUpgrade();
        }
    }

    public void NextStage()
    {
        //�ε� ����
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        Img_loading.SetActive(true);
        UI_Loading_Slider slider = Img_loading.GetComponent<UI_Loading_Slider>();
        stageCount++;
        SettingStage();

        map.ResetTile();

        float timer = 0f;
        while(timer < waitTime)
        {
            timer += Time.deltaTime;
            slider.SetSliderValue(timer);
            yield return null;
        }
        slider.SetSliderValue(1f);
        yield return new WaitForSecondsRealtime(0.2f);

        //�ε�â �ƿ�
        Img_loading.SetActive(false);
    }

    public void GameOver()
    {
        if(isGameOver == true) { return; }
        isGameOver = true;
        manager_turn.GetComponent<PlayerMovement>().SetControl(false);
        map.GetComponent<DestoryTile>().DropTile(); //Ÿ�� ������
    }

    public void ShowResult()
    {
        ui_gameover.ShowResult();
    }
    public void ResetData()
    {
        isGameOver = false;
        //������ ����
        UpgradeDatabase.instance.ResetUpgradeData();
        UpgradeManager.instance.ResetUpgrade();//��ȭ ����
        ScoreManager.instance.ResetScore();//���ھ� ����

        SceneManager.LoadScene(1);
    }
}
