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
        int j = 0;
        foreach (ItemUISlot slot in slots)
        {
            int i = Random.Range(0, items.Count);
            if(slot.addItem != null)
            {
                slot.ChangeItem(items[i]);
                ItemInventory.instance.ChangeGetList(j, items[i]);
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
