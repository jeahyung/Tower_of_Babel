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
    }
}
