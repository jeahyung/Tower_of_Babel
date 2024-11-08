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

    private void Start()
    {
        for (int i = 0; i < knightList.Count; ++i)
        {
            //knightList[i].InitMob();
            //추가수정 필수
        }
    }

    public void AddMob(TraceMonsterMovement mob)
    {
        if (mob == null) { return; }
        mob.transform.SetParent(transform);
        knightList.Add(mob);

        mCount++;
        if (mi != 0) { mi = 0; }
    }
    public void InitMob()
    {
        knightList.Clear();
        mCount = 0;
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

    public void RemoveMob(TraceMonsterMovement m)
    {
        knightList.Remove(m);
        mi = 0;
        mCount--;
    }

    public List<Tile> ShowMobTile()
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = 0; i < knightList.Count; ++i)
        {
            tiles.Add(knightList[i].ShowTile());
        }
        return tiles;
    }
    public List<Mob> GetChase()
    {
        List<Mob> mobs = new List<Mob>();
        for (int i = 0; i < knightList.Count; ++i)
        {
            mobs.Add(knightList[i].GetComponent<Mob>());
        }
        return mobs;
    }
}
