using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMobManager : MonoBehaviour
{
    private List<TraceMonsterMovement> knightList = new List<TraceMonsterMovement>();

    public int mi = 0;
    public int mCount = 0;

    private void Awake()
    {
        knightList.Clear();
        knightList.AddRange(GetComponentsInChildren<TraceMonsterMovement>());

        mi = 0;
        mCount = knightList.Count;
    }
    public void CheckMobAction()
    {
        if (++mi >= mCount)
        {
            mi = 0;
            transform.parent.SendMessage("EndChase");
        }
        else
        {
            knightList[mi].Act();
        }
    }
    public void StartActMob()
    {
        if(mCount == 0)
        {
            transform.parent.SendMessage("EndChase");
            return;
        }
        mi = 0;
        knightList[mi].Act();
    }
}
