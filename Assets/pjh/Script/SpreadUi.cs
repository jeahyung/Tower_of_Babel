using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpreadUi : MonoBehaviour
{
    public RectTransform uiElement; // ���� ���̸� ������ UI ����� RectTransform
    public float startSize;
    public float endSize;
    public float duration;

    void Start()
    {       
        Vector2 originalSize = uiElement.sizeDelta;
        uiElement.sizeDelta = new Vector2(originalSize.x, startSize);

        // DOTween�� ����Ͽ� ���� ���̸� ����
        uiElement.DOSizeDelta(new Vector2(originalSize.x, endSize), duration);
    }
}
