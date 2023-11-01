using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingCamera : MonoBehaviour
{
    private Transform target = null;

    //가려진 물체를 확인하는데 쓸 원형 케스트의 반지름
    [SerializeField]
    private float sphereCastRadius = 1f;

    private RaycastHit[] hitBuffer = new RaycastHit[32];

    //술김 오브젝트와 표시될 오브젝트를 저장할 장소
    private List<HideObject> hiddenObject = new List<HideObject>();
    private List<HideObject> previewObj = new List<HideObject>();

    //플레이어 탐색
    private GameObject tPlayer;

    private void LateUpdate()
    {
        RefreshHiddenObject();
    }

    public void RefreshHiddenObject()
    {
        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("Player");
            if (tPlayer != null)
            {
                target = tPlayer.transform;
            }
        }

        //타켓 위치에 대한 레이 계산
        Vector3 toTarget = (target.position - transform.position);
        float targetDistance = toTarget.magnitude;
        Vector3 targetDirection = toTarget / targetDistance;

        //오류 방지 플레이어 뒤의 벽에 충돌 방지
        targetDistance -= sphereCastRadius * 1.1f;

        //리스트 초기화
        hiddenObject.Clear();

        int hitCount = Physics.SphereCastNonAlloc(transform.position, sphereCastRadius, targetDirection, hitBuffer, targetDistance, -1, QueryTriggerInteraction.Ignore);

        //숨길 오브젝트 모으기
        for (int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            var hideable = HideObject.GetRootHideByCollider(hit.collider);

            if (hideable != null)
                hiddenObject.Add(hideable);
        }

        //숨기는 경우
        foreach (var hideable in hiddenObject)
            if (!previewObj.Contains(hideable))
                hideable.SetVisible(false);
        //표시하는 경우
        foreach (var hideable in previewObj)
            if (!hiddenObject.Contains(hideable))
                hideable.SetVisible(true);

        //스왑 목록
        var temp = hiddenObject;
        hiddenObject = previewObj;
        previewObj = temp;

    }


}