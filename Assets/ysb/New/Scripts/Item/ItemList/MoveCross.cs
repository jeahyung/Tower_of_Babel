using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCross : Item
{
    private void Start()
    {
        range = 1;
    }
    public override void SelectItem()
    {
        manager_Item.SeletItem_Eight(this, range);
    }
    public override void UseItem()
    {
        manager_Item.MovePlayer();
    }
}
