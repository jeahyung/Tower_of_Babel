using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Item
{
    private void Start()
    {
        range = 10;
    }
    public override void SelectItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 10;
        manager_Item.SelectItem_Rope(this);
    }
    public override bool UseItem()
    {
        //manager_Item.CreateObject(itemPrefab);
        return CheckMob();
    }

    public bool CheckMob()
    {
        Mob mob = manager_Item.FindMob();
        if (mob != null)
        {
            mob.DontMove();
            //ScoreManager.instance.KillMob();    //½ºÄÚ¾î
            manager_Item.NextTurn();
            
            return true;
        }
        
        return false;
    }
}
