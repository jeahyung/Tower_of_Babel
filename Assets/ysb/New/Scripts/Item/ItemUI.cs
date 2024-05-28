using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public List<ItemUISlot> slots = new List<ItemUISlot>();


    private void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ItemUISlot>());

        //SortItem(ItemInventory.instance.items);
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
                return true;
            }
        }
        return false;
    }

    public void ChangeItem(List<Item> items)
    {
        foreach (ItemUISlot slot in slots)
        {
            int i = Random.Range(0, items.Count);
            if(slot.addItem != null)
            {
                slot.SetSlot(items[i]);
            }
        }
    }
}
