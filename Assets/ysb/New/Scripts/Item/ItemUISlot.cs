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

        if(addItem == null) {             
            img.sprite = null;
            img.enabled = false;
        }
    }

    public void ChangeItem(Item item)
    {
        if (addItem == null) { return; }
        addItem = item;
        img.enabled = true;
        img.sprite = addItem.itemImg;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => addItem.SelectItem());
    }

    public void SetSlot(Item item)
    {
        //if(item == null) { return; }
        if (addItem != null) { return; }
        addItem = item;
        img.enabled = true;
        img.sprite = addItem.itemImg;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => addItem.SelectItem());

        //img.enabled = true;
    }

    public void RemoveItem()
    {
        if(addItem == null) { return; }
        addItem = null;
        img.sprite = null;
        img.enabled = false;

        btn.onClick.RemoveAllListeners();
        //img.enabled = false;
    }

    public void Show()
    {
        if(addItem == null) { return; }
        GetComponentInParent<ItemUI>().ShowExplain();
        GetComponentInParent<ItemUI>().SetText(addItem.ex);
        //Debug.Log(addItem.ex);
    }
    
    public void Hide()
    {
        if(addItem == null) { return; }
        GetComponentInParent<ItemUI>().SetText("");
        GetComponentInParent<ItemUI>().HideExplain();

    }
}
