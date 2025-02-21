using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Sound
{
    public string name;
    public int number;
}


public class SoundManager : MonoBehaviour
{
    #region singleton
    static public SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
            
        else
            Destroy(instance);
    }
    #endregion singleton

    public AudioSource[] audioSourceEffects;
    public AudioSource[] audioSourceBgm;

    public Sound[] effectSounds;
    public Sound[] bgmSounds;

    public void PlaySE(string _name)
    {
        for (int i = 0; i < effectSounds.Length; i++)
        {
            if (_name == effectSounds[i].name)
            {
                if (!audioSourceEffects[effectSounds[i].number].isPlaying)
                    audioSourceEffects[effectSounds[i].number].Play();
                return;
            }
        }
        Debug.Log(_name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }

    public void StopSE(string _name)
    {
        for (int i = 0; i< effectSounds.Length; i++)
        {
            if (effectSounds[i].name == _name && audioSourceEffects[effectSounds[i].number].isPlaying)
            {
                audioSourceEffects[effectSounds[i].number].Stop();
                return;
            }
        }
        
    }

    public void PlayBgm(string _name)
    {
        for (int i = 0; i < bgmSounds.Length; i++)
        {
            if (_name == bgmSounds[i].name)
            {
                audioSourceBgm[bgmSounds[i].number].Play();
            }
        }
    }

    public void StopAllBgm()
    {
        for (int i = 0; i < audioSourceBgm.Length; i++)
        {
            audioSourceBgm[i].Stop();
        }
    }
}
