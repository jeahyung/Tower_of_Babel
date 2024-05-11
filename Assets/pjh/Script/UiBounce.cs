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

        // ��Ҹ� ��ǥ ��ġ�� �̵��ϴ� �ִϸ��̼�
        targetTransform.DOMove(targetPosition, duration)
            .SetEase(Ease.OutBounce); // Ease Out Bounce ��¡ �Լ� ����
    }
}
