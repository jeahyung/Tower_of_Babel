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
        manager_Item.SetPlayerPos();
    }
}
