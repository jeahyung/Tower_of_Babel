using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBoxNextWall : CheckBox_Old
{
    public override BoxMove CheckBlock(Vector3 dir)
    {
        box = null;
        canMove = true;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, 10f))
        {
            box = hit.collider.GetComponent<BoxMove>();
            box.canMove = true;

            if (BoxCanMove(dir) == false) { box.canMove = false; }
            if (direction.x == dir.x || direction.z == dir.z)
            {
                canMove = false;    //이제 못 움직임.
                box.canMove = false;
            }
            return box;
        }
        return null;
    }

    //public override bool BoxCanMove(Vector3 dir)
    //{
    //    if (dir == Vector3.left)
    //    {
    //        if (leftBox == null || leftBox.canMove == false)
    //        {
    //            canMove = false;
    //            return false;
    //        }
    //    }
    //    else if (dir == Vector3.right)
    //    {
    //        if (rightBox == null || rightBox.canMove == false)
    //        {
    //            canMove = false;
    //            return false;
    //        }
    //    }
    //    else if (dir == Vector3.forward)
    //    {
    //        if (upBox == null || upBox.canMove == false)
    //        {
    //            canMove = false;
    //            return false;
    //        }
    //    }
    //    else if (dir == Vector3.back)
    //    {
    //        if (downBox == null || downBox.canMove == false)
    //        {
    //            canMove = false;
    //            return false;
    //        }
    //    }
    //    return true;
    //}
}
