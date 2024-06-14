using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class IntroUIManager : MonoBehaviour
{
    private static IntroUIManager instance;
    public GameObject uiObject;
    public CanvasGroup uiCanvasGroup;
    public float fadeDuration = 1.0f; // ���̵� �� �ð�


    public static IntroUIManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<IntroUIManager>();
            if (instance == null)
            {
                GameObject singleton = new GameObject(typeof(UIManager).Name);
                instance = singleton.AddComponent<IntroUIManager>();
            }
            return instance;
        }
    }

    public void ShowUI()
    {
        uiCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true)
            .OnStart(() =>
            {
                // ���̵� ���� ���۵� �� ��ȣ�ۿ� �Ұ����ϵ��� ����
             //   uiCanvasGroup.interactable = false;
              //  uiCanvasGroup.blocksRaycasts = false;
            })
            .OnComplete(() =>
            {
                // ���̵� ���� �Ϸ�Ǿ��� �� ��ȣ�ۿ� �����ϵ��� ����
                uiCanvasGroup.interactable = true;
                uiCanvasGroup.blocksRaycasts = true;
            });
    }

    public void HideUI()
    {
        uiCanvasGroup.DOFade(0f, fadeDuration).SetUpdate(true)
    .OnStart(() =>
    {
        // ���̵� �ƿ��� ���۵� �� ��ȣ�ۿ� �Ұ����ϵ��� ����
        //uiCanvasGroup.interactable = true;
        //uiCanvasGroup.blocksRaycasts = false;
    });
    }

    private void Awake()
    {
        uiCanvasGroup.alpha = 0f;
      //  uiCanvasGroup.interactable = false;
       // uiCanvasGroup.blocksRaycasts = false;
        // �̱��� �ν��Ͻ� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Scene ��ȯ �� �ı� ����
        }
    }
}
