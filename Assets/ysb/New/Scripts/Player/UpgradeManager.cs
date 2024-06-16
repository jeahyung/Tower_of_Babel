using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : Singleton<UpgradeManager>
{
    //public static UpgradeManager instance = null;

    public UI_Setting ui_Setting;
    private static List<Upgrade> selectedUp = new List<Upgrade>();

    public static int saNum;               //선택한 행동 번호(0)
    public static int bonusCount = 0;      //특수행동 보너스(1)

    public static int energy = 0;

    public static int bonusTurn = 0;       //보너스 턴(2)
    public static int bonusItem = 0;       //보너스 아이템(3)

    public static bool changeItem = false;  //아이템 셔플 가능?
    public static bool noneEnergy = false;  //기본 이동시 에너지 소모 없음
      
    public static int bonusRange = 0;      //보너스 범위(4)
    public static int bonusScore_item = 0;      //보너스 스코어(5)-아이템 획득
    public static int bonusScore_noneItem = 0;  //보너스 스코어-아이템 미사용
    public static int bonusScore_turn = 0;      //보너스 스코어-제한 턴 수
    public static int bonusScore_killmob = 0;   //보너스 스코어-몬스터 파괴

    public static int SumScore = 0;        //스코어(총합)

    private int turn_bonus;

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
        turn_bonus = bonusTurn;
        if(selectedUp.Count > 0) { ui_Setting.AddUpgradeAll(selectedUp); }

        //ScoreManager.instance.SetSumSocre(SumScore);
    }

    public void ResetUpgrade()
    {
        selectedUp.Clear();
        saNum = 0;               //선택한 행동 번호(0)    
        bonusCount = 0;      //특수행동 보너스(1)    
        energy = 0;
    
        bonusTurn = 0;       //보너스 턴(2)    
        bonusItem = 0;       //보너스 아이템(3)
    
        changeItem = false;  //아이템 셔플 가능?
        noneEnergy = false;
    
        bonusRange = 0;      //보너스 범위(4)    
        bonusScore_item = 0;      //보너스 스코어(5)-아이템 획득    
        bonusScore_noneItem = 0;  //보너스 스코어-아이템 미사용    
        bonusScore_turn = 0;      //보너스 스코어-제한 턴 수   
        bonusScore_killmob = 0;   //보너스 스코어-몬스터 파괴
}

    //게임 시작 시 세팅돼야 하는 것들
    public void StartGame()
    {
        turn_bonus = bonusTurn;
        UpgradeDatabase.instance.SetData();
    }

    //이 업그레이드 추가 가능?
    public bool CheckUpgrade(Upgrade up)
    {
        if (selectedUp.Contains(up) == true)
        {
            return false;
        }
        return true;
    }

    public void AddUpgrade(Upgrade up)
    {
        if(up.id < 10 || up.id >= 20)
        {
            if(selectedUp.Contains(up) == true)
            {
                return;
            }
        }
        //업그레이드 적용
        ApplyUpgrade(up);
    }
    public void SetAction(int num)
    {
        saNum = num;
    }
    private void ApplyUpgrade(Upgrade up)
    {
        switch(up.upType)
        {
            case 0: //Turn
                bonusTurn += up.state;
                break;
            case 1: //GetItem
                bonusItem += up.state;
                break;
                
            case 2: //ChangeItem    ?
                changeItem = true;
                break;

            case 3: //Energy
                FindObjectOfType<EnergySystem>().SetEnergy(up.state);
                //energy += up.state;
                break;

            case 6: //add Action Count
                bonusCount += up.state;
                break;
            case 7: //change Action
                UpgradeDatabase.instance.SetActionData();
                break;


            case 10: //score item
                bonusScore_item += up.state;
                break;
            case 11: //score noneitem
                bonusScore_noneItem += up.state;
                break;
            case 12: //score turn
                bonusScore_turn += up.state;
                break;
            case 13: //scorekill mob
                bonusScore_killmob += up.state;
                break;

            case 20:
                noneEnergy = true;
                break;
        }

        if(up.upType == 5 || up.upType == 7) { return; }
        selectedUp.Add(up);
        ui_Setting.AddUpgrade(up);
    }

    #region bonus
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
        turn_bonus += i;
        return turn_bonus;
    }
    
    public int getBonusItem(int i = 0)
    {
        bonusItem -= i;
        return bonusItem;
    }

    public bool getItemChange() //아이템 셔플 가능?
    {
        return changeItem;
    }
    
    public int getBonusRange()
    {
        return bonusRange;
    }
    
    public int getSumScore(int i = 0)
    {
        SumScore += i;
        return SumScore;
    }

    public int getEnergy(int i = 0)
    {
        energy = i;
        return energy;
    }
    #endregion

    #region Score
    public int GetScore_Item()
    {
        return bonusScore_item;
    }public int GetScore_ItemNone()
    {
        return bonusScore_noneItem;
    }public int GetScore_Turn()
    {
        return bonusScore_turn;
    }public int GetScore_KillMob()
    {
        return bonusScore_killmob;
    }
    #endregion

    public bool getNoneEnergy()
    {
        return noneEnergy;
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
