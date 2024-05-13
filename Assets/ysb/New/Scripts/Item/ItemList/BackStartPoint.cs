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
        UseItem();
    }
    public override void UseItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 2;
        manager_Item.SetPlayerPos(this);
    }
}
