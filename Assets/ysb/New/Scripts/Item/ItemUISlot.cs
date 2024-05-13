using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUISlot : MonoBehaviour
{
    [SerializeField]
    public Item addItem = null;
    private Image img;
    private Button btn;

    private void Awake()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();

        if(addItem == null) { img.sprite = null; }
    }

    public void SetSlot(Item item)
    {
        //if(item == null) { return; }
        if(addItem != null) { return; }
        addItem = item;
        img.sprite = addItem.itemImg;

        btn.onClick.AddListener(() => addItem.SelectItem());

        //img.enabled = true;
    }

    public void RemoveItem()
    {
        if(addItem == null) { return; }
        addItem = null;
        img.sprite = null;

        btn.onClick.RemoveAllListeners();
        //img.enabled = false;
    }
}
