using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Map map;
    private ItemInventory inven;
    private Item selectedItem;

    private List<CreatedObject> objs = new List<CreatedObject>();

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

        objs.Add(newObejct.GetComponent<CreatedObject>());
    }
    public void MovePlayer()
    {
        map.MovePlayerPosition();
    }

    public void SetPlayerPos()
    {
        map.SetPlayerPosition();
    }

    //설치물 제거
    public void RemoveObj()
    {
        List<CreatedObject> tempList = new List<CreatedObject>();
        foreach(var obj in objs)
        {
            //if(obj.gameObject == null) { return; }
            if(obj.DestroyObj() == true) { tempList.Add(obj); }
        }
        for(int i = 0; i < tempList.Count;++i)
        {
            objs.Remove(tempList[i]);
        }
    }

    public void RemoveList(CreatedObject i)
    {
        objs.Remove(i);
    }
}
