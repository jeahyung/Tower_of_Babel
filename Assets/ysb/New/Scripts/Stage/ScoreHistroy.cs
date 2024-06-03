using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScoreHistroy : MonoBehaviour
{
    RectTransform rect;
    ScoreUI scoreUI;
    float hideTime = 2f;
    float xPos = 0;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        xPos = -50;//transform.position.x;
    }
    public void Move()
    {
        if (scoreUI == null) scoreUI = GetComponentInParent<ScoreUI>();

        rect.DOAnchorPosX(xPos, 1f).OnComplete(() => StartCoroutine(HideHistory()));
        //transform.DOMoveX(xPos, 1f).OnComplete(() => StartCoroutine(HideHistory()));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator HideHistory()
    {
        yield return new WaitForSeconds(hideTime);
        scoreUI.HideHistory();
        gameObject.SetActive(false);
    }
}
