//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PatrolState : MonsterState
//{
//    private int patrolIndex = 0;

//    public PatrolState(MonsterAI monster) : base(monster) { }

//    public override void Enter()
//    {
//        // 순찰 시작
//        monster.MoveTo(patrolIndex);
//    }

//    public override void Update()
//    {
//        // 플레이어와의 거리 체크
//        float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.player.transform.position);
//        if (distanceToPlayer <= monster.chaseDistance)
//        {
//            monster.SetState(new ChaseState(monster)); // 추격 상태로 전환
//        }
//        else if (monster.HasReachedDestination())
//        {
//            patrolIndex = (patrolIndex + 1) % monster.patrolPoints.Length; // 다음 순찰 지점으로 이동
//            monster.MoveTo(patrolIndex);
//        }
//    }

//    public override void Exit() { }
//}
