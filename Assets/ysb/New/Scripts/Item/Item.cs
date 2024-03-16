using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
    protected ItemManager manager_Item;
    public Sprite itemImg;

    protected int range = 1;

    private void Awake()
    {
        manager_Item = GameObject.FindWithTag("Player").GetComponent<ItemManager>();
    }

    public virtual void SelectItem()
    {
        manager_Item.SeletItem_Four(this, range);
    }
    public virtual void UseItem() { }
}