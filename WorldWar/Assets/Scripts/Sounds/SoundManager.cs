using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
[Serializable]
public class SoundInfo
{
    public List<AudioClip> clips = new List<AudioClip>();
    int index = 0;
    public void Init()
    {
        index = 0;
    }
    public float PlayClip(Vector3 point, float volume = 0.5f)
    {
        if(index >= clips.Count)
        {
            return 0f;
        }
        if (clips[index] == null)
        {
            Debug.Log("Sound error : Can not find file..");
            return 0f;
        }

        //3d사운드 더 가깝게
        //if(PlayerInput.IsCreated())
        //{
        //    Vector3 camToPoint = point - PlayerInput.Instance.CamHead.transform.position;
        //    float maxDis = 5f;
        //    if (camToPoint.magnitude > maxDis)
        //    {
        //        camToPoint = camToPoint.normalized * maxDis;
        //    }
        //    Vector3 rePoint = PlayerInput.Instance.CamHead.transform.position + camToPoint * 0.5f;
        //    point = rePoint;
        //}

        //기본볼륨조절
        //volume *= 0.5f;

        //옵션볼륨조절
        volume *= GameManager.Instance.soundfxValue;

        AudioSource.PlayClipAtPoint(clips[index], point, volume);
        float length = clips[index].length;
        index++;
        if (index == clips.Count)
            index = 0;
        return length;
    }
}

public class SoundManager : AutoSingleton<SoundManager> 
{
    public SoundInfo _AKMShot;
    public SoundInfo _FootStep;
    public SoundInfo _FootStep2;
    public SoundInfo _GrenadePin;

    public SoundInfo _bowPullStart;
    public SoundInfo _bowPull;
    public SoundInfo _bowFire;
    public SoundInfo _arrowHit;

    public SoundInfo _handAxe;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    public void PlayAKMShotSound(Vector3 point, float volume = 0.5f)
    {
        _AKMShot.PlayClip(point, volume);
    }

    public void PlayFootStepSound(Vector3 point, float volume = 0.5f)
    {
        _FootStep.PlayClip(point, volume);
    }

    public void PlayFootStep2Sound(Vector3 point, float volume = 0.5f)
    {
        _FootStep2.PlayClip(point, volume);
    }

    public void PlayGrenadePinSound(Vector3 point, float volume = 0.5f)
    {
        _GrenadePin.PlayClip(point, volume);
    }

    public void PlayBowPullStart(Vector3 point, float volume = 0.5f)
    {
        _bowPullStart.PlayClip(point, volume);
    }

    public void PlayBowPull(Vector3 point, float volume = 0.5f)
    {
        _bowPull.PlayClip(point, volume);
    }

    public void PlayBowFireSound(Vector3 point, float volume = 0.5f)
    {
        _bowFire.PlayClip(point, volume);
    }

    public void PlayArrowHitSound(Vector3 point, float volume = 0.5f)
    {
        _arrowHit.PlayClip(point, volume);
    }

    public void PlayHandAxe(Vector3 point, float volume = 0.5f)
    {
        _handAxe.PlayClip(point, volume);
    }
}
