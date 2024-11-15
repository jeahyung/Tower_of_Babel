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
using UnityEngine.SocialPlatforms.Impl;

public class DB_Manager : MonoBehaviour
{
    public string DBurl = "https://babel-4c2c2-default-rtdb.firebaseio.com/";
    DatabaseReference reference;
    List<Rankdata> rankList = new List<Rankdata>();

    public TMP_InputField input; // 입력한 닉네임
    public TMP_Text[] displayText; // 랭킹판에 보이는 닉네임  
    public TMP_Text RkText; //결과창 순위
    public string[] userNames;

    public TMP_Text[] userInfo;

    public Transform contentTransform; // ScrollView의 Content Transform
                                       // public TextMeshProUGUI textPrefab; // TMP_Text 프리팹
                                       //public GameObject content;
    private int rank = 0;
   // public RankUpdate upRank;
    public ScrollViewUpdater scrolName;
   // public ScrolScore scrolScore;
    public string names;
    public string score;
    public List<string> nameList = new List<string>();
    public List<string> scoreList = new List<string>();
    //랭킹점수
    private ScoreUI scoreUI;
    //public int rankScore;
    private bool isDataLoaded = false;
    private bool isLoading = false;
    void Start()
    {
        //scoreUI = FindObjectOfType<ScoreUI>();
        //isLoading = true;
        ////displayText.fontSize = 36;
        //// Firebase 초기화
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        //{
        //    if (task.Result == DependencyStatus.Available)
        //    {
        //        // Firebase 앱 준비 완료
        //        FirebaseApp app = FirebaseApp.DefaultInstance;
        //        app.Options.DatabaseUrl = new Uri(DBurl);

        //        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //        Debug.Log("Firebase 초기화 완료");
        //       // WriteDB();
               
        //        GetRankingData();
        //        //ReadDB(); // 데이터베이스 읽기 메서드 호출

        //    }
        //    else
        //    {
        //        Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
        //    }
        //});

    
    }

 
    public void UserInfoUpdata()
    {
        
        userInfo[0].text = "SCORE. " + scoreUI.sumScore.text;
        userInfo[1].text = rank.ToString();
        
        
        LoadingData();
    }
    public void IntroUserInfoUpdata()
    {
        LoadingData();
    }

    public void Pasing()
    {
        if (int.TryParse(scoreUI.sumScore.text, out int result))
        {
            CompareNumber(result);
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
        AddRankdata("나난", 29000);
        AddRankdata("창근", 1000);
        //UpLoadingRank();

        // 데이터를 JSON으로 변환하여 Firebase에 저장
        //SaveDataToFirebase();
    }

    

    public void AddRankdata(string name, int rankScore) //이걸로 서버에 등록
    {


        // 새로운 데이터 추가
        rankList.Add(new Rankdata(name, rankScore));
        Debug.Log($"새로운 데이터 추가: {name}, 점수: {rankScore}");
       // string jsonData = JsonUtility.ToJson(rankList[rankList.Count - 1]);

        // 점수에 따라 정렬
        rankList = rankList.OrderByDescending(data => data.rankScore).ToList();
        Debug.Log("정렬 완료");

        // 상위 50개만 유지 (선택적)
        if (rankList.Count > 50)
        {
            rankList = rankList.Take(50).ToList();
        }

        SaveDataToFirebase();

        //reference.Child("RankingBoard").Child($"Rank{rank}").SetRawJsonValueAsync(jsonData);

 //       Debug.Log("랭킹 데이터 업데이트 완료");


        // SaveDataToFirebase();
   

    }


    void SaveDataToFirebase()
    {
        for (int i = 0; i < rankList.Count; i++)
        {
            string jsonData = JsonUtility.ToJson(rankList[i]);
            reference.Child("RankingBoard").Child($"Rank{i+1}").SetRawJsonValueAsync(jsonData);
        }       
             
       Debug.Log("랭킹 데이터 업데이트 완료");
    }

    private void ChageRank(string name, int rankScore)
    {

        rankList.Add(new Rankdata(name, rankScore));
        Debug.Log($"새로운 데이터 추가: {name}, 점수: {rankScore}");
        string jsonData = JsonUtility.ToJson(rankList[rankList.Count - 1]);

        reference.Child("RankingBoard").Child($"Rank{rank}").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    SaveDataToFirebase();
                    Debug.Log($"Rank{rank}에 이미 데이터가 존재합니다.");
                    // 여기서 덮어쓰기 여부를 결정하거나 다른 작업을 수행할 수 있습니다.
                }
                else
                {
                    // 데이터가 존재하지 않음, 새로 쓰기
                    reference.Child("RankingBoard").Child($"Rank{rank}").SetRawJsonValueAsync(jsonData);
                }
            }
        });
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
                rank = scores.IndexOf(compareScore) + 1;
                Debug.Log(rank);
            }
        });
       
    }

    private void GetRankingData()
    {
       
        // "RankingBoard" 데이터베이스 경로 참조
        reference.Child("RankingBoard").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                nameList.Clear();
                scoreList.Clear();
                

                foreach (DataSnapshot data in snapshot.Children)
                {
                    IDictionary<string, object> rankingData = (IDictionary<string, object>)data.Value;

                    string name = rankingData["name"].ToString();

                    // rankScore를 int로 변환
                    if (int.TryParse(rankingData["rankScore"].ToString(), out int rankScore))
                    {
                        // Rankdata 객체 생성 및 rankList에 추가
                        if (isLoading)
                        {
                            rankList.Add(new Rankdata(name, rankScore));       
                        }

                        // 기존의 nameList와 scoreList에도 추가
                        nameList.Add(name);
                        scoreList.Add(rankScore.ToString());

                        Debug.Log($"Added to rankList: Name: {name}, Score: {rankScore}");
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to parse rankScore for {name}");
                    }
                }

                // rankList를 점수 기준으로 내림차순 정렬 (선택적)
                rankList = rankList.OrderByDescending(data => data.rankScore).ToList();

                scrolName.UpdateScrollView(nameList, scoreList);

                Debug.Log($"Loaded {rankList.Count} ranking data items.");

                
            }
            else
            {
                Debug.LogError("Failed to get data: " + task.Exception);
            }
        });

        isLoading = false;
    }

    public void TestRankingData(string[] name)
    {
       // upRank.UpdateName(names);
    }

    public void LoadingData()
    {
        if (!isDataLoaded)
        {
            GetRankingData();
            isDataLoaded = true;
        }
        scrolName.UpdateScrollView(nameList, scoreList);
    }

    public void UpLoadingRank()     // 버튼 등록 함수
    {
        if (string.IsNullOrEmpty(input.text) || string.IsNullOrEmpty(scoreUI.sumScore.text))
        {
            Debug.Log("입력값이 없습니다.");
            return;
        }

        if (int.TryParse(scoreUI.sumScore.text, out int result))
        {
            UpdateOrAddRankData(input.text, result);
            GetRankingData(); // 랭킹 데이터 새로 불러오기
            
        }
        else
        {
            Debug.Log("No Data ~~~~~~~~~~~~");
        }
        
    }

    private void UpdateOrAddRankData(string name, int score)
    {
        var existingData = rankList.FirstOrDefault(data => data.name == name);
        if (existingData != null)
        {
            Debug.Log("중복된 데이터 입력");
            return;
        }
        else
        {
            AddRankdata(name, score);
        }
        //rankList = rankList.OrderByDescending(data => data.rankScore).ToList();
        //SaveDataToFirebase();
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
