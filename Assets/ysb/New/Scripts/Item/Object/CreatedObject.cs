using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedObject : MonoBehaviour
{
    protected int hp = 1;
    public virtual bool DestroyObj(int i = 1)
    {
        Debug.Log("using Dia!!!!!!!!!!!!!!!!!!!!!!!!!!!######");
        hp -= i;
        if(hp <= 0)
        {
            if(this.gameObject != null)
            {
                Destroy(this.gameObject);
                return true;
            }

        }
        return false;
    }
}
