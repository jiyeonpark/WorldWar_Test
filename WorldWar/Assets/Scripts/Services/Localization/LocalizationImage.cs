using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LocalizationImage : Localization<Image>
{
    public List<Sprite> Sprites;

    protected override void OnChangeLocalization()
    {
        Sprite sprite = GetSprite((int)LocalizationManager.sInstance.Language);
        if(sprite)
            Value.sprite = sprite;

        //CurvedUI 갱신이 안되서 직접 호출 해줘야함
        CurvedUIVertexEffect curvedUI = GetComponent<CurvedUIVertexEffect>();
        if (curvedUI)
            curvedUI.SetDirty();
    }

    private Sprite GetSprite(int index)
    {
        return Sprites[index];
    }
}
