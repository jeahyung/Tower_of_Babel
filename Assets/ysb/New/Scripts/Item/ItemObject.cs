using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour
{
    private ItemInventory inven;
    public Item item;
    //public Sprite img;

    private void Awake()
    {
        item = GetComponent<Item>();
        inven = FindObjectOfType<ItemInventory>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bool canGet = inven.PickUpItem(item);
            if(canGet == true)
            {
                ScoreManager.instance.Score_ItemGet();
                this.gameObject.SetActive(false);
            }
        }
    }
}
