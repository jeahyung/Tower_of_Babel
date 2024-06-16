using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTrans : MonoBehaviour
{
    public RectTransform[] uiElements; // ũ�Ⱑ 4�� UI ��� �迭

    private Vector2[] originalPositions; // UI ��ҵ��� ���� ��ġ�� ������ �迭
    public int size = 30;

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

    }

    // uiMove �޼���
    public void UiMove(int n)
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            originalPositions[i] = uiElements[i].anchoredPosition;
        }

        if (n < 0 || n >= uiElements.Length)
        {
            Debug.LogError("Invalid index: " + n);
            return;
        }

        for (int i = 0; i < uiElements.Length; i++)
        {
            if (i != n)
            {
                if(i > n )
                {
                    Rmove(i);
                }
                else
                {
                    Lmove(i);
                }
            }
        }
    }

    // uiMove�� ȣ���� �Ŀ� �ٽ� ���� ��ġ�� �����ϴ� �޼��� (�ɼ�)
    public void ResetUIPositions()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].DOAnchorPos(originalPositions[i], 0.5f).SetEase(Ease.InOutQuad);
        }
    }

    public void Rmove(int i)
    {
        // �̵��� ��ġ ���� (��: ���������� size ���� �̵�)
        Vector2 targetPosition = originalPositions[i] + new Vector2(size, 0);

        // DOTween�� ����Ͽ� �̵�
        uiElements[i].DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.InOutQuad);
    }

    public void Lmove(int i)
    {
        // �̵��� ��ġ ���� (��: �������� size ���� �̵�)
        Vector2 targetPosition = originalPositions[i] + new Vector2(-size, 0);

        // DOTween�� ����Ͽ� �̵�
        uiElements[i].DOAnchorPos(targetPosition, 0.5f).SetEase(Ease.InOutQuad);
    }
}
