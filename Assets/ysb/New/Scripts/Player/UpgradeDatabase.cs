using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpType { self = 0, score = 1, turn = 2, item = 3, action = 5, actionCount = 6, actionSelect = 7, }
public class Upgrade
{
    public int id;

    public string name;
    public int state;
    public int upType;
    public string explain;

    //public UpType upType;
    public Upgrade(int ID, string n, int i, int t, string e)
    {
        id = ID;

        name = n;
        state = i;
        upType = t;
        explain = e;
        //upType = UpType.self;
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

    public bool isFirst = false; //처음 선택?
    private void Start()
    {
        upController = FindObjectOfType<UpgradeController>();
        count = 10;
        LoadCSVFile();

        if (StageManager.instance.CheckStage() == true)
            SetActionData();
        //SetData();
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
            
            upList.Add(up);
        }
    }

    public void SetActionData()
    {

        actionsList.Clear();
        Upgrade up1 = new Upgrade(991, "룩", 0, 5, "가로 & 세로");
        Upgrade up2 = new Upgrade(992, "비숍", 1, 5, "대각선");
        Upgrade up3 = new Upgrade(993, "킹", 2, 5, "8방향 & 보너스 행동 1회");

        actionsList.Add(up1);
        actionsList.Add(up2);
        actionsList.Add(up3);

        upController.SetActionUpgrade(actionsList);
        upController.SetSelectList();   //업그레이드 보여주기
    }

    //업그레이드 데이터 세팅
    public void SetData()
    {
        upController.SetUpgrade(upList);
    }
}
