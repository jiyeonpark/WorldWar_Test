using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioList : AutoSingleton<AudioList> 
{
    public List<AudioSource> audioSourceList = new List<AudioSource>();

    private int audioCount = 20;

    protected override void OnAwake()
    {
    }

    protected override void OnStart()
    {
    }

    AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSourceList.Count; i++)
        {
            if (!audioSourceList[i].isPlaying)
                return audioSourceList[i];
        }

        if (audioSourceList.Count < audioCount)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSourceList.Add(source);
            return source;
        }

        audioSourceList[0].Stop();
        return audioSourceList[0];
    }

    public bool PlaySoundClip(AudioClip clip, float fVolume=0.5f, bool bloop=false, bool world = false)
    {
        if (clip == null) return false;
        AudioSource source = GetAudioSource();
        if (source == null)
            return false;

        if (world) source.spatialBlend = 1f;
        else source.spatialBlend = 0f;
        source.clip = clip;
        if (GameManager.IsCreated())
            fVolume *= GameManager.Instance.soundfxValue;
        source.volume = fVolume;
        source.loop = bloop;
        source.Play();
        return true;
    }
}
