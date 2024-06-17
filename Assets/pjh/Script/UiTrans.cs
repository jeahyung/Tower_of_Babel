using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTrans : MonoBehaviour
{
    public RectTransform[] uiElements; // 크기가 4인 UI 요소 배열

    private Vector2 originalScale;
    private Vector2[] originalPositions; // UI 요소들의 원래 위치를 저장할 배열
    public int size = 30;
    private bool move = true;

    public float scaleMultiplier = 1.3f;
   
    private void Awake()
    {
        RectTransform firstChildRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        originalScale = firstChildRectTransform.sizeDelta;
    }

    void Start()
    {
        // 자식 오브젝트들을 자동으로 배열에 등록
        uiElements = GetComponentsInChildren<RectTransform>();
      
        // UIManager 자신의 RectTransform을 제외하고 자식들만 배열에 포함
        List<RectTransform> uiElementsList = new List<RectTransform>();
        foreach (Transform child in transform)
        {
            RectTransform rectTransform = child.GetComponent<RectTransform>();
            if (rectTransform != null && rectTransform != this.GetComponent<RectTransform>())
            {
                uiElementsList.Add(rectTransform);
            }
        }
        uiElements = uiElementsList.ToArray();

        // 원래 위치를 저장
        originalPositions = new Vector2[uiElements.Length];
        move = true;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    uiElements[0].DOSizeDelta(originalScale * scaleMultiplier, 0.3f);
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    uiElements[num].DOSizeDelta(originalScale, 0.3f);
        //}

    }
    // uiMove 메서드
    public void UiMove(int n)
    {
        //슬롯크기 증가
        uiElements[n].DOSizeDelta(originalScale * scaleMultiplier, 0.3f);
    }

    // uiMove를 호출한 후에 다시 원래 위치로 복귀하는 메서드 (옵션)
    public void ResetUIPositions(int num)
    {
        uiElements[num].DOSizeDelta(originalScale, 0.3f);
 
    }

}
