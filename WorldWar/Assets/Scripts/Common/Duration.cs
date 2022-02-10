using UnityEngine;
using System.Collections;

public class Duration
{
    float _timeStart;
    float _timeEnd;

    public Duration(float delay)
    {
        _timeStart = Time.time;
        _timeEnd = _timeStart + delay;
    }

    public float Value01()
    {
        return Mathf.InverseLerp(_timeStart, _timeEnd, Time.time);
    }

    public bool IsDone()
    {
        return Time.time > _timeEnd;
    }
}
