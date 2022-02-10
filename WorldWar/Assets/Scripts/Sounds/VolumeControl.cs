using UnityEngine;
using System.Collections;

public class VolumeControl : MonoBehaviour 
{
    public float StartDelay = 0f;
    public float fadeInTime = 0f;
    public float fadeOutTime = 0f;

    private AudioSource source = null;
    private float maxVolume = 0f;

    float _originVolume = 0f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        _originVolume = source.volume;
        GameEvents.ChangeBgmVolumeEvent += OnChangeBgmVolume;
    }

    void OnChangeBgmVolume(float volume)
    {
        source.volume = _originVolume * volume;
    }

    void OnDestroy()
    {
        GameEvents.ChangeBgmVolumeEvent -= OnChangeBgmVolume;
    }

	void Start () 
    {
        PlaySetting();
	}

    //void OnEnable()
    //{
    //    PlaySetting();
    //}

    void PlaySetting()
    {
        if (source == null) return;
        maxVolume = source.volume;
        StopCoroutine("SoundPlay");
        StartCoroutine("SoundPlay");
    }

    IEnumerator SoundPlay()
    {
        source.volume = 0f;
        yield return new WaitForSeconds(StartDelay);

        if (fadeInTime > 0f)
        {
            source.volume = 0f;
            float curtime = fadeInTime;
            while (curtime > 0f)
            {
                curtime -= Time.deltaTime;
                source.volume += Time.deltaTime * maxVolume * (1 / fadeInTime);
                yield return null;
            }
        }
        source.volume = maxVolume;

        if (source.loop)
            yield break;

        if (fadeOutTime > 0f)
        {
            float wait = source.clip.length - fadeInTime - fadeOutTime;
            yield return new WaitForSeconds(wait);

            float curtime = fadeOutTime;
            while (curtime > 0f)
            {
                curtime -= Time.deltaTime;
                source.volume -= Time.deltaTime * maxVolume * (1 / fadeOutTime);
                yield return null;
            }
            source.volume = 0f;
        }
    }
}
