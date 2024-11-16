using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedObject : MonoBehaviour
{
    protected int hp = 1;

    private Map map;

    private void Start()
    {
        map = FindObjectOfType<Map>();
    }

    public virtual bool DestroyObj(int i = 1)
    {
        Debug.Log("using Dia!!!!!!!!!!!!!!!!!!!!!!!!!!!######");
        hp -= i;
        EffectManage.Instance.PlayEffect("Diamond_Destroy", this.transform.position);
        if (hp <= 0)
        {
            if(this.gameObject != null)
            {
                this.gameObject.SetActive(false);
               map.ChageTempTile();
                return true;
            }

        }
        return false;
    }
}
