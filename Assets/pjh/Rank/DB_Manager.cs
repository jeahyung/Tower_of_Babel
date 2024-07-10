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

    public TMP_InputField input; // 입력한 닉네임
    public TMP_Text displayText; // 랭킹판에 보이는 닉네임  
    public TMP_Text RkText; //결과창 순위

    void Start()
    {
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase 앱 준비 완료
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

        // 데이터 추가 예시
        AddRankdata("동서", 22000);
        AddRankdata("남북", 26000);
        AddRankdata("북서", 24000);

        // 데이터를 JSON으로 변환하여 Firebase에 저장
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
                    Debug.Log("이름 : " + Rankdata["name"] + " 점수 : " + Rankdata["rankScore"]);
                }
            }
        });
    }

    public void AddRankdata(string name, int rankScore)
    {
        rankList.Add(new Rankdata(name, rankScore));

        // 추가 후 다시 정렬
        rankList = rankList.OrderByDescending(data => data.rankScore).ToList();
    }

    void SaveDataToFirebase()
    {
        int index = 1; // 데이터 순서를 나타내기 위한 인덱스
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

                // 비교할 점수를 리스트에 추가하고 정렬
                scores.Add(compareScore);
                scores.Sort((a, b) => b.CompareTo(a)); // 내림차순 정렬

                // compareScore의 순위 계산
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
