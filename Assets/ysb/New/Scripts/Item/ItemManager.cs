using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private EnergySystem energy;
    private TurnManager manager_Turn;
    private Map map;
    //private ItemInventory inven;
    private Item selectedItem;

    private List<CreatedObject> objs = new List<CreatedObject>();

    private void Awake()
    {
        map = FindObjectOfType<Map>();
        manager_Turn = GetComponent<TurnManager>();
        energy = GetComponent<EnergySystem>();
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
        if (selectedItem.UseItem() == false)
        {
            CancelItem();
            return;
        }
        ScoreManager.instance.Score_ItemUse();
        energy.UseEnergy();
        ItemInventory.instance.RemoveItem(selectedItem);
        selectedItem = null;
    }

    //아이템 사용 취소
    public void CancelItem()
    {
        //energy.UseEnergy(-energy.useEnergy);
        map.CancelItem();
        selectedItem = null;
    }

    public void CreateObject(GameObject obj)
    {
        GameObject newObejct = Instantiate(obj);
        map.SetObjectPosition(newObejct);

        objs.Add(newObejct.GetComponent<CreatedObject>());
    }
    public void MovePlayer(Item item)
    {
        map.MovePlayerPosition();
        selectedItem = item;
        ItemInventory.instance.RemoveItem(selectedItem);
    }

    public void SetPlayerPos(Item item)
    {
        map.SetPlayerPosition();
        selectedItem = item;
        ItemInventory.instance.RemoveItem(selectedItem);
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

    //열기
    public void Open()
    {

    }

    public void NextTurn()
    {
        manager_Turn.EndPlayerTurn();
    }
}
