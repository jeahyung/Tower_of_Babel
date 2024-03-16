using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObtacle : Item
{
    public GameObject itemPrefab;

    private void Start()
    {
        range = 2;
    }
    public override void SelectItem()
    {
        manager_Item.SeletItem_Eight(this, range);
    }
    public override void UseItem()
    {
        manager_Item.CreateObject(itemPrefab);
    }
}
