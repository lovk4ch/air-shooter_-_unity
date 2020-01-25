using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public AudioClip clip;

    public string name;

    [Range(0f, 1f)]
    public float spatialBlend = 1;
    [Range(0f, 1f)]
    public float volume = 1;
    [Range(.1f, 3f)]
    public float pitch = 1;
    public bool loop;
    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;

    public Sound(AudioSource source, AudioClip clip, float spatialBlend, float volume, float pitch, bool loop, bool playOnAwake)
    {
        this.source = source;
        this.source.clip = clip;

        this.source.spatialBlend = spatialBlend;
        this.source.volume = volume;
        this.source.pitch = pitch;
        this.source.loop = loop;
        this.source.playOnAwake = playOnAwake;
    }
}

public class AudioManager : Manager<AudioManager>
{
    public Sound[] sounds;

    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.spatialBlend = s.spatialBlend;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    private Sound FindSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        return s;
    }

    public void Stop()
    {
        foreach (Sound s in sounds)
            s.source.Stop();
    }

    public float Play(string name)
    {
        Sound s = FindSound(name);
        if (s != null)
        {
            s.source.Play();
            return s.clip.length;
        }
        return -1;
    }
}