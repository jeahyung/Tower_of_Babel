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
    public float fadeDuration = 1.0f; // 페이드 인 시간
    private DB_Manager manager;

    private void Start()
    {
        manager = FindObjectOfType<DB_Manager>();
    }


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
                // 페이드 인이 시작될 때 상호작용 불가능하도록 설정
                //   uiCanvasGroup.interactable = false;
                // uiCanvasGroup.blocksRaycasts = false;
                uiObject.SetActive(true);
          
            })
            .OnComplete(() =>
            {
                // 페이드 인이 완료되었을 때 상호작용 가능하도록 설정
                uiCanvasGroup.interactable = true;
                uiCanvasGroup.blocksRaycasts = true;
            });
        manager.LoadingData();
      //  Debug.Log("데이터 로드 횟수");
    }

    public void HideUI()
    {
        uiCanvasGroup.DOFade(0f, fadeDuration).SetUpdate(true)
    .OnStart(() =>
    {
       // uiObject.SetActive(false);
        // 페이드 아웃이 시작될 때 상호작용 불가능하도록 설정
        //uiCanvasGroup.interactable = true;
        // uiCanvasGroup.blocksRaycasts = false;
    })
     .OnComplete(() =>
     {
         uiObject.SetActive(false);
     });
    }

    private void Awake()
    {
        uiCanvasGroup.alpha = 0f;
      //  uiCanvasGroup.interactable = false;
       // uiCanvasGroup.blocksRaycasts = false;
        // 싱글톤 인스턴스 설정
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Scene 전환 시 파괴 방지
        }
    }
}
