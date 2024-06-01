using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    private Rigidbody rigid;

    private PlayerMovement player;
    private bool control = true;
    AudioSource audio;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        player = GetComponent<PlayerMovement>();
        audio = GetComponent<AudioSource>();
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
        if (rigid.velocity.x != 0 || rigid.velocity.z != 0)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
            audio.Stop();
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