using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpKey : Key
{
    public override void SelectItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 10;
        manager_Item.SelectItem_Key2(this);
    }

    public override bool CheckMob()
    {
        ScoreManager.instance.KillMob();    //½ºÄÚ¾î
        manager_Item.NextTurn();
        return true;
    }
}
