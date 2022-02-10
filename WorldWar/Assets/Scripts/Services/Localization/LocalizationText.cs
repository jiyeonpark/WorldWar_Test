using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LocalizationText : Localization<Text>
{
    public string Key;

    protected override void OnChangeLocalization()
    {
        LocalizationManager.sInstance.SetLocalizationKey(Value, Key);

        //CurvedUI 갱신이 안되서 직접 호출 해줘야함
        CurvedUIVertexEffect curvedUI = GetComponent<CurvedUIVertexEffect>();
        if (curvedUI)
            curvedUI.SetDirty();
    }
}
