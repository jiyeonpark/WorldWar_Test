using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bgm : MonoBehaviour {
    public bool isCommon = false;
    public bool isInGame = false;
    public bool isBossAppear = false;
    public bool isClear = false;
    List<AudioSource> listAudio = new List<AudioSource>();
    float[] listVolume = null;
	// Use this for initialization
	void Awake () {
        GetComponentsInChildren<AudioSource>(listAudio);
        listVolume = new float[listAudio.Count];
        for (int i = 0; i < listVolume.Length; i++)
        {
            if (listAudio[i])
                listVolume[i] = listAudio[i].volume;
        }
	}

	public void Play()
    {
        for(int i=0; i<listAudio.Count; i++)
        {
            if (listAudio[i])
                listAudio[i].Play();
            else
                Debug.Log("Bgm null (Play()) : " + gameObject.name);
        }
    }

    public void Resume()
    {
        for (int i = 0; i < listAudio.Count; i++)
        {
            if (listAudio[i])
                listAudio[i].UnPause();
            else
                Debug.Log("Bgm null (Resume()) : " + gameObject.name);
        }
    }

    public void Pause()
    {
        for (int i = 0; i < listAudio.Count; i++)
        {
            if (listAudio[i])
                listAudio[i].Pause();
            else
                Debug.Log("Bgm null (Pause()) : " + gameObject.name);
        }
    }

    public void Stop()
    {
        for (int i = 0; i < listAudio.Count; i++)
        {
            if(listAudio[i])
                listAudio[i].Stop();
            else
                Debug.Log("Bgm null (Stop()) : " + gameObject.name);
        }
    }

    public void SetVolumePercentage(float percent = 1f)
    {   // 1f 일경우 원래의 볼륨..
        for (int i = 0; i < listAudio.Count; i++)
        {
            if (listAudio[i])
                listAudio[i].volume = listVolume[i] * percent;
        }
    }
}
