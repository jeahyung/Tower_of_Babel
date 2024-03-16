using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUISlot : MonoBehaviour
{
    [SerializeField]
    public Item addItem = null;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void SetSlot(Item item)
    {
        if(item == null) { return; }
        addItem = item;
        img.sprite = addItem.itemImg;
    }
}
