using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterState
{
    protected MonsterAI monster;

    public MonsterState(MonsterAI monster)
    {
        this.monster = monster;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
