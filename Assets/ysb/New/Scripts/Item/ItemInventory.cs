using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemInventory : Singleton<ItemInventory>
{
    public List<ItemUISlot> slots = new List<ItemUISlot>();

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        slots.Clear();
        slots.AddRange(GetComponentsInChildren<ItemUISlot>());
    }

    public void PickUpItem(Item i)
    {
        foreach (ItemUISlot slot in slots)
        {
            if (slot.addItem == null)
            {
                slot.SetSlot(i);
                break;
            }
        }
    }
    public void RemoveItem(Item i)
    {
        foreach (ItemUISlot slot in slots)
        {
            if (slot.addItem != null && slot.addItem == i)
            {
                slot.RemoveItem();
                
                break;
            }
        }
    }
}
