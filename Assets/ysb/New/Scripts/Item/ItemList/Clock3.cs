using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock3 : BackStartPoint
{
    public override void SelectItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }
        manager_Item.SetPlayerPos_UI2(this);
        //UseItem();
    }
    public override bool UseItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 2;
        
        return manager_Item.SetPlayerPos2(this);
    }
}
