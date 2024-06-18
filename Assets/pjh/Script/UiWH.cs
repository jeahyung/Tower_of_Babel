using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiWH : MonoBehaviour
{
    public RectTransform[] ui;
    private Vector2 target;

   
    public float fadeInDuration = 1.0f;
    

    void Start()
    {
        ResetUIImageSize();
        target = new Vector2(434, 650);
    }

    public void ResetUIImageSize()
    {
        for (int i = 0; i < ui.Length; i++)
        {
            ui[i].sizeDelta = Vector2.zero;            
        }
          
    }

    public void UIImageSize(float duration)
    {
        for(int i = 0; i < ui.Length; i++)
        {
            ui[i].DOSizeDelta(target, duration);       
        }
      
    }
    
}
