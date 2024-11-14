using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : Singleton<ItemInventory>
{
    //아이템 데이터
    private List<Item> datas = new List<Item>();
    public string path = "Prefabs/Item/";
    public ItemUI itemUI;
    private int count_useItem = 0;  //사용한 아이템 갯수
    public int Count_ItemUse => count_useItem;
    private void Start()
    {
        itemUI = FindObjectOfType<ItemUI>();
        datas.Clear();
        datas.AddRange(Resources.LoadAll<Item>(path));

        int box = UpgradeManager.instance.GetBoxCount();
        for (int i = 0; i < box; ++i)
        {
            AddBox();
        }
    }
    public void ResetItem()
    {
        //getItems.Clear();
    }

    public void SetItem()
    {
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
        Item i = datas[datas.Count - 1];
        if (itemUI.PickUpItem(i) == true)
        {
            return;
        }
    }
    public bool PickUpItem(Item i)
    {
        if(itemUI.PickUpItem(i) == true)
        {
            int id = i.id;
            if (id == 20)    //랜덤 박스
            {
                UpgradeManager.instance.SetBoxCount(1);
            }
            return true;
        }
        return false;
    }
    public void RemoveItem(Item i)
    {
        Debug.Log("use box");
        bool b = itemUI.RemoveItem(i);
        if (b == true)
        {
            count_useItem++;
        }
    }

    //매턴 마다
    public void ChangeItem()
    {
        bool bi = UpgradeManager.instance.getItemChange();
        Debug.Log(bi.ToString() + "체이니 아이템");
        if (bi == true)
        {
            itemUI.ChangeItem(datas);
        }
    }
    public void ChangeGetList(int num, Item i)
    {
    }
    public void SetBoxCount(int i)
    {
        UpgradeManager.instance.SetBoxCount(i);
    }
}
