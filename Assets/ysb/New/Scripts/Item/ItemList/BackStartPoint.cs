using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackStartPoint : Item
{
    private void Start()
    {
        range = 2;
    }
    public override void SelectItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }
        manager_Item.SetPlayerPos_UI(this, 0);
        //UseItem();
    }
    public override bool UseItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 2;
        
        return manager_Item.SetPlayerPos(this, 0);
    }
}
