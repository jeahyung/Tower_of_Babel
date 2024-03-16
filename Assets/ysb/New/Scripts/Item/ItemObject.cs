using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemObject : MonoBehaviour
{
    public Item item;
    //public Sprite img;

    private void Awake()
    {
        //item = new Item(img);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //other.GetComponent<ItemManager>().GetItem(item);
            this.gameObject.SetActive(false);
        }
    }
}
