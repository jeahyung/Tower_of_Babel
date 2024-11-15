using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherBtnActive : MonoBehaviour
{
    public CanvasGroup uiGroup;

    public static AnotherBtnActive Instance;


    private void Awake()
    {
        Instance = this;
    }


    public void DisableUIInteraction()
    {
        uiGroup.interactable = false;  // UI 요소들과의 상호작용을 비활성화
        uiGroup.blocksRaycasts = true;  // 레이캐스트 차단을 비활성화 (클릭 이벤트가 뒤의 오브젝트로 전달됨) //다시 활성화 시킴. 뒤에 있는 타일이 눌리는 문제 발생함
    }

    public void EnableUIInteraction()
    {
        uiGroup.interactable = true;
        uiGroup.blocksRaycasts = true;
    }
}
