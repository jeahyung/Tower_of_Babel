using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : Singleton<ItemInventory>
{
    //public static ItemInventory instance;

    //public List<ItemUISlot> slots = new List<ItemUISlot>();
    //public List<Item> items = new List<Item>();

    //아이템 데이터
    private List<Item> datas = new List<Item>();
    public string path = "Prefabs/Item/";

    public ItemUI itemUI;
    private void Start()
    {
        itemUI = FindObjectOfType<ItemUI>();

        datas.Clear();
        datas.AddRange(Resources.LoadAll<Item>(path));

        int bc = UpgradeManager.instance.getBonusItem();
        for (int i = 0; i < bc; ++i)
        {
            AddBonusItem();
        }
        //UpgradeManager.instance.getBonusItem(-bc);
    }

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    //slots.Clear();

    //    //아이템 데이터 로드
    //    datas.Clear();
    //    datas.AddRange(Resources.LoadAll<Item>(path));

    //    //slots.Clear();
    //    //slots.AddRange(GetComponentsInChildren<ItemUISlot>());
    //}

    public void AddBonusItem()
    {
        int rand = Random.Range(0, datas.Count);
        PickUpItem(datas[rand]);
    }
    public bool PickUpItem(Item i)
    {
        if(itemUI.PickUpItem(i) == true)
        {
            //items.Add(i);
            return true;
        }
        return false;
    }
    public void RemoveItem(Item i)
    {
        if(itemUI.RemoveItem(i) == true)
        {
            //items.Remove(i);
        }
    }
}
