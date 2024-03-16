using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    public List<ItemUISlot> slots = new List<ItemUISlot>();

    private void Awake()
    {
        slots.AddRange(GetComponentsInChildren<ItemUISlot>());
    }

    public void PickUpItem(Item i)
    {
        foreach(ItemUISlot slot in slots)
        {
            if(slot.addItem == null)
            {
                slot.SetSlot(i);
                break;
            }
        }
    }
}
