using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpType { 
    turn = 0,   //ok
    getItem = 1,//ok
    changeItem = 2,//ok
    energy = 3, //ok

    selectAction = 5,   //ok
    countAction = 6,    //ok
    changeAction = 7,   //ok

    itemScore = 10, //ok
    noneItemScore = 11,
    turnScore = 12, //ok
    breakScore = 13,    //ok

    noneEnergy = 20,
    }
public class Upgrade
{
    public int id;

    public string name;
    public int state;
    public int upType;
    public string explain;
    public Upgrade(int ID, string n, int i, int t, string e)
    {
        id = ID;

        name = n;
        state = i;
        upType = t;
        explain = e;
    }
}

public class UpgradeDatabase : Singleton<UpgradeDatabase>
{
    private UpgradeController upController;
    public int count;
    public List<Upgrade> actionsList = new List<Upgrade>();
    public List<Upgrade> upList = new List<Upgrade>();

    //데이터
    public string fileName = "Upgrade";
    List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    private void Start()
    {
        upController = FindObjectOfType<UpgradeController>();
        count = 10;
        LoadCSVFile();

        if (StageManager.instance.CheckStage() == true)
        {
            SetActionData();
        }
        else//테스트용
        {
            SetData();
            //upController.OpenUpgradePanel();
        }
    }
    public void ResetUpgradeData()
    {
        upList.Clear();
        LoadCSVFile();
    }
    public void LoadCSVFile()
    {
        dicList.Clear();
        dicList = CSVReader.Read(fileName);

        for(int i = 0; i < dicList.Count; ++i)
        {
            int id = int.Parse(dicList[i]["Id"].ToString());
            string n = dicList[i]["Name"].ToString();
            int s = int.Parse(dicList[i]["State"].ToString());
            int t = int.Parse(dicList[i]["Type"].ToString());
            string e = dicList[i]["Explain"].ToString();

            Upgrade up = new Upgrade(id, n, s, t, e);

            //이 업그레이드 1번만 등장하는 업그레이든가? 이미 가지고 있는가?
            //bool canAdd = UpgradeManager.instance.CheckUpgrade(up);
            //if(canAdd == false) { continue; }

            upList.Add(up);
        }
        //SetData();
    }

    //액션 선택
    public void SetActionData()
    {
        upController.OpenActionPanel();
        StageManager.instance.PlayerMoving(false);
        //actionsList.Clear();
        //Upgrade up1 = new Upgrade(991, "룩", 0, 5, "가로 & 세로");
        //Upgrade up2 = new Upgrade(992, "비숍", 1, 5, "대각선");
        //Upgrade up3 = new Upgrade(993, "킹", 2, 5, "8방향 & 보너스 행동 1회");

        //actionsList.Add(up1);
        //actionsList.Add(up2);
        //actionsList.Add(up3);

        //upController.SetActionUpgrade(actionsList);
        //upController.SetSelectList();   //업그레이드 보여주기
    }

    //업그레이드 데이터 세팅
    public void SetData()
    {
        Debug.Log(upList.Count);
        upController.SetUpgrade(upList);
    }

    //일부 증강체는 선택 시 삭제 - 영구 능력들
    public void RemoveData(Upgrade up)
    {
        //if(up.upType < 10 || up.upType >= 20) { return; }
        for(int i = 0; i < upList.Count; ++i)
        {
            if (up.id == upList[i].id)
            {
                upList.Remove(upList[i]);
                break;

            }
        }
        Debug.Log("delete : " + up.name);

        //if (upList.Contains(up) && up.upType < 10 || up.upType >= 20)
        //{
        //    upList.Remove(up);
        //    Debug.Log("delete : " + up.name);
        //}
    }

    public void OpenUpgrade()
    {
        SetData();
        upController.OpenUpgradePanel();       
    }
}
