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
            //other.GetComponent<ItemManager>().GetItem(item);
            inven.PickUpItem(item);
            this.gameObject.SetActive(false);

        }
    }
}
