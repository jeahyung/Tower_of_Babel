using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator ani;
    private PlayerMovement player;
    private bool control = true;
    private AudioSource audio;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
        ani = GetComponentInChildren<Animator>();
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
            ani.SetBool("isRun", true);
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
        //if (rigid.velocity.x != 0 || rigid.velocity.z != 0)
        //{
        //    if (!AudioManager.instance.CheckAudio(AudioManager.Sfx.Player_Walk))
        //    {
        //        AudioManager.instance.PlaySfx(AudioManager.Sfx.Player_Walk);
        //    }
        //}
        //else
        //{
        //    AudioManager.instance.StopSfx(AudioManager.Sfx.Player_Walk);
        //}

    }
}