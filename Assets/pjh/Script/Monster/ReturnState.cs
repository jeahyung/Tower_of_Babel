//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ReturnState : MonsterState
//{
//    public ReturnState(MonsterAI monster) : base(monster) { }

//    public override void Enter()
//    {
//        // ���� ����
//        monster.MoveToStartPoint();
//    }

//    public override void Update()
//    {
//        if (monster.HasReachedStartPoint())
//        {
//            monster.SetState(new PatrolState(monster)); // ���� ���·� ��ȯ
//        }
//    }

//    public override void Exit() { }
//}

