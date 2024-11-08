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
    [SerializeField] private UI_Suicide ui_go_s;
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
    private Player_Move playerMover;
    private EnergySystem energy;

    public RectTransform uiElement;
    public string[] sName;
    public bool isBonusStage { get; set; }

    private void Start()
    {
        ////�������� �߰�
        //for(int i = 0; i < transform.childCount; ++i)
        //{
        //    stages.Add(transform.GetChild(i).gameObject);
        //    stages[i].SetActive(false);
        //}
        //StartCoroutine(Loading());
        ////ù ��° �������� Ȱ��ȭ
        //SelectStage();
        playerMover = FindObjectOfType<Player_Move>();
        energy = playerMover.GetComponent<EnergySystem>();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        manager_turn = FindObjectOfType<TurnManager>();
        if (ui_turn == null)
        {
            ui_turn = FindObjectOfType<UI_Turn>();
        }
        if (ui_gameover == null) { ui_gameover = FindObjectOfType<UI_GameOver>(); }
        if(ui_go_s == null) { ui_go_s = FindObjectOfType<UI_Suicide>(); }
        if(map == null) { map = FindObjectOfType<Map>(); }

        StartCoroutine(Loading());

        ResetStage();   //�������� ������ ����
        SettingStage();
        //ui_turn.SetStageInfo(chapterCount, stageCount);
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
        manager_turn.KingReset();

        //�� ��������
        isBonusStage = false;
        int si = Random.Range(0, stages.Count);
        curStage = stages[si];
        stages.Remove(stages[si]);

        if (chapterCount != 1 && si >= stages.Count - 2)
        {
            isBonusStage = true;
        }
        curStage.SetActive(true);
        mob = curStage.GetComponentInChildren<MobManager>();
    }



    public int GetStageCount => stageCount;
    public int GetChapterCount => chapterCount;

    public void PlayerMoving(bool b)
    {
        manager_turn.GetComponent<PlayerMovement>().SetControl(b);
    }
    public void StartGame()
    {
        UpgradeDatabase.instance.SetData();

        isPlaying = true;
        if(mob != null) { mob.StartGame(); }
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
        if(SceneManager.GetActiveScene().buildIndex == 1) { return true; }
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
    void NextChapter()
    {
        if(chapterCount > 3)
        {
            //GameOver_suicide(); //�ӽ�
            //return;
        }
        SceneManager.LoadScene(chapterCount);
    }

    public void NextStage()
    {
        if(stageCount % index_Upgrade == 0)
        {
            chapterCount += 1;
            NextChapter();
            return;
        }
        manager_turn.GetComponent<PlayerMovement>().SetControl(false);
        Img_loading.SetActive(true);
        stageCount++;
        SettingStage();

        map.ResetTile();

        //�ε� ����
        StartCoroutine(Loading());
    }
    IEnumerator Loading()
    {
        UI_Loading_Slider slider = Img_loading.GetComponent<UI_Loading_Slider>();

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
        manager_turn.GetComponent<PlayerMovement>().SetControl(true);
        if(uiElement == null)
        {
            playerMover.CheckAndMoving();
        }
        else
        {
            if (uiElement.localScale.x == 0)
            {
                playerMover.CheckAndMoving();
            }
        }
    }

    //���� ���� ���� - �ڻ��
    public void GameOver_suicide()
    {
        if(isGameOver == true) { return; }
        isGameOver = true;
        ScoreManager.instance.GetEnergyScore(chapterCount, energy.GetEnergy());
        //PlayerMovement player = manager_turn.gameObject.GetComponent<PlayerMovement>();

        manager_turn.GameOver();
        manager_turn.GetComponent<PlayerMovement>().SetControl(false);
        map.GetComponent<DestoryTile>().DropTile(); //Ÿ�� ������
        for(int i = 0; i<4; i++)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Game_Over_Broken);
        }
    }

    public void GameOver()
    {
        if (isGameOver == true) { return; }
        isGameOver = true;
        //ScoreManager.instance.GetEnergyScore(chapterCount, energy.GetEnergy());

        manager_turn.GameOver();
        manager_turn.GetComponent<PlayerMovement>().SetControl(false);

        ui_gameover.ShowResult();

        //�ڻ� ����
        //map.GetComponent<DestoryTile>().DropTile(); //Ÿ�� ������
        for (int i = 0; i < 4; i++)
        {
            //AudioManager.instance.PlaySfx(AudioManager.Sfx.Game_Over_Broken);
        }
    }

    public void ShowResult()
    {
        //ui_gameover.ShowResult();
        ui_go_s.ShowResult();
    }

    public void ResetStage()
    {
        stageCount = 1;

        stages.Clear();
        for (int i = 0; i < transform.childCount; ++i)
        {
            stages.Add(transform.GetChild(i).gameObject);
            stages[i].SetActive(false);
        }
    }
    public void ResetData()
    {
        chapterCount = 1;
        stageCount = 1;

        energy.ResetEnergy();

        isGameOver = false;
        //������ ����
        ItemInventory.instance.ResetItem();
        UpgradeDatabase.instance.ResetUpgradeData();
        UpgradeManager.instance.ResetUpgrade();//��ȭ ����
        ScoreManager.instance.ResetScore();//���ھ� ����

        //SceneManager.LoadScene(1);
    }

    public void ReStart()
    {
        Img_loading.SetActive(true);
        SceneManager.LoadScene(1);  //1����������
    }

    public void BackTile()
    {
        Img_loading.SetActive(true);
        ResetData();
        SceneManager.LoadScene(0);
    }
}
