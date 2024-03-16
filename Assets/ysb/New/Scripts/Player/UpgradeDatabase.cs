using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpType { self = 0, score = 1, action = 5, }
public class Upgrade
{
    public int state;
    public UpType upType;
    public Upgrade(int i)
    {
        state = i;
        upType = UpType.self;
    }
}

public class UpgradeDatabase : Singleton<UpgradeDatabase>
{
    private UpgradeController upController;
    public int count;
    public List<Upgrade> actionsList = new List<Upgrade>();
    public List<Upgrade> upList = new List<Upgrade>();

    public bool isFirst = false; //처음 선택?
    private void Start()
    {
        upController = FindObjectOfType<UpgradeController>();
        count = 10;

        //타입에 따라 따로 저장(data : 타입 / state / 설명 / 이미지?
        //특수 액션
        for(int i = 0; i < 3; ++i)
        {
            Upgrade up = new Upgrade(i);
            up.upType = UpType.action;

            actionsList.Add(up);
        }

        //일반
        for(int i = 0; i < count; ++i)
        {
            Upgrade up = new Upgrade(i + 1);
            upList.Add(up);
        }

        SetData();
    }

    //업그레이드 데이터 세팅
    public void SetData()
    {
        if (isFirst == true)
        {
            upController.SetActionUpgrade(actionsList);
            isFirst = false;
            return;
        }
        upController.SetUpgrade(upList);
    }
}
