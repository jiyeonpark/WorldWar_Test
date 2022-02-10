using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


static public class SystemEvents
{
    static public event Action<bool> PreferenceSaveEvent;
    static public event Action<bool> BeforePreferenceSaveEvent;
    static public event Action ScheduleNotificationsEvent;
    static public event Action<string> OnPurchaseFailedEvent;
    static public event Action<string> OnPurchaseCancelledEvent;
    static public event Action<KeyCode> KeyPressedEvent;
    static public event Action AfterLoginEvent;

    static public void OnAfterLoginEvent()
    {
        if (AfterLoginEvent != null)
        {
            AfterLoginEvent();
        }
    }

    static public void OnPreferenceSave(bool pause)
    {
        if (PreferenceSaveEvent != null)
        {
            PreferenceSaveEvent(pause);
        }
    }

    static public void OnBeforePreferenceSave(bool pause)
    {
        if (BeforePreferenceSaveEvent != null)
        {
            BeforePreferenceSaveEvent(pause);
        }
    }

    static public void OnScheduleNotifications()
    {
        if (ScheduleNotificationsEvent != null)
        {
            ScheduleNotificationsEvent();
        }
    }

    //static public void OnPurchaseSuccessful(IAP p)
    //{
    //    if (OnPurchaseSuccessfulEvent != null)
    //    {
    //        OnPurchaseSuccessfulEvent(p);
    //    }
    //}

    static public void OnPurchaseFailed(string error)
    {
        if (OnPurchaseFailedEvent != null)
        {
            OnPurchaseFailedEvent(error);
        }
    }

    static public void OnPurchaseCancelled(string error)
    {
        if (OnPurchaseCancelledEvent != null)
        {
            OnPurchaseCancelledEvent(error);
        }
    }
    
    static public void OnKeyPressed(KeyCode keyCode)
    {
        if (KeyPressedEvent != null)
        {
            KeyPressedEvent(keyCode);
        }
    }
}

