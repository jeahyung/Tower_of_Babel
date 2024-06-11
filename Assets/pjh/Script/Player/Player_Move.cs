using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Player_Move : MonoBehaviour
{
    private Rigidbody rigid;
    public Animator ani;
    private PlayerMovement player;
    private bool control = true;
    public bool move = true;
    private AudioSource audio;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        ani = GetComponentInChildren<Animator>();
        move = true;
    }

   
    void Update()
    {
        Moving();
    }

    private void Moving()
    {
        control = player.isControl;
        if (control == false) { return; }
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
        if (other.CompareTag("Start"))
        {
            move = true;

        }
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
}