using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    move_none = 0,
    move_point,         // point 순간이동
    move_warp,          // point 빠르게이동
    move_controller,    // 패드, 조이스틱으로 위치이동
    move_slow,          // 순간 슬로우상태
    move_swing,         // 컨트롤러 앞뒤흔들어서 이동
}


public class MoveControl : MonoBehaviour 
{
    public MoveType type = MoveType.move_none;
    public string movelayer = "Default";
    public bool isMove = false;
    public float effectvalue = 0.35f;
    public float effecttime = 0.1f;

	void Start () 
    {
        if (StoreManager.sInstance.device == DeviceType.Normal)
            return;

        OnStart();
	}
	
	void Update () 
    {
        if (PlayerInput.Instance.CamRig == null)
            return;

        OnUpdate();
	}

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }

    protected IEnumerator EffectFadeIn(float time)
    {
        if (CamChoice.Instance.frostEffect.FrostAmount != 0f)
            yield break;
        CamChoice.Instance.frostEffect.FrostAmount = 0f;

        float alltime = 0;
        while (alltime < time)
        {
            alltime += Time.deltaTime;
            float value = alltime * 1 / time;
            CamChoice.Instance.frostEffect.FrostAmount = Mathf.Lerp(0f, effectvalue, value);
            yield return null;
        }

        //yield return new WaitForSeconds(time);
        //CamChoice.Instance.frostEffect.FrostAmount = 0f;
    }

    protected IEnumerator EffectFadeOut(float time)
    {
        if (CamChoice.Instance.frostEffect.FrostAmount == 0f)
            yield break;
        CamChoice.Instance.frostEffect.FrostAmount = effectvalue;

        yield return new WaitForSeconds(0.5f);

        float alltime = 0;
        while (alltime < time)
        {
            alltime += Time.deltaTime;
            float value = alltime * 1 / time;
            CamChoice.Instance.frostEffect.FrostAmount = Mathf.Lerp(effectvalue, 0f, value);
            yield return null;
        }

        //yield return new WaitForSeconds(time);
        //CamChoice.Instance.frostEffect.FrostAmount = 0f;
    }
}
