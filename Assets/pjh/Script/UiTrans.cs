using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTrans : MonoBehaviour
{
    public RectTransform[] uiElements; // ũ�Ⱑ 4�� UI ��� �迭

    private Vector2 originalScale;
    private Vector2[] originalPositions; // UI ��ҵ��� ���� ��ġ�� ������ �迭
    public int size = 30;
    private bool move = true;

    public float scaleMultiplier = 1.3f;
   
    private void Awake()
    {
        RectTransform firstChildRectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        originalScale = firstChildRectTransform.sizeDelta;
    }

    void Start()
    {
        // �ڽ� ������Ʈ���� �ڵ����� �迭�� ���
        uiElements = GetComponentsInChildren<RectTransform>();
      
        // UIManager �ڽ��� RectTransform�� �����ϰ� �ڽĵ鸸 �迭�� ����
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

        // ���� ��ġ�� ����
        originalPositions = new Vector2[uiElements.Length];
        move = true;
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    uiElements[0].DOSizeDelta(originalScale * scaleMultiplier, 0.3f);
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    uiElements[num].DOSizeDelta(originalScale, 0.3f);
        //}

    }
    // uiMove �޼���
    public void UiMove(int n)
    {
        //����ũ�� ����
        uiElements[n].DOSizeDelta(originalScale * scaleMultiplier, 0.3f);
    }

    // uiMove�� ȣ���� �Ŀ� �ٽ� ���� ��ġ�� �����ϴ� �޼��� (�ɼ�)
    public void ResetUIPositions(int num)
    {
        uiElements[num].DOSizeDelta(originalScale, 0.3f);
 
    }

}
