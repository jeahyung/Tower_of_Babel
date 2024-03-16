using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBox_Old : MonoBehaviour
{
    public bool canMove = true;

    public CheckBox_Old leftBox;
    public CheckBox_Old rightBox;
    public CheckBox_Old upBox;
    public CheckBox_Old downBox;

    public BoxMove box = null;

    public Vector3 direction;
    public virtual BoxMove CheckBlock(Vector3 dir)
    {
        box = null;
        canMove = true;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.up, out hit, 10f))
        {
            box = hit.collider.GetComponent<BoxMove>();
            box.canMove = true;

            if (BoxCanMove(dir) == false) { box.canMove = false; }

            return box;
        }
        return null;
    }

    public virtual bool BoxCanMove(Vector3 dir)
    {
        if (dir == Vector3.left)
        {
            if (leftBox != null && leftBox.canMove == false)
            {
                canMove = false;
                return false;
            }
        }
        else if (dir == Vector3.right)
        {
            if (rightBox != null && rightBox.canMove == false)
            {
                canMove = false;
                return false;
            }
        }
        else if (dir == Vector3.forward)
        {
            if (upBox != null && upBox.canMove == false)
            {
                canMove = false;
                return false;
            }
        }
        else if (dir == Vector3.back)
        {
            if (downBox != null && downBox.canMove == false)
            {
                canMove = false;
                return false;
            }
        }
        return true;
    }
}
