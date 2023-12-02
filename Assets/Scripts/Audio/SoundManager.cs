using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SoundManager : SingletonClass<SoundManager>
{
    #region Declaration & Definition
    public float masterVolumeBGM;
    public float masterVolumeUI;
    public float masterVolumeSkill;

    [SerializeField] AudioClip[] BGMClip;
    [SerializeField] AudioClip[] uiAudioClip;
    [SerializeField] AudioClip[] SkillAudioClip;

    [Header("Audio Sources")]
    [SerializeField] public AudioSource bgmPlayer;
    [SerializeField] AudioSource UIPlayer;
    [SerializeField] AudioSource skillPlayer;

    public Dictionary<string, AudioClip> bgmAudioClipDic;
    public Dictionary<string, AudioClip> uiAudioClipDic;
    public Dictionary<string, AudioClip> skillAudioClipDic;
    #endregion

    #region Unity Default Method1
    private new void Awake()
    {
        AwakeAfter();
        SetUpBGM("city_07");
    }
    #endregion

    private void Start()
    {
        SetUpBGM("city_07");
    }


    #region Method
    public void SetUpBGM(string soundName)
    {
        // Exception Handling 01 - If BGMClip Doesn't Exist
        if (BGMClip == null)
            return;

        bgmPlayer.clip = bgmAudioClipDic[soundName];
        bgmPlayer.volume = masterVolumeBGM;
        bgmPlayer.loop = true;
        //bgmPlayer.playOnAwake = true;
        bgmPlayer.Play();
    }

    void AwakeAfter()
    {
        // Exception Handling 01 - If AudioClip Doesn't Exist
        if (null != uiAudioClipDic)
            return;

        if (null != skillAudioClipDic)
            return;

        if (null != bgmAudioClipDic)
            return;

        uiAudioClipDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip item in uiAudioClip)
            uiAudioClipDic.Add(item.name, item);

        skillAudioClipDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip item in SkillAudioClip)
            skillAudioClipDic.Add(item.name, item);

        bgmAudioClipDic = new Dictionary<string, AudioClip>();
        foreach (AudioClip item in BGMClip)
            bgmAudioClipDic.Add(item.name, item);

    }

    public void PlayUISoundOneShot(string soundName)
    {
        // Exception Handling 01 - If AudioClip Doesn't Exist
        if (null == uiAudioClipDic)
            AwakeAfter();

        // Exception Handling 02 - If Someone Input Wrong AudioClip Name
        if (uiAudioClipDic.ContainsKey(soundName) == false)
        {
            Debug.Log(soundName + "is not Contained");
            return;
        }

        float soundVolume = 1.0f;
        UIPlayer.PlayOneShot(uiAudioClipDic[soundName], soundVolume * masterVolumeUI);
    }

    public void PlaySkillSoundOneShot(string soundName)
    {
        // Exception Handling 01 - If AudioClip Doesn't Exist
        if (null == skillAudioClipDic)
            AwakeAfter();

        // Exception Handling 02 - If Someone Input Wrong AudioClip Name
        if (skillAudioClipDic.ContainsKey(soundName) == false)
        {
            Debug.Log(soundName + "is not Contained");
            return;
        }

        float soundVolume = 1.0f;
        skillPlayer.PlayOneShot(skillAudioClipDic[soundName], soundVolume * masterVolumeSkill);
    }

    public void PlaySkillSoundLoop(string soundName)
    {
        // Exception Handling 01 - If AudioClip Doesn't Exist
        if (null == skillAudioClipDic)
            AwakeAfter();

        // Exception Handling 02 - If Someone Input Wrong AudioClip Name
        if (skillAudioClipDic.ContainsKey(soundName) == false)
            Debug.Log(soundName + "is not Contained");

        skillPlayer.clip = uiAudioClipDic[soundName];

        float soundVolume = 1.0f;
        skillPlayer.volume = soundVolume * masterVolumeSkill;
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void StopSkillLoop()
    {
        skillPlayer.Stop();
    }

    public void SetVolumeSFX(float soundVolume)
    {
        masterVolumeSkill = soundVolume;
    }

    public void SetVolumeBGM(float soundVolume)
    {
        masterVolumeBGM = soundVolume;
        bgmPlayer.volume = masterVolumeBGM;
    }

    public AudioClip PlaySFX(string name)
    {
        AudioClip clip = skillAudioClipDic[name];
        return clip;
    }

    #endregion
}