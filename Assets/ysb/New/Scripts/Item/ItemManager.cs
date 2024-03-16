using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Map map;
    private ItemInventory inven;
    private Item selectedItem;

    private void Awake()
    {
        map = FindObjectOfType<Map>();
        inven = FindObjectOfType<ItemInventory>();
    }
    public void SeletItem_Four(Item item, int range)
    {
        if(map.canControl == false) { return; }
        selectedItem = item;
        map.SelectItem(item);
        map.UseItem_Four(range);
    }

    public void SeletItem_Eight(Item item, int range)
    {
        if (map.canControl == false) { return; }
        selectedItem = item;
        map.SelectItem(item);
        map.UseItem_Eight(range);
    }

    public void UseItem()
    {
        selectedItem.UseItem();
        inven.RemoveItem(selectedItem);
    }

    public void CreateObject(GameObject obj)
    {
        GameObject newObejct = Instantiate(obj);
        map.SetObjectPosition(newObejct);
    }
    public void MovePlayer()
    {
        map.MovePlayerPosition();
    }

    public void SetPlayerPos()
    {
        map.SetPlayerPosition();
    }
}
