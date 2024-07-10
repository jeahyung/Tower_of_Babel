using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using Firebase.Extensions;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DB_Manager : MonoBehaviour
{
    public string DBurl = "https://babel-4c2c2-default-rtdb.firebaseio.com/";
    DatabaseReference reference;
    List<Rankdata> rankList = new List<Rankdata>();

    public TMP_InputField input; // �Է��� �г���
    public TMP_Text displayText; // ��ŷ�ǿ� ���̴� �г���  
    public TMP_Text RkText; //���â ����

    void Start()
    {
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase �� �غ� �Ϸ�
                FirebaseApp app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new Uri(DBurl);

                reference = FirebaseDatabase.DefaultInstance.RootReference;

      
            
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
        
    }

    public void WriteDB()
    {
        if (reference == null)
        {
            Debug.LogError("Firebase database reference is not initialized.");
            return;
        }

        // ������ �߰� ����
        AddRankdata("����", 22000);
        AddRankdata("����", 26000);
        AddRankdata("�ϼ�", 24000);

        // �����͸� JSON���� ��ȯ�Ͽ� Firebase�� ����
        SaveDataToFirebase();
    }

    public void ReadDB()
    {
        if (reference == null)
        {
            Debug.LogError("Firebase database reference is not initialized.");
            return;
        }

        reference.Child("RankingBoard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary Rankdata = (IDictionary)data.Value;
                    Debug.Log("�̸� : " + Rankdata["name"] + " ���� : " + Rankdata["rankScore"]);
                }
            }
        });
    }

    public void AddRankdata(string name, int rankScore)
    {
        rankList.Add(new Rankdata(name, rankScore));

        // �߰� �� �ٽ� ����
        rankList = rankList.OrderByDescending(data => data.rankScore).ToList();
    }

    void SaveDataToFirebase()
    {
        int index = 1; // ������ ������ ��Ÿ���� ���� �ε���
        foreach (var data in rankList)
        {
            string jsonData = JsonUtility.ToJson(data);
            reference.Child("RankingBoard").Child("Data" + index).SetRawJsonValueAsync(jsonData);
            index++;
        }
    }
    public void UpdateTxet()
    {
        displayText.text = input.text;
    }

    public void CompareNumber(int compareScore)
    {
        if (reference == null)
        {
            Debug.LogError("Firebase database reference is not initialized.");
            return;
        }

        reference.Child("RankingBoard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<int> scores = new List<int>();

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary Rankdata = (IDictionary)data.Value;
                    int rankScore = Convert.ToInt32(Rankdata["rankScore"]);
                    scores.Add(rankScore);
                }

                // ���� ������ ����Ʈ�� �߰��ϰ� ����
                scores.Add(compareScore);
                scores.Sort((a, b) => b.CompareTo(a)); // �������� ����

                // compareScore�� ���� ���
                int rank = scores.IndexOf(compareScore) + 1;
                RkText.text = rank.ToString();              

            }
        });
    }
}

[Serializable]
public class Rankdata
{
    public string name;
    public int rankScore;

    public Rankdata(string Name, int RankScore)
    {
        name = Name;
        rankScore = RankScore;
    }
}
