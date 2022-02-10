using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/*
 *                      <   필 dog    >
 * 
 * Action delegate 는 내용 뒤에 Event 를 붙이자(***Event)                   ex) ChangeViewEvent
 * 해당 delegate 이벤트를 Invoke 해주는 함수는 내용 그대로를 쓰자(***)      ex) ChangeView()
 * 해당 이벤트 콜백 함수는 내용앞에 On을 붙이자(On***)                      ex) OnChangeView()
 * */

static public class GameEvents
{
    static public event Action              IsInitializedEvent;     // 초기세팅 완료..

    static public event Action              FinishCIEvent;
    static public event Action              FinishTitleEvent;
    static public event Action              FinishLoadingUIEvent;

    static public event Action              FinishFileLoadEvent;     // Data FileLoad 후 세팅이 끝나고 떨어지는 이벤트..

    // sound event
    static public event Action<float>       ChangeBgmVolumeEvent;
    static public event Action<float>       ChangeEffectVolumeEvent;

    // effect event
    static public event Action              EffectDamageEvent;

    // weapon event
    static public event Action              ChangeWeaponEvent;      // 무기변경..

    static public void IsInitialized()
    {
        Debug.Log("IsInitialized");
        if (IsInitializedEvent != null)
            IsInitializedEvent();
    }

    static public void FinishCI()
    {
        Debug.Log("FinishCI");
        if (FinishCIEvent != null)
            FinishCIEvent();
    }

    static public void FinishTitle()
    {
        Debug.Log("FinishTitle");
        if (FinishTitleEvent != null)
            FinishTitleEvent();
    }

    static public void FinishLoadingUI()
    {
        Debug.Log("FinishLoadingUI");
        if (FinishLoadingUIEvent != null)
            FinishLoadingUIEvent();
    }

    static public void FinishFileLoad()
    {
        Debug.Log("FinishFileLoad");
        if (FinishFileLoadEvent != null)
            FinishFileLoadEvent();
    }

    ////////////////////////////////////////////////////////////////////////////
    // Sound Event
    static public void ChangeBgmVolume(float volume)
    {
        if (ChangeBgmVolumeEvent != null)
            ChangeBgmVolumeEvent(volume);
    }

    static public void ChangeEffectVolume(float volume)
    {
        if (ChangeEffectVolumeEvent != null)
            ChangeEffectVolumeEvent(volume);
    }
    ////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////
    // Effect Event
    static public void EffectDamage()
    {
        Debug.Log("EffectDamage");
        if (EffectDamageEvent != null)
            EffectDamageEvent();
    }
    ////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////
    // Weapon Event
    static public void ChangeWeapon()
    {
        Debug.Log("ChangeWeapon");
        if (ChangeWeaponEvent != null)
            ChangeWeaponEvent();
    }
    ////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////
    // ??? Event
    ////////////////////////////////////////////////////////////////////////////
}