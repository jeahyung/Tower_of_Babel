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
    public TMP_Text[] displayText; // 랭킹판에 보이는 닉네임  
    public TMP_Text RkText; //결과창 순위
    public string[] userNames;

    public Transform contentTransform; // ScrollView의 Content Transform
   // public TextMeshProUGUI textPrefab; // TMP_Text 프리팹
    //public GameObject content;

   // public RankUpdate upRank;
    public ScrollViewUpdater scrolName;
   // public ScrolScore scrolScore;
    public string names;
    public string score;
    public List<string> nameList = new List<string>();
    public List<string> scoreList = new List<string>();

    void Start()
    {
        //displayText.fontSize = 36;
        // Firebase 초기화
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Firebase 앱 준비 완료
                FirebaseApp app = FirebaseApp.DefaultInstance;
                app.Options.DatabaseUrl = new Uri(DBurl);

                reference = FirebaseDatabase.DefaultInstance.RootReference;

                Debug.Log("Firebase 초기화 완료");
                // WriteDB();
               
                GetRankingData();
                //ReadDB(); // 데이터베이스 읽기 메서드 호출

            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            scrolName.UpdateScrollView(nameList, scoreList);
           
        }
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
        //reference.Child("RankingBoard").GetValueAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        DataSnapshot snapshot = task.Result;
        //        List<(string userName, int rankScore)> userList = new List<(string userName, int rankScore)>();

        //        foreach (DataSnapshot data in snapshot.Children)
        //        {
        //            IDictionary rankData = (IDictionary)data.Value;
        //            string userName = rankData["name"].ToString();
        //            int rankScore = Convert.ToInt32(rankData["rankScore"]);
        //            userList.Add((userName, rankScore));
        //        }

        //        // 정렬
        //        userList.Sort((a, b) => b.rankScore.CompareTo(a.rankScore)); // 점수 내림차순 정렬

        //        // 최대 50개의 데이터만 출력
        //        int displayCount = Mathf.Min(userList.Count, 50);
        //        for (int i = 0; i < displayCount; i++)
        //        {
        //            GameObject newObject = Instantiate(textPrefab, contentTransform);
        //            TMP_Text tmpText = newObject.GetComponent<TMP_Text>();
        //            tmpText.fontSize = 36; // 기본 폰트 사이즈 설정
        //            tmpText.text = $"{i + 1}. {userList[i].userName}    {userList[i].rankScore}";
        //        }
        //    }
        //    else
        //    {
        //        Debug.LogError("Firebase 데이터 읽기 실패: " + task.Exception);
        //    }
        //});
        reference.Child("RankingBoard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int i = 0;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary Rankdata = (IDictionary)data.Value;
                    Debug.Log("이름 : " + Rankdata["name"] + " 점수 : " + Rankdata["rankScore"]);
                    string rankInfo = $"{i + 1}. {Rankdata["name"]}                  {Rankdata["rankScore"]}";

                    displayText[i].text = rankInfo;
                    //UpdateTxet(rankInfo);
                    //if (i < 5)
                    //{
                    //    displayText.text = rankInfo;
                    //}
                    //else
                    //{
                    //    Debug.LogWarning($"displayText 배열의 크기()가 너무 작습니다. 데이터를 모두 출력할 수 없습니다.");
                    //    break;
                    //}

                    i++; // 다음 인덱스로 이동

                }
            }
        });
        Canvas.ForceUpdateCanvases();
    }

    public void AddRankdata(string name, int rankScore) //이걸로 서버에 등록
    {
        rankList.Add(new Rankdata(name, rankScore));

        // 추가 후 다시 정렬
        rankList = rankList.OrderByDescending(data => data.rankScore).ToList();

        SaveDataToFirebase();
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

                // 비교할 점수를 리스트에 추가하고 정렬
                scores.Add(compareScore);
                scores.Sort((a, b) => b.CompareTo(a)); // 내림차순 정렬

                // compareScore의 순위 계산
                int rank = scores.IndexOf(compareScore) + 1;

                RkText.text = rank.ToString();              

            }
        });
    }

    private void GetRankingData()
    {
       
        // "RankingBoard" 데이터베이스 경로 참조
        reference.Child("RankingBoard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary<string, object> rankingData = (IDictionary<string, object>)data.Value;

                    // 이름과 점수를 디버그 로그로 출력
                    names = rankingData["name"].ToString();
                    score = rankingData["rankScore"].ToString();
                    //rankingData["rankScore"].ToString()
                    nameList.Add(names);
                    scoreList.Add(score);

                    Debug.Log("Added to list: " + names);
                    long rankScore = (long)rankingData["rankScore"];
                    
                    //TestRankingData(name);
                    //GameObject newText = Instantiate(textPrefab, textPrefab.transform);
                    //newText.GetComponent<Text>().text = "Name: " + name + ", Score: " + rankScore;
                   
                    Debug.Log("Name: " + names + ", Score: " + rankScore);
                }
            }
            else
            {
                // 에러 발생 시 출력
                Debug.LogError("Failed to get data: " + task.Exception);
            }

           // TestRankingData(names);
        });       
    }

    public void TestRankingData(string[] name)
    {
       // upRank.UpdateName(names);
    }

    public void LoadingData()
    {
        scrolName.UpdateScrollView(nameList, scoreList);
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
