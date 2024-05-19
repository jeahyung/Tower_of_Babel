using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : Singleton<UpgradeManager>
{
    //public static UpgradeManager instance = null;

    private static List<Upgrade> selectedUp = new List<Upgrade>();

    public static int saNum;               //������ �ൿ ��ȣ(0)
    public static int bonusCount = 0;      //Ư���ൿ ���ʽ�(1)

    public static int energy = 0;

    public static int bonusTurn = 0;       //���ʽ� ��(2)
    public static int bonusItem = 0;       //���ʽ� ������(3)
      
    public static int bonusRange = 0;      //���ʽ� ����(4)
    public static int bonusScore_item = 0;      //���ʽ� ���ھ�(5)-������ ȹ��
    public static int bonusScore_noneItem = 0;  //���ʽ� ���ھ�-������ �̻��
    public static int bonusScore_turn = 0;      //���ʽ� ���ھ�-���� �� ��
    public static int bonusScore_killmob = 0;   //���ʽ� ���ھ�-���� �ı�

    public static int SumScore = 0;        //���ھ�(����)

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
        //ScoreManager.instance.SetSumSocre(SumScore);
    }

    //�� ���׷��̵� �߰� ����?
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
        //���׷��̵� ����
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
                bonusItem += up.state;
                break;

            case 3: //Energy
                energy += up.state;
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
        }

        if(up.upType == 5 || up.upType == 7) { return; }
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
        turn_bonus += i;
        return turn_bonus;
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
