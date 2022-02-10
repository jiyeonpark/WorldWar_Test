using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocalizationLoadImage : Localization<Image>
{
    protected override void OnChangeLocalization()
    {
        string spritename = Value.sprite.name;
        Sprite sprite = LocalizationManager.sInstance.GetLocalizationSprite(spritename);
        if(sprite)
        {
            Value.sprite = sprite;
            Value.sprite.name = spritename;
        }

        //CurvedUI 갱신이 안되서 직접 호출 해줘야함
        CurvedUIVertexEffect curvedUI = GetComponent<CurvedUIVertexEffect>();
        if(curvedUI)
            curvedUI.SetDirty();
    }
}
