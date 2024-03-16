using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedObject : MonoBehaviour
{
    protected int hp = 1;
    public virtual void DestroyObj(int i = 1)
    {
        hp -= i;
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
