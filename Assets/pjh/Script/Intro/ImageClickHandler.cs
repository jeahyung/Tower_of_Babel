using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageClickHandler : MonoBehaviour
{
    private IntroPlayer player;
    
    //  public BtnType currentType;
    private void Awake()
    {
        InitializePlayer();
    }
    private void Start()
    {

       
    }

    public void OnImageClick()
    {  
        IntroUIManager ui = IntroUIManager.Instance;
        if (player != null)
        {
            player.Moving();
            ui.HideUI();
        }
        else
        {
            Debug.Log("추가못함");
            InitializePlayer();
            player.Moving();
            ui.HideUI();
        }
           
    }

    private void InitializePlayer()
    {
        player = FindObjectOfType<IntroPlayer>();
        if (player == null)
        {
            Debug.LogWarning("missing player");
        }
    }
}