using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroDoor : MonoBehaviour
{
    public float shakeAmount = 0.8f; // 작게 진동할 때의 스케일 크기
    public float duration = 3f;   // 애니메이션 지속 시간
    public GameObject[] door;   //0이 왼쪽문 -> -180 1이 오른쪽문 -> 0
    
   // private float startY;
    private AudioSource audio;
    private Vector3 originalPosition;
    private float initRota;
    private bool open = true;

    private void Start()
    {
        originalPosition = transform.position;
        audio = GetComponent<AudioSource>();
        initRota = door[0].transform.localEulerAngles.z;
        open = true;
    }

    public void MoveDownY()
    {
        // 현재 위치에서 y값을 -10만큼 이동
        audio.Play();
        transform.DOMoveY(originalPosition.y - 5, 3f).SetEase(Ease.Linear);
     //   transform.DOShakePosition(duration, shakeAmount);

    }

    public void MoveUpY()
    {
        transform.DOMoveY(originalPosition.y, 1.5f).SetEase(Ease.Linear);
    }

    public void OpenTheDoor()
    {
        if(open)
        {
            Vector3 rightDoor = new Vector3(door[0].transform.localEulerAngles.x, door[0].transform.localEulerAngles.y, -90);

            door[0].transform.DOLocalRotate(rightDoor, 1f).SetEase(Ease.Linear);

            Vector3 leftDoor = new Vector3(door[1].transform.localEulerAngles.x, door[1].transform.localEulerAngles.y, 90);

            door[1].transform.DOLocalRotate(leftDoor, 1f).SetEase(Ease.Linear);

            audio.Play();

            open = false;
        }

    }
}
