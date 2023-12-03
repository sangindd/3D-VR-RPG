using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSetting : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void SetAudio(string name)
    {
        AudioClip clip;
        SoundManager.Instance.skillAudioClipDic.TryGetValue(name, out clip);
        source.clip = clip;
        source.volume = SoundManager.Instance.masterVolumeSkill;
        source.Play();
    }

    public void SetUIAudio(string name)
    {
        AudioClip clip;
        SoundManager.Instance.uiAudioClipDic.TryGetValue(name, out clip);
        source.clip = clip;
        source.volume = SoundManager.Instance.masterVolumeUI;
        source.Play();
    }
}
