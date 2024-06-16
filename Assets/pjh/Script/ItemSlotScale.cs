using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ItemSlotScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //���� itemUislot��ġ�� �ֱ�
    
    private Vector2 originalScale;

    private Vector2 borderLineOriginalScale;
    private bool enter = false;
    private bool exit = true;
    private bool check = false;
    
    public RectTransform uiElement;
    public float scaleMultiplier = 2f;
    public GameObject borderLine;

    private UiTrans uiT;
    private CanvasGroup canvasRenderer;
    public float fadeInDuration = 2.0f;
    public float fadeOutDuration = 1.0f;
    public int num;

    void Start()
    {
        canvasRenderer = borderLine.GetComponent<CanvasGroup>();
        canvasRenderer.alpha = 0f;
        uiT = FindObjectOfType<UiTrans>();
        originalScale = uiElement.sizeDelta;
        //border_line_originalScale = border_line.sizeDelta;
        borderLineOriginalScale = borderLine.transform.localScale;
        check = false;
        exit = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("���콺 ����");
        this.uiElement.DOSizeDelta(originalScale * scaleMultiplier*(-1), 0.3f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.UI_Hover);
        uiT.UiMove(num);
        enter = true; 

        if (borderLine != null)
        {
         //   borderLine.transform.DOScale(borderLineOriginalScale * scaleMultiplier, 0.3f);
        }
    
        check = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Ŭ������ ���� �ʿ�
        {  
            if (enter)
            {
                //���ͼ� Ŭ��
                canvasRenderer.DOFade(1f, 0);
                exit = false;
            }
            else
            {
                //������ Ŭ���� ���
                exit = true;
                OriginUiScaleSize();
            }
        }
      
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("���콺 ����");
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
        if(check)
        {
            uiT.ResetUIPositions();
        }       

        if (borderLine != null)
        {
            borderLine.transform.DOScale(borderLineOriginalScale, 0.3f);
        }

        if (exit)
        {
            canvasRenderer.DOFade(0f, 0);
        }
        check = false;
    }

}
