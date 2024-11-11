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

    //public static List<Item> getItems = new List<Item>();    //얻은 아이템들

    public ItemUI itemUI;

    private int count_useItem = 0;  //사용한 아이템 갯수
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
        //for (int i = 0; i < getItems.Count; ++i)
        //{
        //    itemUI.PickUpItem(getItems[i]);
        //}
    }

    //게임 시작시 세팅돼야 하는 것들
    public void StartGame()
    {
        int bc = UpgradeManager.instance.getBonusItem();
        for (int i = 0; i < bc; ++i)
        {
            AddBonusItem();
        }
        count_useItem = 0;

        int box = UpgradeManager.instance.GetBoxCount();
        for (int i = 0; i < box; ++i)
        {
            AddBox();
        }
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
    public void AddBox()
    {
        PickUpItem(datas[datas.Count - 1]);
    }
    public bool PickUpItem(Item i)
    {
        if(itemUI.PickUpItem(i) == true)
        {
            int id = i.id;
            if(id == 10)    //랜덤 박스
            {
                UpgradeManager.instance.SetBoxCount(1);
            }
            //for(int j = 0; j < datas.Count; ++j)
            //{
            //    if(datas[j].id == id)
            //    {
            //        getItems.Add(datas[j]);
            //    }
            //}
            //items.Add(i);
            return true;
        }
        return false;
    }
    public void RemoveItem(Item i)
    {
        bool b = itemUI.RemoveItem(i);
        if (b == true)
        {
            count_useItem++;
            if(i.id == 10) { UpgradeManager.instance.SetBoxCount(-1); }
            //for(int j = 0; j < getItems.Count; ++j)
            //{
            //    if(i.id == getItems[j].id)
            //    {
            //        getItems.Remove(getItems[j]);
            //    }
            //}
            //items.Remove(i);
        }
        Debug.Log(b);
    }

    //매턴 마다
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
        //if (num >= getItems.Count) { return; }
        //getItems[num] = i;
    }
    public void SetBoxCount(int i)
    {
        UpgradeManager.instance.SetBoxCount(i);
    }
}
