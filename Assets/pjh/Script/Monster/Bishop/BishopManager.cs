using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopManager : MonoBehaviour
{
    private List<MonsterAI> bishopList = new List<MonsterAI>();

    public int mi = 0;
    public int mCount = 0;

    private void Awake()
    {
        bishopList.Clear();
        bishopList.AddRange(GetComponentsInChildren<MonsterAI>());

        mi = 0;
        mCount = bishopList.Count;
    }

    private void Start()
    {
        for (int i = 0; i < bishopList.Count; ++i)
        {
            bishopList[i].InitMob();
            //knightList[i].InitMob();
            //추가수정 필수
        }
    }

    public void AddMob(MonsterAI mob)
    {
        if (mob == null) { return; }
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
        if (++mi >= mCount)
        {
            mi = 0;
            transform.parent.SendMessage("EndBishop");
        }
        else
        {
      
            bishopList[mi].Act();
        }
    }
    public void StartActMob()
    {
        if (mCount == 0)
        {
            transform.parent.SendMessage("EndBishop");
            return;
        }
        mi = 0;
     
        bishopList[mi].Act();
    }

    public void RemoveMob(MonsterAI m)
    {
        bishopList.Remove(m);
        mi = 0;
        mCount--;
    }

    public List<Tile> ShowMobTile()
    {
        List<Tile> tiles = new List<Tile>();
        for (int i = 0; i < bishopList.Count; ++i)
        {
            tiles.Add(bishopList[i].ShowTile());
        }
        return tiles;
    }
    public List<Mob> GetChase()
    {
        List<Mob> mobs = new List<Mob>();
        for (int i = 0; i < bishopList.Count; ++i)
        {
            mobs.Add(bishopList[i].GetComponent<Mob>());
        }
        return mobs;
    }
}
