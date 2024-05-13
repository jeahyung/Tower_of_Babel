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

        // 초기 투명도 설정 (완전히 투명하게)
        canvasRenderer.alpha = 0f;

        // 텍스트를 서서히 나타나게 함
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
