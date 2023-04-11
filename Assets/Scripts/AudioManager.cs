﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop = false;

    AudioSource source;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        if (AudioManager.instance.muted)
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f)) * 0;
        }
        else
        {
            source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        }
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
        Debug.Log(source.clip.name + "사운드 출력");
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public bool muted = false;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }

        PlaySound("BGM");
        PlaySound("Seagull");
    }

    public void PlaySound(string _name)
    {
        if (muted)
            return;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(_name))
            {
                sounds[i].Play();
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager : Sound not found in Sounds Array");
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(_name))
            {
                sounds[i].Stop();
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager : Sound not found in Sounds Array");
    }

    public void RaiseSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(_name))
            {
                sounds[i].volume = 1f;
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager : Sound not found in Sounds Array");
    }

    public void MuteSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name.Equals(_name))
            {
                sounds[i].volume = 0f;
                return;
            }
        }

        // no sound with _name
        Debug.LogWarning("AudioManager : Sound not found in Sounds Array");
    }
}
