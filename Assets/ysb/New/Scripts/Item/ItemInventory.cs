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

    public ItemUI itemUI;

    private int count_useItem = 0;  //����� ������ ����
    private void Start()
    {
        itemUI = FindObjectOfType<ItemUI>();

        datas.Clear();
        datas.AddRange(Resources.LoadAll<Item>(path));
        //UpgradeManager.instance.getBonusItem(-bc);
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
            return true;
        }
        return false;
    }
    public void RemoveItem(Item i)
    {
        if(itemUI.RemoveItem(i) == true)
        {
            count_useItem++;
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
}
