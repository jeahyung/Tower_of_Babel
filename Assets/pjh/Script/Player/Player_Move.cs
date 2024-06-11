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
        Debug.Log("회전");
        // 현재 회전 값을 저장하고 싶다면 이곳에서 저장할 수 있습니다.
        Vector3 initialRotation = transform.rotation.eulerAngles;

        // 오브젝트를 (0, 0, 0)으로 회전시킵니다.
        transform.DORotate(Vector3.zero, 1f) // 1초 동안 (0, 0, 0)으로 회전
               .SetEase(Ease.Linear); // 회전 속도는 선형으로 설정
    }
}