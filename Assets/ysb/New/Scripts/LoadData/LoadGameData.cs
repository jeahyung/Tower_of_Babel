using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class LoadGameData : Singleton<LoadGameData>
{
    public GameObject dddd;
    //로드
    private bool isLoad = false;
    //스코어
    List<Dictionary<string, object>> sd = new List<Dictionary<string, object>>();   
    //업그레이드
    //List<Dictionary<string, object>> ud = new List<Dictionary<string, object>>();   
    //아이템
    //List<Dictionary<string, object>> id = new List<Dictionary<string, object>>();

    private string scoreFile = "Score";
    //private string upgradeFile = "GetUpData";
    //private string itemFile = "ItemData";

    //세이브
    List<string[]> newData = new List<string[]>();
    string[] tempData;

    private void OnEnable()
    {
        LoadCSVFile();
        //ResetUpData();
        //dddd.SetActive(true);
    }
    public void SaveCSVFile(string fileName, List<string[]> data)
    {
        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        string filepath = SystemPath.GetPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        StreamWriter outStream = System.IO.File.CreateText(filepath + fileName + ".csv");
        outStream.Write(sb);
        outStream.Close();
    }

    public void LoadCSVFile()
    {
        //LoadData();
        LoadScoreDB();
        //LoadUpgrade();

        isLoad = true;
    }
    //public void LoadData()
    //{
    //    List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
    //    data = CSVReader.Read("GameData");

    //    ScoreManager.instance.SetSumSocre(int.Parse(data[0]["Score"].ToString()));
    //}
    //public void SaveGameData()
    //{
    //    List<string[]> nd = new List<string[]>();
    //    string[] td = new string[1];

    //    td[0] = "Score";
    //    nd.Add(td);

    //    td[0] = ScoreManager.instance.scoreSum.ToString();
    //    nd.Add(td);

    //    Debug.Log(nd.Count);
    //    //SaveCSVFile("GameData", nd);
    //}

    public void LoadScoreDB()
    {
        sd.Clear();
        sd = CSVReader.Read(scoreFile);
    }
    //public void LoadUpgrade()
    //{
    //    ud.Clear();
    //    ud = CSVReader.Read(upgradeFile);
    //}
    public int SearchScoreData(string key, int sa)
    {
        int baseScore = int.Parse(sd[0][key].ToString());
        int per = int.Parse(sd[sa + 1][key].ToString());
        return baseScore * per;
    }

    //private void ResetUpData()
    //{
    //    tempData = new string[6];
    //    tempData[0] = "SA";
    //    tempData[1] = "SACount";
    //    tempData[2] = "Turn";
    //    tempData[3] = "Item";
    //    tempData[4] = "Range";
    //    tempData[5] = "Score";
    //    newData.Add(tempData);

    //    for (int i = 0; i < ud.Count; ++i)
    //    {
    //        tempData = new string[6];
    //        tempData[0] = ud[i]["SA"].ToString();
    //        tempData[1] = ud[i]["SACount"].ToString();
    //        tempData[2] = ud[i]["Turn"].ToString();
    //        tempData[3] = ud[i]["Item"].ToString();
    //        tempData[4] = ud[i]["Range"].ToString();
    //        tempData[5] = ud[i]["Score"].ToString();
    //        newData.Add(tempData);
    //    }
    //}
    //public void ReplaceUpData(int keyIndex, int count)
    //{
    //    if (isLoad == false)
    //        LoadUpgrade();
    //    tempData = new string[6];
    //    tempData[0] = ud[0]["SA"].ToString();
    //    tempData[1] = ud[0]["SACount"].ToString();
    //    tempData[2] = ud[0]["Turn"].ToString();
    //    tempData[3] = ud[0]["Item"].ToString();
    //    tempData[4] = ud[0]["Range"].ToString();
    //    tempData[5] = ud[0]["Score"].ToString();

    //    tempData[keyIndex] = count.ToString();

    //    newData[1] = tempData;
    //    SaveCSVFile(upgradeFile, newData);
    //}
    //public int SearchUpData(string key)
    //{
    //    if (isLoad == false)
    //        LoadUpgrade();
    //    int data = int.Parse(ud[0][key].ToString());
    //    return data;
    //}
}
