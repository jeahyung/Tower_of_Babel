using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
    protected ItemManager manager_Item;
    public Sprite itemImg;
    public string ex;

    public int id;
    protected int range = 1;

    public bool canAdd = false; //융합 가능?

    private void Awake()
    {
        manager_Item = FindObjectOfType<ItemManager>();
    }

    public virtual void SelectItem()
    {
        manager_Item.SeletItem_Four(this, range);
    }
    public virtual bool UseItem() { return true; }
}