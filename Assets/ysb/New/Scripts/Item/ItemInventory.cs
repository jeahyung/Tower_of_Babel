using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : Singleton<ItemInventory>
{
    //public static ItemInventory instance;

    //public List<ItemUISlot> slots = new List<ItemUISlot>();
    //public List<Item> items = new List<Item>();

    //������ ������
    private List<Item> datas = new List<Item>();
    public string path = "Prefabs/Item/";

    //public static List<Item> getItems = new List<Item>();    //���� �����۵�

    public ItemUI itemUI;

    private int count_useItem = 0;  //����� ������ ����
    public int Count_ItemUse => count_useItem;
    private void Start()
    {
        itemUI = FindObjectOfType<ItemUI>();

        datas.Clear();
        datas.AddRange(Resources.LoadAll<Item>(path));

        //SetItem();
        //UpgradeManager.instance.getBonusItem(-bc);
    }

    public void ResetItem()
    {
        //getItems.Clear();
    }

    public void SetItem()
    {
        //for(int i = 0; i < getItems.Count; ++i)
        //{
        //    PickUpItem(getItems[i]);
        //}
    }

    //���� ���۽� ���õž� �ϴ� �͵�
    public void StartGame()
    {
        int bc = UpgradeManager.instance.getBonusItem();
        for (int i = 0; i < bc; ++i)
        {
            AddBonusItem();
        }
        count_useItem = 0;
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

    //    //������ ������ �ε�
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
            //Item getItem = i;
            //getItems.Add(getItem);
            return true;
        }
        return false;
    }
    public void RemoveItem(Item i)
    {
        if(itemUI.RemoveItem(i) == true)
        {
            count_useItem++;
            //getItems.Remove(i);
            //items.Remove(i);
        }
    }

    //���� ����
    public void ChangeItem()
    {
        bool bi = UpgradeManager.instance.getItemChange();
        if (bi == true)
        {
            itemUI.ChangeItem(datas);
        }
    }
    public void ChangeGetList(int num, Item i)
    {
        //if(num >= getItems.Count) { return; }
        //getItems[num] = i;
    }
}
