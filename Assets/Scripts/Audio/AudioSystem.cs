using UnityEngine;
using System;

public class AudioSystem : MonoBehaviour
{

    public Sound[] sounds;

    private static AudioSystem instance;

    private void Start()
    {
        Play("Background");
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        foreach(Sound s in sounds)
        {
            AudioSource sas = s.source = gameObject.AddComponent<AudioSource>();
            sas.clip = s.clip;
            sas.volume = s.volume;
            sas.pitch = s.pitch;
            sas.loop = s.loop;
        }
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null) s.source.Play();
    }
}