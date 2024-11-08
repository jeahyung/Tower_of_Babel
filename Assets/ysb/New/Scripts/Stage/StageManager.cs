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
    private Player_Move playerMover;
    private EnergySystem energy;

    public RectTransform uiElement;
    public string[] sName;

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
        //int si = Random.Range(0, stages.Count);    
        int si = 3;
        curStage = stages[si];
        curStage.SetActive(true);
        mob = curStage.GetComponentInChildren<MobManager>();

        stages.Remove(stages[si]);
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
            GameOver(); //�ӽ�
            return;
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

    public void GameOver()
    {
        if(isGameOver == true) { return; }
        isGameOver = true;
        manager_turn.GameOver();
        manager_turn.GetComponent<PlayerMovement>().SetControl(false);
        map.GetComponent<DestoryTile>().DropTile(); //Ÿ�� ������
        for(int i = 0; i<4; i++)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Game_Over_Broken);
        }
    }

    public void ShowResult()
    {
        ui_gameover.ShowResult();
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
