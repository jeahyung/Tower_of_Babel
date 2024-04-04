using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidingCamera : MonoBehaviour
{
    private Transform target = null;

    //������ ��ü�� Ȯ���ϴµ� �� ���� �ɽ�Ʈ�� ������
    [SerializeField]
    private float sphereCastRadius = 1f;

    private RaycastHit[] hitBuffer = new RaycastHit[32];

    //���� ������Ʈ�� ǥ�õ� ������Ʈ�� ������ ���
    private List<HideObject> hiddenObject = new List<HideObject>();
    private List<HideObject> previewObj = new List<HideObject>();

    //�÷��̾� Ž��
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

        //Ÿ�� ��ġ�� ���� ���� ���
        Vector3 toTarget = (target.position - transform.position);
        float targetDistance = toTarget.magnitude;
        Vector3 targetDirection = toTarget / targetDistance;

        //���� ���� �÷��̾� ���� ���� �浹 ����
        targetDistance -= sphereCastRadius * 1.1f;

        //����Ʈ �ʱ�ȭ
        hiddenObject.Clear();

        int hitCount = Physics.SphereCastNonAlloc(transform.position, sphereCastRadius, targetDirection, hitBuffer, targetDistance, -1, QueryTriggerInteraction.Ignore);

        //���� ������Ʈ ������
        for (int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            var hideable = HideObject.GetRootHideByCollider(hit.collider);

            if (hideable != null)
                hiddenObject.Add(hideable);
        }

        //����� ���
        foreach (var hideable in hiddenObject)
            if (!previewObj.Contains(hideable))
                hideable.SetVisible(false);
        //ǥ���ϴ� ���
        foreach (var hideable in previewObj)
            if (!hiddenObject.Contains(hideable))
                hideable.SetVisible(true);

        //���� ���
        var temp = hiddenObject;
        hiddenObject = previewObj;
        previewObj = temp;

    }


}