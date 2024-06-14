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
    public float duration = 2.0f; // �̵� ���� �ð�

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    public void Moving()
    {
        control = false; // �̵� �߿��� �߰� �Է��� ���� �ʵ��� ����

        // �÷��̾� �ִϸ��̼� �� ����� ��� ����
        ani.SetBool("isRun", true);
        if (!audio.isPlaying)
        {
            audio.Play();
        }
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z);

        // ��Ʈ���� ����Ͽ� Ÿ�� ��ġ���� �̵�
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(() => {
            ani.SetBool("isRun", false);
            audio.Stop();
            control = true; // �̵��� �Ϸ�Ǹ� �ٽ� ���� �����ϵ��� ����
        });



    }
}
