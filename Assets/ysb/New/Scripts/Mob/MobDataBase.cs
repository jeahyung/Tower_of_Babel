using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobData_P
{
    public int id;
    public int moveX;
    public int moveY;
    public int rangeR;
    public int rangeL;

    public MobData_P(int i, int mx, int my, int rr, int rl)
    {
        id = i;
        moveX = mx;
        moveY = my;
        rangeR = rr;
        rangeL = rl;
    }
}
public class MobDataBase : Singleton<MobDataBase>
{
    public string fileName = "";
    List<Dictionary<string, object>> data_p = new List<Dictionary<string, object>>();
    List<MobData_P> pMobs = new List<MobData_P>();


    private void Start()
    {
        LoadPatralMob();
    }
    public void LoadPatralMob()
    {
        fileName = "Mob_P";
        data_p.Clear();
        data_p = CSVReader.Read(fileName);

        for (int i = 0; i < data_p.Count; ++i)
        {
            int id = int.Parse(data_p[i]["ID"].ToString());
            int dirX = int.Parse(data_p[i]["Move_X"].ToString());
            int dirY = int.Parse(data_p[i]["Move_Y"].ToString());
            int rangeR = int.Parse(data_p[i]["Range_R"].ToString());
            int rangeL = int.Parse(data_p[i]["Range_L"].ToString());

            MobData_P mob = new MobData_P(id, dirX, dirY, rangeR, rangeL);
            pMobs.Add(mob);
        }
        
    }


    public MobData_P GetpMobData()
    {
        Debug.Log("load");
        return pMobs[Random.Range(0, pMobs.Count)];
    }
}
