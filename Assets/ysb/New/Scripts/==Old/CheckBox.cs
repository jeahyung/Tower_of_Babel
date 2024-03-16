using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox : MonoBehaviour
{
    private Collider boxCollider;
    [SerializeField]
    private Block findBlock;    //찾아낸 블록

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        CheckBlock();
    }

    public void CheckBlock()
    {
        Vector3 size = new Vector3(transform.localScale.x / 2, transform.localScale.y / 2, transform.localScale.z / 2);
        Collider[] col = Physics.OverlapBox(transform.position, size, transform.rotation, 1 << LayerMask.NameToLayer("Block"));

        for (int i = 0; i < col.Length; ++i)
        {
            if (col[i].GetComponent<Block>() != null)
            {
                findBlock = col[i].GetComponent<Block>();
                break;
            }

        }
    }

    public Block GetBlock()
    {
        if(findBlock == null)
        {
            CheckBlock();
        }
        return findBlock;
    }
}
