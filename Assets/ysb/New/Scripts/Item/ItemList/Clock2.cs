using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock2 : BackStartPoint
{
    public override void SelectItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }
        manager_Item.SetPlayerPos_UI(this, 1);
        //UseItem();
    }
    public override bool UseItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 2;
        
        return manager_Item.SetPlayerPos(this, 1);
    }
}
