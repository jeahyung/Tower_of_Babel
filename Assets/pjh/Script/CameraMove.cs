using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform�� ����
    public Vector3 offset = new Vector3(0f, 25f, -13f); // ī�޶�� �÷��̾� ���� �Ÿ��� ��ġ ����
   // public float rotationSpeed = 2.0f; // ȸ�� �ӵ�

    void Update()
    {
        if (player != null)
        {
            //// ��ǥ ȸ���� ���
            //Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);

            //// �ε巴�� ȸ���ϵ��� Lerp�� ����Ͽ� ���� ȸ���� ��ǥ ȸ������ ����
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ī�޶��� ��ġ�� �÷��̾� ��ġ�� offset�� ���� ��ġ�� ����
            transform.position = player.position + offset;
        }
    }

}
