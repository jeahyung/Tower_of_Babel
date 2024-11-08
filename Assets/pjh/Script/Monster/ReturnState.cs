//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ReturnState : MonsterState
//{
//    public ReturnState(MonsterAI monster) : base(monster) { }

//    public override void Enter()
//    {
//        // 복귀 시작
//        monster.MoveToStartPoint();
//    }

//    public override void Update()
//    {
//        if (monster.HasReachedStartPoint())
//        {
//            monster.SetState(new PatrolState(monster)); // 순찰 상태로 전환
//        }
//    }

//    public override void Exit() { }
//}

