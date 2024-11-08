using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PatrolMobManager : MonoBehaviour
{
    //private List<MobMovement> bishopList = new List<MobMovement>();
    private List<MonsterAI> bishopList = new List<MonsterAI>();

    public int mi = 0;
    public int mCount = 0;

    private void Awake()
    {
        bishopList.Clear();
    //    bishopList.AddRange(GetComponentsInChildren<MobMovement>());
        bishopList.AddRange(GetComponentsInChildren<MonsterAI>());

        mi = 0;
        mCount = bishopList.Count;
    }
    public void CheckMobAction()
    {
        if(++mi >= mCount)
        {
            mi = 0;
            transform.parent.SendMessage("EndPatrol");
        }
        else
        {
            bishopList[mi].Act();
        }
    }
    public void StartActMob()
    {
        if(mCount == 0)
        {
            transform.parent.SendMessage("EndPatrol");
            return;
        }
        mi = 0;
        bishopList[mi].Act();
    }    

}
