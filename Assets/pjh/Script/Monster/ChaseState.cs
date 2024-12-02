//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ChaseState : MonsterState
//{
//    public ChaseState(MonsterAI monster) : base(monster) { }

//    public override void Enter()
//    {
//        // 추격 시작
//        monster.StartChasingPlayer();
//    }

//    //public override void Update()
//    //{
//    //    float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.player.transform.position);
//    //    if (distanceToPlayer > monster.chaseDistance)
//    //    {
//    //        monster.SetState(new ReturnState(monster)); // 복귀 상태로 전환
//    //    }
//    //    else
//    //    {
//    //        monster.ChasePlayer(); // 계속 추격
//    //    }
//    //}

//    public override void Exit() { }
//}

