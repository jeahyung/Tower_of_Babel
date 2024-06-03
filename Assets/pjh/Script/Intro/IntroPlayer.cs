using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IntroPlayer : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator ani;
    public float speed;
    private bool control = true;
    private AudioSource audio;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        ani = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }


    void Update()
    {
        Moving();
    }

    private void Moving()
    {
       
        if (control == false) { return; }
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        
        rigid.velocity = new Vector3(x * speed, rigid.velocity.y, z * speed);
        Vector3 movement = new Vector3(x, 0, z).normalized * speed;



        if (rigid.velocity.x != 0 || rigid.velocity.z != 0)
        {
            ani.SetBool("isRun", true);

            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            ani.SetBool("isRun", false);
            audio.Stop();
        }

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime * 100);
        }

    }
}
