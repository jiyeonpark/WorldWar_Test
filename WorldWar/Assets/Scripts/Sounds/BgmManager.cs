using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : AutoSingleton<BgmManager>
{
    List<Bgm> listBgms = new List<Bgm>();
	// Use this for initialization

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        GetComponentsInChildren<Bgm>(listBgms);
        PlayCommonBgm();
        PlayInGameBgm();

        base.OnStart();
	}

    void OnChangeBgmVolume(float volume)
    {
        SetVolumePercentage(volume);
    }

    void OnDestroy()
    {
    }

    public void PauseAllBgms()
    {
        for (int i = 0; i < listBgms.Count; i++)
        {
            listBgms[i].Pause();
        }
    }

    public void ResumeAllBgms()
    {
        for (int i = 0; i < listBgms.Count; i++)
        {
            listBgms[i].Resume();
        }
    }

	void PlayCommonBgm()
    {
        for(int i=0 ;i< listBgms.Count; i++)
        {
            if(listBgms[i].isCommon)
            {
                listBgms[i].Play();
            }
            else
            {
                listBgms[i].Stop();
            }
        }
    }

    void PlayInGameBgm()
    {
        for (int i = 0; i < listBgms.Count; i++)
        {
            if (listBgms[i].isInGame)
            {
                listBgms[i].Play();
            }
            else if (!listBgms[i].isCommon)
            {
                listBgms[i].Stop();
            }
        }
    }

    void PlayBossBgm()
    {
        for (int i = 0; i < listBgms.Count; i++)
        {
            if (listBgms[i].isBossAppear)
            {
                listBgms[i].Play();
            }
            else if (!listBgms[i].isCommon)
            {
                listBgms[i].Stop();
            }
        }
    }

    void PlayClearBgm()
    {
        for (int i = 0; i < listBgms.Count; i++)
        {
            if (listBgms[i].isClear)
            {
                listBgms[i].Play();
            }
            else if (!listBgms[i].isCommon)
            {
                listBgms[i].Stop();
            }
        }
    }

    public void SetVolumePercentage(float percent = -1f)
    {   // 1f 일경우 원래의 볼륨..
        if (percent == -1f)
            percent = GameManager.Instance.soundbgmValue;
        for (int i = 0; i < listBgms.Count; i++)
        {
            if (listBgms[i])
                listBgms[i].SetVolumePercentage(percent);
        }
    }
}
