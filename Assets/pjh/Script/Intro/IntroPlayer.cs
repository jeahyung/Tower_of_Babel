using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;


public class IntroPlayer : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator ani;
    public float speed;
    private bool control = true;
    private AudioSource audio;
    public Transform target;
    public float duration = 2.0f; // 이동 지속 시간

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public void Moving()
    {
        control = false; // 이동 중에는 추가 입력을 받지 않도록 설정

        // 플레이어 애니메이션 및 오디오 재생 설정
        ani.SetBool("isRun", true);
        if (!audio.isPlaying)
        {
            audio.Play();
        }
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z);

        // 두트윈을 사용하여 타겟 위치까지 이동
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(() => {
            ani.SetBool("isRun", false);
            audio.Stop();
            control = true; // 이동이 완료되면 다시 제어 가능하도록 설정
        });



    }
}
