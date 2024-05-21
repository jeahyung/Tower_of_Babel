using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : Singleton<UpgradeManager>
{
    //public static UpgradeManager instance = null;

    private static List<Upgrade> selectedUp = new List<Upgrade>();

    public static int saNum;               //선택한 행동 번호(0)
    public static int bonusCount = 0;      //특수행동 보너스(1)

    public static int bonusTurn = 0;       //보너스 턴(2)
    public static int bonusItem = 0;       //보너스 아이템(3)
      
    public static int bonusRange = 0;      //보너스 범위(4)
    public static int bonusScore = 0;      //보너스 스코어(5)

    public static int SumScore = 0;    //스코어(총합)

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
        //ScoreManager.instance.SetSumSocre(SumScore);
    }

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    void Start()
    {
        //SetData();
    }

    public void AddUpgrade(Upgrade up)
    {
        selectedUp.Add(up);
        ApplyUpgrade(up);
    }

    private void ApplyUpgrade(Upgrade up)
    {
        switch(up.upType)
        {
            case 0://UpType.self
                bonusRange += up.state;
                //LoadGameData.instance.ReplaceUpData(4, bonusRange);
                break;
            case 1://UpType.score
                bonusScore += up.state;
                //LoadGameData.instance.ReplaceUpData(5, bonusScore);
                break;
            case 2:
                bonusTurn += up.state;
                //LoadGameData.instance.ReplaceUpData(2, bonusTurn);
                break;
            case 3: //아이템
                bonusItem++;
                //LoadGameData.instance.ReplaceUpData(3, bonusItem);
                //ItemInventory.instance.AddBonusItem();
                break;
            case 5://UpType.action
                saNum = up.state;
                //LoadGameData.instance.ReplaceUpData(0, saNum);
                break;
            case 6://UpType.actionCount
                bonusCount += up.state;
                //LoadGameData.instance.ReplaceUpData(1, bonusCount);
                break;
            case 7://행동 재설정
                UpgradeDatabase.instance.SetActionData();
                break;
        }

        if(up.upType == 7 || up.upType == 3 || up.upType == 2) { return; }
        selectedUp.Add(up);
    }


    public int GetSANum()
    {
        return saNum;
    }
    public int getBonusCount()
    {
        return bonusCount;
    }
    
    public int getBonusTurn(int i = 0)
    {
        bonusTurn += i;
        return bonusTurn;
    }
    
    public int getBonusItem(int i = 0)
    {
        bonusItem -= i;
        return bonusItem;
    }
    
    public int getBonusRange()
    {
        return bonusRange;
    }
    
    public int getBonusScore(int i = 0)
    {
        bonusScore += i;
        return bonusScore;
    }
    
    public int getSumScore(int i = 0)
    {
        SumScore += i;
        return SumScore;
    }


    //public void SetData()
    //{
    //    saNum = LoadGameData.instance.SearchUpData("SA"); 
    //    bonusCount = LoadGameData.instance.SearchUpData("SACount");

    //    bonusTurn = LoadGameData.instance.SearchUpData("Turn");
    //    bonusItem = LoadGameData.instance.SearchUpData("Item");

    //    bonusRange = LoadGameData.instance.SearchUpData("Range");
    //    bonusScore = LoadGameData.instance.SearchUpData("Score");
    //}
}
