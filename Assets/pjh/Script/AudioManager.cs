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

    public enum Sfx { }

    void Awake()
    {
        instance = this;
        Init();
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

        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = sfxVolume;           
        }

    }
    public void PlaySfx(Sfx sfx)
    {
        for(int i = 0; i< sfxPlayers.Length;i++)
        {
            int loopIndex = (i +channelIndex)%sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)            
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    void Update()
    {
        
    }
}
