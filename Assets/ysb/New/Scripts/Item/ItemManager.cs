using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private PlayerMovement player;
    private EnergySystem energy;
    private TurnManager manager_Turn;
    private Map map;
    //private ItemInventory inven;
    private Item selectedItem;

    private List<CreatedObject> objs = new List<CreatedObject>();

    GameObject RopeUI;
    GameObject[] ClockUI = new GameObject[2];
    GameObject BoxUI;

    private MobManager mob = null;
    private void Awake()
    {
        map = FindObjectOfType<Map>();
        manager_Turn = GetComponent<TurnManager>();
        energy = GetComponent<EnergySystem>();
        player = GetComponent<PlayerMovement>();

        RopeUI = GameObject.Find("RopeUI");
        RopeUI.SetActive(false);

        ClockUI[0] = GameObject.Find("ClockUI");
        ClockUI[0].SetActive(false);
        ClockUI[1] = GameObject.Find("ClockUI (1)");
        ClockUI[1].SetActive(false);

        BoxUI = GameObject.Find("BoxUI");
        BoxUI.SetActive(false);
    }

    public void SeletItem_Four(Item item, int range)
    {
        if(map.canControl == false) { return; }
        mob = StageManager.instance.mob;
        selectedItem = item;
        mob.isUseItem = true;
        map.SelectItem(item);
        map.UseItem_Four(range);
    }

    public void SeletItem_Eight(Item item, int range)
    {
        if (map.canControl == false) { return; }
        mob = StageManager.instance.mob;
        selectedItem = item;
        mob.isUseItem = true;
        map.SelectItem(item);
        map.UseItem_Eight(range);
    }

    public void SelectItem_Key(Item item)
    {
        if (map.canControl == false) { return; }
        mob = StageManager.instance.mob;
        selectedItem = item;
        map.SelectItem(item);
        mob.isUseItem = true;
        mob.isKey = true;
        map.UseItem_Key();
    }

    public void SelectItem_Key2(Item item)
    {
        if (map.canControl == false) { return; }
        mob = StageManager.instance.mob;
        selectedItem = item;
        map.SelectItem(item);
        mob.isUseItem = true;
        mob.isKey2 = true;
        map.UseItem_Key2();
    }

    public void SelectItem_Rope(Item item)
    {
        if (map.canControl == false) { return; }
        mob = StageManager.instance.mob;
        selectedItem = item;
        mob.isUseItem = true;
        map.SelectItem(item);
        ShowRopeUI();
        //map.UseItem_Rope();
    }

    public void SelectItem_Box(Item item)
    {
        if (map.canControl == false) { return; }
        mob = StageManager.instance.mob;
        selectedItem = item;
        mob.isUseItem = true;
        map.SelectItem(item);
        BoxUI.SetActive(true);
    }

    public void UseItem()
    {
        mob = StageManager.instance.mob;
        if (selectedItem.UseItem() == false)
        {
            CancelItem();
            return;
        }
        player.UseItem();   //애니
        EffectManage.Instance.PlayEffect("Item_Use", player.transform.position);
        ScoreManager.instance.Score_ItemUse();
        energy.UseEnergy();
        ItemInventory.instance.RemoveItem(selectedItem);
        selectedItem = null;
        map.useItem = false;
        mob.isKey = false;
        mob.isUseItem = false;
    }

    //아이템 사용 취소
    public void CancelItem()
    {
        //energy.UseEnergy(-energy.useEnergy);
        map.CancelItem();
        selectedItem = null;
        mob.isUseItem = false;
    }

    public void CreateObject(GameObject obj)
    {
        GameObject newObejct = Instantiate(obj);
        newObejct.tag = "Dia";
        map.SetObjectPosition(newObejct);
   
        objs.Add(newObejct.GetComponent<CreatedObject>());
    }
    public void MovePlayer(Item item)
    {
        map.MovePlayerPosition();
        selectedItem = item;
        ItemInventory.instance.RemoveItem(selectedItem);
    }

    public void SetPlayerPos_UI(Item item, int i)
    {
        selectedItem = item;
        mob.isUseItem = true;
        ClockUI[i].SetActive(true);
    }
    
    public void SetPlayerPos_UI2(Item item)
    {
        selectedItem = item;
        mob.isUseItem = true;
        map.SelectItem_Clock();
        //ClockUI[i].SetActive(true);
    }

    public bool SetPlayerPos(Item item, int i)
    {
        
        bool isUse = map.SetPlayerPosition(i);
        if(isUse == false && i != -1) { ClockUI[i].SetActive(false); return false; }
        selectedItem = item;
        if(i!= -1) { ClockUI[i].SetActive(false); }
        ItemInventory.instance.RemoveItem(selectedItem);
        return true;
    }
    public bool SetPlayerPos2(Item item)
    {
        bool isUse = map.SetPlayerPosition2();
        if (isUse == false) { return false; }
        selectedItem = item;
        ItemInventory.instance.RemoveItem(selectedItem);
        return true;
    }

    public Rook FindRook()
    {
        return map.UseKey();
    }

    //로프 전용 - 츠적형, 순찰형
    public Mob FindMob()
    {
        return map.UseRope();
    }

    //설치물 제거
    public void CheckObj()
    {
        //설치물이 있다면
        if(objs.Count > 0) { map.SetPlayerTile(); }
    }

    public void RemoveObj()
    {
        List<CreatedObject> tempList = new List<CreatedObject>();
        foreach(var obj in objs)
        {
            //if(obj.gameObject == null) { return; }
            if(obj.DestroyObj() == true) { tempList.Add(obj); }
        }
        for(int i = 0; i < tempList.Count;++i)
        {
            objs.Remove(tempList[i]);
        }
    }

    public void RemoveList(CreatedObject i)
    {
        objs.Remove(i);
    }

    //열기
    public void Open()
    {

    }

    public void NextTurn()
    {
        RopeUI.SetActive(false);
        if(selectedItem != null) { ItemInventory.instance.RemoveItem(selectedItem); }
        manager_Turn.EndPlayerTurn();
    }


    public void UseRope1()
    {
        mob.DontMovePatrol();
        map.HideArea();
    }
    public void UseRope2()
    {
        mob.DontMoveChase();
        map.HideArea();
    }
    //ui
    public void ShowRopeUI()
    {
        RopeUI.SetActive(true);
    }
    public void HideRopeUI()
    {
        RopeUI.SetActive(false);
        CancelItem();
    }
}
