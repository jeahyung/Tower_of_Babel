using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : Singleton<UI_Setting>
{
    static float bgmVolume; //bgm
    static float sfxVolume; //effect

    public GameObject Setting;
    public GameObject Option;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public List<UpSlot> slots = new List<UpSlot>();

    private void Awake()
    {
        if (slots.Count <= 0) { slots.AddRange(Setting.transform.GetComponentsInChildren<UpSlot>()); }
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey("BGM") || !PlayerPrefs.HasKey("SFX"))
        {
            bgmVolume = 1;
            sfxVolume = 1;
            PlayerPrefs.SetFloat("BGM", bgmVolume);
            PlayerPrefs.SetFloat("SFX", sfxVolume);
        }
        else
        {
            bgmVolume = PlayerPrefs.GetFloat("BGM");
            sfxVolume = PlayerPrefs.GetFloat("SFX");

            bgmSlider.value = bgmVolume;
            sfxSlider.value = sfxVolume;

            AudioManager.instance.SetBgmVolume(bgmVolume);
            AudioManager.instance.SetSfxVolume(sfxVolume);
        }

        Setting.SetActive(false);
        Option.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Option.activeSelf == true) { OpenOption(); return; }
            OpenSetting();
        }
    }


    public void AddUpgrade(Upgrade up)
    {
        if (slots.Count <= 0) { slots.AddRange(Setting.transform.GetComponentsInChildren<UpSlot>()); }
        foreach (var s in slots)
        {
            s.gameObject.SetActive(true);
            if (s.AddUpgrade(up)) { break; }
        }
    }

    public void AddUpgradeAll(List<Upgrade> up)
    {
        if(slots.Count <= 0) { slots.AddRange(Setting.transform.GetComponentsInChildren<UpSlot>()); }
        int i = 0;
        foreach(var u in up)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].AddUpgrade(u);
            i++;
        }
    }
    public void OpenSetting()
    {
        if(Setting.activeSelf == false)
        {
            Setting.SetActive(true);
        }
        else
        {
            Setting.SetActive(false);
        }
    }
    public void OpenOption()
    {
        if(Option.activeSelf == false)
        {
            Option.SetActive(true);
        }
        else
        {
            Option.SetActive(false);
        }
    }

    public void SetBgmVolume()
    {
        bgmVolume = bgmSlider.value;
        AudioManager.instance.SetBgmVolume(bgmVolume);

        PlayerPrefs.SetFloat("BGM", bgmVolume);
    }
    public void SetSfxVolume()
    {
        sfxVolume = sfxSlider.value;
        AudioManager.instance.SetSfxVolume(sfxVolume);

        PlayerPrefs.SetFloat("SFX", sfxVolume);
    }

    public void BackTitle()
    {
        StageManager.instance.BackTile();
    }

    public void Resume()
    {
        OpenSetting();
    }

}
