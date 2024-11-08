using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Stop : MonoBehaviour
{
    ItemManager item;
    private void Awake()
    {
        item = GameObject.FindWithTag("Player").GetComponent<ItemManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            item.NextTurn();
        }
    }
}
