using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMobManager : MonoBehaviour
{
    private List<MobMovement> bishopList = new List<MobMovement>();

    public int mi = 0;
    public int mCount = 0;

    private void Awake()
    {
        bishopList.Clear();
        bishopList.AddRange(GetComponentsInChildren<MobMovement>());

        mi = 0;
        mCount = bishopList.Count;
    }

    private void Start()
    {
        for (int i = 0; i < bishopList.Count; ++i)
        {
            bishopList[i].InitMob();
        }
    }

    public void AddMob(MobMovement mob)
    {
        if(mob == null) { return; }

        mob.transform.SetParent(transform);
        bishopList.Add(mob);
        mob.InitMob();

        mCount++;
        if (mi != 0) { mi = 0; }
    }

    public void InitMob()
    {
        bishopList.Clear();
        mCount = 0;
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


    public void RemoveMob(MobMovement reMob)
    {
        bishopList.Remove(reMob);
        mi = 0;
        mCount--;
    }

    public List<Tile> ShowMobTile()
    {
        List<Tile> tiles = new List<Tile>();
        for(int i = 0; i < bishopList.Count; ++i)
        {
            tiles.Add( bishopList[i].ShowTile());
        }
        return tiles;
    }
    public List<Mob> GetPatrol()
    {
        List<Mob> mobs = new List<Mob>();
        for (int i = 0; i < bishopList.Count; ++i)
        {
            mobs.Add(bishopList[i].GetComponent<Mob>());
        }
        return mobs;
    }
}
