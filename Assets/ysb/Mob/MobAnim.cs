using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAnim : MonoBehaviour
{
    MobMovement mob;
    Queen queen;
    private void Awake()
    {
        mob = GetComponentInParent<MobMovement>();
        queen = GetComponentInParent<Queen>();
    }
    public void Attack()
    {
        mob.Attack();
    }
    public void AttackEnd()
    {
        mob.AttackEnd();
    }

    public void QueenAttack()
    {
        queen.Attack();
    }
}
