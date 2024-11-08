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
        //manager_Item.CreateObject(itemPrefab);\
        return CheckMob();
    }

    public virtual bool CheckMob()
    {
<<<<<<< HEAD
        Mob mob = manager_Item.FindMob();
        if (mob != null)
        {
            mob.DontMove();
            //ScoreManager.instance.KillMob();    //스코어
            manager_Item.NextTurn();
            
            return true;
        }
=======
        //Mob mob = manager_Item.FindMob();
        //if (mob != null)
        //{
        //    mob.DontMove();
        //    //ScoreManager.instance.KillMob();    //스코어
        //    manager_Item.NextTurn();
        //    return true;
        //}
>>>>>>> main
        
        //return false;

        manager_Item.UseRope1();
        manager_Item.NextTurn();
        return true;
    }
}
