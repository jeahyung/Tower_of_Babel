using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ItemSlotScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //기존 itemUislot위치에 넣기
    
    private Vector2 originalScale;

    private Vector2 borderLineOriginalScale;
    private bool enter = false;
    private bool exit = true;
    
    public RectTransform uiElement;
    public float scaleMultiplier = 2f;
    public GameObject borderLine;
    

    private CanvasGroup canvasRenderer;
    public float fadeInDuration = 2.0f;
    public float fadeOutDuration = 1.0f;

    void Start()
    {
        canvasRenderer = borderLine.GetComponent<CanvasGroup>();
        canvasRenderer.alpha = 0f;

        originalScale = uiElement.sizeDelta;
        //border_line_originalScale = border_line.sizeDelta;
        borderLineOriginalScale = borderLine.transform.localScale;

        exit = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("마우스 들어옴");
        this.uiElement.DOSizeDelta(originalScale * scaleMultiplier*(-1), 0.3f);

        enter = true; 

        if (borderLine != null)
        {
         //   borderLine.transform.DOScale(borderLineOriginalScale * scaleMultiplier, 0.3f);
        }
    
        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 클릭이후 고정 필요
        {  
            if (enter)
            {
                //들어와서 클릭
                canvasRenderer.DOFade(1f, 0);
                exit = false;
            }
            else
            {
                //나가서 클릭한 경우
                exit = true;
                OriginUiScaleSize();
            }
        }
      
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("마우스 나옴");
        enter = false;
        //Debug.Log(enter);
        //Debug.Log(exit);
        if (exit)
        {
            OriginUiScaleSize();
        }

        
    }

    private void OriginUiScaleSize()
    {
        this.uiElement.DOSizeDelta(originalScale, 0.3f);


        if (borderLine != null)
        {
            borderLine.transform.DOScale(borderLineOriginalScale, 0.3f);
        }

        if (exit)
        {
            canvasRenderer.DOFade(0f, 0);
        }
    }

}
