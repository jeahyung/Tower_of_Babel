using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : Singleton<ScoreManager>
{
    public static int scoreSum;
    public ScoreUI scoreUI;
    public UI_Result resultUI;
    public int[] turnScore = new int[4];

    //�� �� ��� ���
    int stageClearScore = 0;
    int clearTurnScore = 0; //Ŭ���� �Ͽ� ���� ���ھ�

    int actScore = 0;       //���� Ư���ൿ Ƚ��

    //�� �� ���� ���
    int itemScore = 0;      //������ ���ھ�
    int noneItemSocre = 0;  //������ ���ھ� - ��� ����

    int getSocre = 0;

    int killSocre = 0;      //���� �ı� ���ھ�

    int energyScore = 0;
    List<float> esPer;
    public int TotalScore => scoreSum;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scoreUI = FindObjectOfType<ScoreUI>();
        scoreUI.SetSumSocre(scoreSum);

        resultUI = FindObjectOfType<UI_Result>();

        for (int i = 0; i < turnScore.Length; ++i)
        {
            if (i == 3) { turnScore[i] = 3000; return; }
            turnScore[i] = 1000 + 500 * i;
        }

        stageClearScore = 0;
        clearTurnScore = 0; //Ŭ���� �Ͽ� ���� ���ھ�

        actScore = 0;       //���� Ư���ൿ Ƚ��

        //�� �� ���� ���
        itemScore = 0;      //������ ���ھ�
        noneItemSocre = 0;  //������ ���ھ� - ��� ����

        getSocre = 0;

        killSocre = 0;      //���� �ı� ���ھ�

        energyScore = 0;
    }

    public void ResetScore()
    {
        scoreSum = 0;
    }

    private void Start()
    {
        scoreUI = FindObjectOfType<ScoreUI>();
        scoreUI.SetSumSocre(scoreSum);

        for(int i = 0; i < turnScore.Length; ++i)
        {
            if(i == 3) { turnScore[i] = 3000; return; }
            turnScore[i] = 1000 + 500 * i;
        }
    }
    public void SetSumSocre(int score)
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();

        scoreSum = score;
        scoreUI.SetSumSocre(scoreSum);
    }


    public int SearchScore(string key)
    {
        int saNum = UpgradeManager.instance.GetSANum();
        int score = LoadGameData.instance.SearchScoreData(key, saNum);
        return score;
    }
    public void Score_ItemGet()
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();

        int bonus = UpgradeManager.instance.GetScore_Item();//UpgradeManager.instance.getBonusCount();//UpgradeManager.instance.bonusScore;
        int score = (SearchScore("Get") + bonus);
        scoreSum += score;
        //UpgradeManager.instance.SumScore = scoreSum;
        getSocre += score;  //result

        scoreUI.GetItem(score);
        scoreUI.SetSumSocre(scoreSum);
    }
    
    public void Score_ItemUse()
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();

        int bonus = UpgradeManager.instance.GetScore_Item();//.bonusScore;
        int score = (SearchScore("Use") + bonus);
        scoreSum += score;

        itemScore += score; //���â ��
        //UpgradeManager.instance.SumScore = scoreSum;
        scoreUI.UseItem(score);
        scoreUI.SetSumSocre(scoreSum);
    }
    public void KillMob()
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();

        int bonus = UpgradeManager.instance.GetScore_KillMob();//bonusScore;
        int score = (SearchScore("Mob") + bonus);
        scoreSum += score;
            
        killSocre += score; //���â ��

        //UpgradeManager.instance.SumScore = scoreSum;
        scoreUI.KillMob(score);
        scoreUI.SetSumSocre(scoreSum);
    }

    public void StageClear()
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();

        stageClearScore = 0;

        int bonus = 0;//UpgradeManager.instance.getBonusScore();//.bonusScore;
        int score = (SearchScore("Stage") + bonus);
        stageClearScore = score;
    }
    public void Score_SACount()//int rc)
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();
        actScore = 0;

        int rc = FindObjectOfType<SAManager>().getActCount;

        int bonus = 0;//UpgradeManager.instance.getBonusScore();//.bonusScore;
        int score = (SearchScore("Action") * rc + bonus);

        actScore = score;
    }

    public void Score_ClearTurn()//int turn)
    {
        clearTurnScore = 0; //�ʱ�ȭ
        int turn = FindObjectOfType<TurnManager>().TurnCount;

        int score = 0;
        int part = StageManager.instance.GetChapterCount;
        if (UpgradeManager.instance.GetSANum() == 2)//.saNum == 2)
        {
            score = turnScore[part];
            clearTurnScore = score;
            return;
        }
        if(part <= 1)
        {
            //������ ����
            turn -= UpgradeManager.instance.GetScore_Turn();
            if(turn < 0) { turn = 0; }

            score = turnScore[part] - 500 * turn;
            if (score < 0) { score = 0; return; }
        }
        else
        {
            //������ ����
            turn -= UpgradeManager.instance.GetScore_Turn();
            if (turn < 0) { turn = 0; }

            score = turnScore[part] - 1000 * turn;
            if (score < 0) { score = 0; return; }
        }
        clearTurnScore = score;
    }

    //������ �̻�� ����
    public void Score_NoneItem()
    {
        if(ItemInventory.instance.Count_ItemUse > 0) { 
            noneItemSocre = 0;
            return;
        }

        noneItemSocre = UpgradeManager.instance.GetScore_ItemNone();
    }

    public void CalculateScore()
    {
        StageClear();
        Score_ClearTurn();
        Score_SACount();
        Score_NoneItem();

        //int sc = 0;
        //if(stageClearScore != 0) { sc++; }
        //if(clearTurnScore != 0) { sc++; }
        //if(actScore != 0) { sc++; }
        //if(noneItemSocre != 0) { sc++; }

        List<int> scoreList = new List<int>();
        scoreList.Add(stageClearScore + clearTurnScore);
        //scoreList.Add(clearTurnScore);
        scoreList.Add(actScore);
        scoreList.Add(getSocre);
        scoreList.Add(itemScore + noneItemSocre);
        scoreList.Add(killSocre);

        int totalScore = scoreSum + stageClearScore + clearTurnScore + actScore + noneItemSocre;
        resultUI.ShowResult(scoreSum, totalScore, scoreList);

        //resultUI.ShowResult(sc, scoreSum, totalScore, scoreList);

        scoreSum = totalScore;
        scoreUI.SetSumSocre(scoreSum);

        //���ھ� �ʱ�ȭ
        stageClearScore = 0;
        clearTurnScore = 0; //Ŭ���� �Ͽ� ���� ���ھ�
        actScore = 0;       //���� Ư���ൿ Ƚ��
        //�� �� ���� ���
        itemScore = 0;      //������ ���ھ�
        noneItemSocre = 0;  //������ ���ھ� - ��� ����
        getSocre = 0;
        killSocre = 0;      //���� �ı� ���ھ�
    }
    
    public List<int> CalculateScore_GameOver()
    {
        if(StageManager.instance.isPlaying == true)
        {
            StageClear();
            Score_ClearTurn();
            Score_SACount();
        }
        else
        {
            stageClearScore = 0;
            clearTurnScore = 0;
            actScore = 0;
        }
        Score_NoneItem();

        List<int> scoreList = new List<int>();
        scoreList.Add(0);//stageClearScore + clearTurnScore);
        scoreList.Add(actScore);
        scoreList.Add(getSocre);
        scoreList.Add(itemScore + noneItemSocre);
        scoreList.Add(killSocre);

        int totalScore = scoreSum + actScore + noneItemSocre; //stageClearScore + clearTurnScore
        scoreList.Add(scoreSum);
        scoreList.Add(totalScore);
        scoreSum = totalScore;
        return scoreList;
    }


    //�ڻ�
    void SetESPer()
    {
        energyScore = 0;

        esPer = new List<float>();
        esPer.Clear();

        esPer.Add(10f);
        esPer.Add(20f);
        esPer.Add(30f);
    }
    public void GetEnergyScore(int stage = 1, int e = 0)
    {
        SetESPer();
        energyScore = (int)(e * scoreSum * 0.05f);//(int)(e * esPer[stage - 1]);
    }
    public List<int> CalculateScore_Suicide()
    {
        if (StageManager.instance.isPlaying == true)
        {
            StageClear();
            Score_ClearTurn();
            Score_SACount();
        }
        else
        {
            stageClearScore = 0;
            clearTurnScore = 0;
            actScore = 0;
        }
        Score_NoneItem();

        List<int> scoreList = new List<int>();
        scoreList.Add(energyScore);
        scoreList.Add(actScore);
        scoreList.Add(getSocre);
        scoreList.Add(itemScore + noneItemSocre);
        scoreList.Add(killSocre);

        int totalScore = scoreSum + energyScore + actScore + noneItemSocre;
        scoreList.Add(scoreSum);
        scoreList.Add(totalScore);
        scoreSum = totalScore;
        return scoreList;
    }


    public void AddBoxScore()
    {
        Debug.Log("box open");
        //�������� �ѹ� �����ͼ� �������� ����ϼ�
        int stageNumber = StageManager.instance.GetChapterCount;

        int score = 0;
        switch(stageNumber)
        {
            case 1:
                score = 3000;//Random.Range(50, 100);
                break;
            case 2:
                score = 8000;//Random.Range(100, 150);
                break;
            case 3:
                score = 13000;//Random.Range(150, 200);
                break;
            case 4:
                score = 20000;//Random.Range(200, 300);
                break;
            case 5:
                score = 30000;//Random.Range(300, 500);
                break;
            default:
                break;
        }
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUI>();

        //int bonus = UpgradeManager.instance.GetScore_Item();//.bonusScore;
        scoreSum += score;

        itemScore += score; //���â ��
        //UpgradeManager.instance.SumScore = scoreSum;
        scoreUI.UseItem(score);
        scoreUI.SetSumSocre(scoreSum);
    }

    public void DecreaseScore(int del)
    {
        scoreSum -= del;
    }
}
