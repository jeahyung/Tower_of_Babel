using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Clock : MonoBehaviour
{
    ItemManager item;
    public int i;
    // Start is called before the first frame update
    void Start()
    {
        item = FindObjectOfType<ItemManager>();
    }

    public void Ok()
    {
        item.UseItem();
        gameObject.SetActive(false);
    }
    public void Cancle()
    {
        item.CancelItem();
        gameObject.SetActive(false);
    }
}
