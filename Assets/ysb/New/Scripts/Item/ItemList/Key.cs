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
        manager_Item.SeletItem_Eight(this, range);
    }
    public override void UseItem()
    {
        //manager_Item.CreateObject(itemPrefab);
        CheckMob();
    }

    public void CheckMob()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000);
            foreach (var hit in hits)
            {
                if(hit.collider.CompareTag("Wall"))
                {
                    Debug.Log("find");
                    hit.collider.GetComponent<Rook>().OpenRook();
                    ScoreManager.instance.KillMob();    //½ºÄÚ¾î
                    manager_Item.NextTurn();
                    return;
                }
                
            }
        }
        manager_Item.CancelItem();
    }
}
