using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Player_Move : MonoBehaviour
{
    private Rigidbody rigid;
    public Animator ani;
    private PlayerMovement player;
    private bool control;
    public bool move = true;
    private AudioSource audio;
    private float duration = 2f;
    public Transform target;
    private bool isMoving = true;
    Vector3 targetPosition;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        ani = GetComponentInChildren<Animator>();
        move = true;
        targetPosition = new Vector3(transform.position.x, transform.position.y, target.position.z);
        control = player.isControl;
    }

    private void OnEnable()
    {
        isMoving = true;
    }

    void Update()
    {
        Moving();

    }

    public void CheckAndMoving()
    {
        if (transform.position.z < targetPosition.z)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            StartCoroutine(MoveToTarget());
        }

    }

    private void Moving()
    {
        control = player.isControl;
        if (control == false) { return; }
        if (isMoving == true) { return; }
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        rigid.velocity = new Vector3(x * 5, rigid.velocity.y, z * 5);
        Vector3 movement = new Vector3(x, 0, z).normalized * 5;


        if (rigid.velocity.x != 0 || rigid.velocity.z != 0)
        {
            if (move)
            {
                ani.SetBool("isRun", true);
            }


            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
            ani.SetBool("isRun", false);
        }


        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 5 * Time.deltaTime * 100);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{       
    //    if (other.CompareTag("start"))
    //    {
    //        RotateObject();
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Start"))
        //{
        //    move = true;

        //}
    }
    public void StopAni()
    {
        move = false;
        ani.SetBool("isRun", false);
    }
    public void StartAni()
    {
        move = true;
    }
    public void RotateObject()
    {
        Debug.Log("ȸ��");
        // ���� ȸ�� ���� �����ϰ� �ʹٸ� �̰����� ������ �� �ֽ��ϴ�.
        Vector3 initialRotation = transform.rotation.eulerAngles;

        // ������Ʈ�� (0, 0, 0)���� ȸ����ŵ�ϴ�.
        transform.DORotate(Vector3.zero, 1f) // 1�� ���� (0, 0, 0)���� ȸ��
               .SetEase(Ease.Linear); // ȸ�� �ӵ��� �������� ����
    }

    private IEnumerator MoveToTarget()
    {
        isMoving = true;
        ani.SetBool("isRun", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 5f * Time.deltaTime);
            yield return null;
        }

        // ��Ȯ�� ��ġ�� �̵��� �ʿ䰡 ����
        //transform.position = targetPosition;

        audio.Stop();
        ani.SetBool("isRun", false);

        isMoving = false;
    }
}