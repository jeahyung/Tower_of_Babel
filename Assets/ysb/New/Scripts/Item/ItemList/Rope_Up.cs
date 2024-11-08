using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope_Up : Rope
{
    public override bool CheckMob()
    {
        manager_Item.UseRope2();
        manager_Item.NextTurn();
        return true;
    }
}
