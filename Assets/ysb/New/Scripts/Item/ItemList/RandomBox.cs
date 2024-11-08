using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : Item
{
    private void Start()
    {
        range = 10;
        canAdd = true;
    }
    public override void SelectItem()
    {
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }
        manager_Item.SelectItem_Box(this);
        //UseItem();
    }
    public override bool UseItem()
    {
        ScoreManager.instance.AddBoxScore();
        manager_Item.NextTurn();
        return true;
    }
}
