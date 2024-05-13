using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpreadUi : MonoBehaviour
{
    public RectTransform uiElement; // 세로 길이를 변경할 UI 요소의 RectTransform
    public float startSize;
    public float endSize;
    public float duration;

    void Start()
    {       
        Vector2 originalSize = uiElement.sizeDelta;
        uiElement.sizeDelta = new Vector2(originalSize.x, startSize);

        // DOTween을 사용하여 세로 길이를 변경
        uiElement.DOSizeDelta(new Vector2(originalSize.x, endSize), duration);
    }
}
