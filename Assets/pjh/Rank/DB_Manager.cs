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
    public TMP_Text[] displayText; // ��ŷ�ǿ� ���̴� �г���  
    public TMP_Text RkText; //���â ����
    public string[] userNames;

    public Transform contentTransform; // ScrollView�� Content Transform
    public GameObject textPrefab; // TMP_Text ������

    void Start()
    {
        //displayText.fontSize = 36;
        // Firebase �ʱ�ȭ
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase �� �غ� �Ϸ�
                FirebaseApp app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new Uri(DBurl);

                reference = FirebaseDatabase.DefaultInstance.RootReference;

                Debug.Log("Firebase �ʱ�ȭ �Ϸ�");
               // WriteDB();
                ReadDB(); // �����ͺ��̽� �б� �޼��� ȣ��

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
        reference.Child("RankingBoard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                List<(string userName, int rankScore)> userList = new List<(string userName, int rankScore)>();

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary rankData = (IDictionary)data.Value;
                    string userName = rankData["name"].ToString();
                    int rankScore = Convert.ToInt32(rankData["rankScore"]);
                    userList.Add((userName, rankScore));
                }

                // ����
                userList.Sort((a, b) => b.rankScore.CompareTo(a.rankScore)); // ���� �������� ����

                // �ִ� 50���� �����͸� ���
                int displayCount = Mathf.Min(userList.Count, 50);
                for (int i = 0; i < displayCount; i++)
                {
                    GameObject newObject = Instantiate(textPrefab, contentTransform);
                    TMP_Text tmpText = newObject.GetComponent<TMP_Text>();
                    tmpText.fontSize = 36; // �⺻ ��Ʈ ������ ����
                    tmpText.text = $"{i + 1}. {userList[i].userName}    {userList[i].rankScore}";
                }
            }
            else
            {
                Debug.LogError("Firebase ������ �б� ����: " + task.Exception);
            }
        });
        //reference.Child("RankingBoard").GetValueAsync().ContinueWith(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;
        //        int i = 0;
        //        foreach (DataSnapshot data in snapshot.Children)
        //        {
        //            IDictionary Rankdata = (IDictionary)data.Value;
        //            Debug.Log("�̸� : " + Rankdata["name"] + " ���� : " + Rankdata["rankScore"]);
        //            string rankInfo = $"{i + 1}. {Rankdata["name"]}                  {Rankdata["rankScore"]}";

        //            displayText[i].text = rankInfo;
        //            //UpdateTxet(rankInfo);
        //            //if (i < 5)
        //            //{
        //            //    displayText.text = rankInfo;
        //            //}
        //            //else
        //            //{
        //            //    Debug.LogWarning($"displayText �迭�� ũ��()�� �ʹ� �۽��ϴ�. �����͸� ��� ����� �� �����ϴ�.");
        //            //    break;
        //            //}

        //            i++; // ���� �ε����� �̵�

        //        }
        //    }
        //});
        //Canvas.ForceUpdateCanvases();
    }

    public void AddRankdata(string name, int rankScore) //�̰ɷ� ������ ���
    {
        rankList.Add(new Rankdata(name, rankScore));

        // �߰� �� �ٽ� ����
        rankList = rankList.OrderByDescending(data => data.rankScore).ToList();

        SaveDataToFirebase();
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
    public void UpdateTxet(int i)
    {
        displayText[i].text = input.text;
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
