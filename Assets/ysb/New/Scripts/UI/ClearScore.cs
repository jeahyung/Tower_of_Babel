using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class ClearScore : MonoBehaviour
{
    private RectTransform rectT;
    [SerializeField]private TMP_Text score;

    private void Awake()
    {
        rectT = GetComponent<RectTransform>();
        if(score == null) { transform.Find("Text_Score").GetComponent<TMP_Text>(); }
        //score = GetComponent<TMP_Text>();
        //score.enabled = false;
    }

    public void HideText()
    {
        score.text = "";
        score.enabled = false;
        rectT.anchoredPosition = new Vector3(-250, 0, 0);
        transform.DOKill();

        gameObject.SetActive(false);
    }

    public void SetText(string t)
    {
        if(score == null) { score = GetComponent<TMP_Text>(); }
        score.text = "";
        score.enabled = true;
        score.text = t;
    }
    public void SetTarget(float h)
    {
        if(rectT == null) { rectT = GetComponent<RectTransform>(); }
        float height = transform.position.y + h;
        Debug.Log(height);
        transform.DOMoveY(height, 0.5f);
    }    
}
