using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    private List<Upgrade> selectedUp = new List<Upgrade>();

    public int bonusRange = 0;
    public int bonusScore = 0;

    public int num; //선택한 행동 번호
    public void AddUpgrade(Upgrade up)
    {
        selectedUp.Add(up);
        ApplyUpgrade(up);
    }

    private void ApplyUpgrade(Upgrade up)
    {
        if(up.upType == UpType.self)
        {
            bonusRange += up.state;
        }
        else if(up.upType == UpType.score)
        {
            bonusScore += up.state;
        }
        else if(up.upType == UpType.action)
        {
            num = up.state;
        }
    }
}
