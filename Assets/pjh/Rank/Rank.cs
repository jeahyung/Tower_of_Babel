using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;


public class Rank : MonoBehaviour
{
    public TMP_Text displayText; //Á¡¼ö

   
    public int score = 0;
    public int a = 0;
  
    void Start()
    {
     //   manager = FindObjectOfType<DB_Manager>();
    }

   
    public void UpdateUserScore(int total)
    {
        score = total;
        displayText.text = total.ToString();
      
    }
    
    
}
