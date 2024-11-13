using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroRankOpen : MonoBehaviour,IPointerClickHandler
{
    public RectTransform popupRect;
    public RectTransform rankPanel;
    public float animationDuration = 0.5f;
    public CanvasGroup canvasGroup;
    private ScrollViewUpdater sc;
    [SerializeField]private bool isAnimating = false;
    private bool isOpen = false;
    private DB_Manager db;

    private void Start()
    {
        db = FindObjectOfType<DB_Manager>();
        popupRect.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
        rankPanel.localScale = Vector3.zero;

        sc = FindObjectOfType<ScrollViewUpdater>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) { 
            ShowPopup();
        }
    }
    public void ShowPopup()
    {        
        if (isAnimating || isOpen) return;

        sc.TopViewer();

        isAnimating = true;
        isOpen = true;
        rankPanel.localScale = Vector3.one;
        canvasGroup.DOFade(1f, animationDuration);
        //rankPlanel.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
        popupRect.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack)
            .OnComplete(() => isAnimating = false);

        db.UserInfoUpdata();
       // db.IntroUserInfoUpdata();
    }

    public void IntroShowPop()
    {
        if (isAnimating || isOpen) return;

        sc.TopViewer();

        isAnimating = true;
        isOpen = true;
        rankPanel.localScale = Vector3.one;
        canvasGroup.DOFade(1f, animationDuration);
        //rankPlanel.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);
        popupRect.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack)
            .OnComplete(() => isAnimating = false);

      
        db.IntroUserInfoUpdata();
    }

    public void HidePopup()
    {
        if (isAnimating || !isOpen) return;

        isAnimating = true;
        isOpen = false;

       // rankPlanel.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack);

        canvasGroup.DOFade(0f, 0.2f);

        popupRect.DOScale(Vector3.zero, animationDuration).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                isAnimating = false;
                Debug.Log("Popup hidden"); // HidePopup가 호출되었음을 로그로 확인
                rankPanel.localScale = Vector3.zero;
            });
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (isAnimating) return;

        Debug.Log("Click Scanning");
        if (isOpen && !RectTransformUtility.RectangleContainsScreenPoint(popupRect, eventData.position, eventData.pressEventCamera))
        {
            Debug.Log("Click outside detected, hiding popup");
            HidePopup();
        }
    }
}