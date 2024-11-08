using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public List<ItemUISlot> slots = new List<ItemUISlot>();

    public GameObject itemExplain;
    public TMP_Text itemText;

    public List<Item> upItems = new List<Item>();   //업글 시 추가할 아이템들
    private void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ItemUISlot>());

        //SortItem(ItemInventory.instance.items);
    }
    private void Start()
    {
        HideExplain();
    }
    public void SortItem(List<Item> items)
    {
        if(items == null) { return; }
        int i = 0;
        foreach (ItemUISlot slot in slots)
        {
            slot.SetSlot(items[i]);
            i++;
        }
    }
    public bool PickUpItem(Item i)
    {
        if(i.canAdd == true)
        {
            foreach (ItemUISlot slot in slots)
            {
                if (slot.addItem != null && slot.addItem.id == i.id)
                {
                    //이펙트 재생
                    slot.RemoveItem();
                    slot.SetSlot(upItems[0]);
                    return true;
                }
            }
        }

        foreach (ItemUISlot slot in slots)
        {
            if (slot.addItem == null)
            {
                slot.SetSlot(i);

                return true;
            }
        }
        return false;
    }
    public bool RemoveItem(Item i)
    {
        foreach (ItemUISlot slot in slots)
        {
            if (slot.addItem != null && slot.addItem == i)
            {
                slot.RemoveItem();
                HideExplain();
                return true;
            }
        }
        return false;
    }

    public void ChangeItem(List<Item> items)
    {
        int j = 0;
        foreach (ItemUISlot slot in slots)
        {
            int i = Random.Range(0, items.Count);
            if(slot.addItem != null)
            {
                if(slot.addItem.id == 10) { ItemInventory.instance.SetBoxCount(-1); }
                slot.ChangeItem(items[i]);
                ItemInventory.instance.ChangeGetList(j, items[i]);
                if(items[i].id == 10) { ItemInventory.instance.SetBoxCount(1); }
            }
            j++;
        }
    }

    public void SetText(string t)
    {
        itemText.text = t;
    }
    public void ShowExplain()
    {
        itemExplain.SetActive(true);
    }
    public void HideExplain()
    {
        itemExplain.SetActive(false);
    }
}
