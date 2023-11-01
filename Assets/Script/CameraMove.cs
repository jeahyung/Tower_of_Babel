using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform을 연결
    public Vector3 offset = new Vector3(0f, 25f, -13f); // 카메라와 플레이어 간의 거리와 위치 조정
   // public float rotationSpeed = 2.0f; // 회전 속도

    void Update()
    {
        if (player != null)
        {
            //// 목표 회전을 계산
            //Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);

            //// 부드럽게 회전하도록 Lerp를 사용하여 현재 회전을 목표 회전으로 보간
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 카메라의 위치를 플레이어 위치에 offset을 더한 위치로 설정
            transform.position = player.position + offset;
        }
    }

}
