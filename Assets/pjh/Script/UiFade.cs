using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiFade : MonoBehaviour
{
    private CanvasGroup canvasRenderer;
    public float fadeInDuration = 2.0f;
    public float fadeOutDuration = 1.0f;
    void Start()
    {
        canvasRenderer = GetComponent<CanvasGroup>();

        // �ʱ� ���� ���� (������ �����ϰ�)
        canvasRenderer.alpha = 0f;

        // �ؽ�Ʈ�� ������ ��Ÿ���� ��
        canvasRenderer.DOFade(1f, fadeInDuration);
    }

 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            canvasRenderer.DOFade(1f, fadeInDuration);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            canvasRenderer.DOFade(0f, fadeOutDuration);
        }
       
    }
}
