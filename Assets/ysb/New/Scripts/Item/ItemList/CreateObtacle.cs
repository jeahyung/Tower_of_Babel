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
        if (manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 2;
        manager_Item.SeletItem_Eight(this, range);
    }
    public override bool UseItem()
    {
        manager_Item.CreateObject(itemPrefab);
        return true;
    }
}
