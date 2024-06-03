using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
    protected ItemManager manager_Item;
    public Sprite itemImg;
    public string ex;

    protected int range = 1;

    private void Awake()
    {
        manager_Item =FindObjectOfType<ItemManager>();
    }

    public virtual void SelectItem()
    {
        manager_Item.SeletItem_Four(this, range);
    }
    public virtual bool UseItem() { return true; }
}