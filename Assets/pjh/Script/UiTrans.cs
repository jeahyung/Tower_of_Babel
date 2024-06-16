using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTrans : MonoBehaviour
{
    public RectTransform[] uiElements; // 크기가 4인 UI 요소 배열

    private Vector2[] originalPositions; // UI 요소들의 원래 위치를 저장할 배열
    public int size = 30;

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

    }

    // uiMove 메서드
    public void UiMove(int n)
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            originalPositions[i] = uiElements[i].anchoredPosition;
        }

        if (n < 0 || n >= uiElements.Length)
        {
            Debug.LogError("Invalid index: " + n);
            return;
        }

        for (int i = 0; i < uiElements.Length; i++)
        {
            if (i != n)
            {
                if(i > n )
                {
                    Rmove(i);
                }
                else
                {
                    Lmove(i);
                }
            }
        }
    }

    // uiMove를 호출한 후에 다시 원래 위치로 복귀하는 메서드 (옵션)
    public void ResetUIPositions()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].DOAnchorPos(originalPositions[i], 0.5f).SetEase(Ease.InOutQuad);
        }
    }

    public void Rmove(int i)
    {
        // 이동할 위치 설정 (예: 오른쪽으로 size 단위 이동)
        Vector2 targetPosition = originalPositions[i] + new Vector2(size, 0);

        // DOTween을 사용하여 이동
        uiElements[i].DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void Lmove(int i)
    {
        // 이동할 위치 설정 (예: 왼쪽으로 size 단위 이동)
        Vector2 targetPosition = originalPositions[i] + new Vector2(-size, 0);

        // DOTween을 사용하여 이동
        uiElements[i].DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.InOutQuad);
    }
}
