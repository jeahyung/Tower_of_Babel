//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PatrolState : MonsterState
//{
//    private int patrolIndex = 0;

//    public PatrolState(MonsterAI monster) : base(monster) { }

//    public override void Enter()
//    {
//        // ���� ����
//        monster.MoveTo(patrolIndex);
//    }

//    public override void Update()
//    {
//        // �÷��̾���� �Ÿ� üũ
//        float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.player.transform.position);
//        if (distanceToPlayer <= monster.chaseDistance)
//        {
//            monster.SetState(new ChaseState(monster)); // �߰� ���·� ��ȯ
//        }
//        else if (monster.HasReachedDestination())
//        {
//            patrolIndex = (patrolIndex + 1) % monster.patrolPoints.Length; // ���� ���� �������� �̵�
//            monster.MoveTo(patrolIndex);
//        }
//    }

//    public override void Exit() { }
//}
