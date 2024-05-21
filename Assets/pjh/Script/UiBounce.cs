using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class UiBounce : MonoBehaviour
{
    public Transform targetTransform;
    public Transform startPosition;

    private Vector3 targetPosition;
    public float duration = 1.0f;

    void Start()
    {
        targetPosition = transform.position;
        
        targetTransform.position = startPosition.position;

        // 요소를 목표 위치로 이동하는 애니메이션
        targetTransform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutBounce); // Ease Out Bounce 이징 함수 적용
    }
}
