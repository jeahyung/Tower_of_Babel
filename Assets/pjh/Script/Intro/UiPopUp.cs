using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiPopUp : MonoBehaviour, IPointerClickHandler
{
    public event Action<PointerEventData> OnPopupClicked;

    public RectTransform popupRect;
    public float animationDuration = 0.5f;

    private bool isAnimating = false;
    private bool isOpen = false;

    private void Start()
    {
        popupRect.localScale = Vector3.zero;
    }

    public void ShowPopup()
    {
        if (isAnimating || isOpen) return;

        isAnimating = true;
        isOpen = true;

        popupRect.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack)
            .OnComplete(() => isAnimating = false);
    }

    public void HidePopup()
    {
        if (isAnimating || !isOpen) return;

        isAnimating = true;
        isOpen = false;

        popupRect.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                isAnimating = false;
                Debug.Log("Popup hidden");
            });
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isAnimating) return;

        Debug.Log("Click Scanning");
        OnPopupClicked?.Invoke(eventData);

        if (isOpen && !RectTransformUtility.RectangleContainsScreenPoint(popupRect, eventData.position, eventData.pressEventCamera))
        {
            Debug.Log("Click outside detected, hiding popup");
            HidePopup();
        }
    }
}
