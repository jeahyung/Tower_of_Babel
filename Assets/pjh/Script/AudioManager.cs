using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Monster_Change, Monster_Destroy, Monster_Move, Player_Hit, Player_Itemget, Player_Step, Player_Teleport,Player_Walk, Door_Open, Stage_Clear, UI_Click, UI_Hover,Game_Over,Game_Over_Broken }

    void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
      //  sfxPlayers[13].volume = 0.1f;
        PlayBgm(true);
    }
    void Init()
    {
        GameObject bgmObgect = new GameObject("BgmObgect");
        bgmObgect.transform.parent = transform;
        bgmPlayer = bgmObgect.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        GameObject sfxObject = new GameObject("sfxObj");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;
        }

    }

    public bool CheckAudio(Sfx sfx)
    {
        if (sfxPlayers[(int)sfx].isPlaying)
        {
            return true;
        }
        else
        {
            return false;
        }     
    }

    public void StopSfx(Sfx sfx)
    {        
        if (sfxPlayers[(int)sfx].isPlaying)
        {
            sfxPlayers[(int)sfx].Stop();
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if(isPlay)
        {
            bgmPlayer.loop = true;
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }
}
