using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageClickHandler : MonoBehaviour
{
    private IntroPlayer player;
  
    //  public BtnType currentType;
    private void Awake()
    {
        player = FindObjectOfType<IntroPlayer>();
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
            Debug.Log("�߰�����");
    }
}