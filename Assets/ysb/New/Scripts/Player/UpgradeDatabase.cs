using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpType { self = 0, score = 1,}
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
    public List<Upgrade> upList = new List<Upgrade>();
    private void Start()
    {
        upController = FindObjectOfType<UpgradeController>();
        count = 10;
        for(int i = 1; i < count; ++i)
        {
            Upgrade up = new Upgrade(i);
            upList.Add(up);
        }
        upController.SetUpgrade(upList);
    }
}
