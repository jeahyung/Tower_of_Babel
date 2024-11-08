using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    private void Start()
    {
        range = 10;
    }
    public override void SelectItem()
    {
        if(manager_Item == null) { manager_Item = FindObjectOfType<ItemManager>(); }

        range = 10;
        manager_Item.SelectItem_Key(this);
    }
    public override bool UseItem()
    {
        //manager_Item.CreateObject(itemPrefab);
        return CheckMob();
    }

    public bool CheckMob()
    {
        Rook rook = manager_Item.FindRook();
        if (rook != null)
        {
            rook.OpenRook();
            ScoreManager.instance.KillMob();    //스코어
            manager_Item.NextTurn();
            return true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    Debug.Log("find");
                    hit.collider.GetComponent<Rook>().OpenRook();
                    ScoreManager.instance.KillMob();    //스코어
                    manager_Item.NextTurn();
                    return true;
                }
            }
        }
        return false;
    }
}
