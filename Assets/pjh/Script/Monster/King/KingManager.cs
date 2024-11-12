using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingManager : MonoBehaviour
{
    private List<KingMob> KingList = new List<KingMob>();
    private MobManager manager;
    public int mi = 0;
    public int mCount = 0;

    private void Awake()
    {
        KingList.Clear();
        KingList.AddRange(GetComponentsInChildren<KingMob>());
        manager = FindObjectOfType<MobManager>();
        mi = 0;
        mCount = KingList.Count;
    }

    public void CheckMobAction()
    {
        if (++mi >= mCount)
        {
            mi = 0;
            //transform.parent.SendMessage("EndKing");
            manager.EndKing();
        }
        else
        {
            KingList[mi].Act();
        }
    }
    public void StartActMob()
    {
        if (mCount == 0)
        {
            //transform.parent.SendMessage("EndKing");
            manager.EndKing();
            return;
        }
        mi = 0;
        KingList[mi].Act();
    }

    public void RemoveMob(KingMob m)
    {
        KingList.Remove(m);
        mi = 0;
        mCount--;
    }

    public void OffSkill()
    {
       
            KingList[mi].BurnOff();
       
        
    }
}
