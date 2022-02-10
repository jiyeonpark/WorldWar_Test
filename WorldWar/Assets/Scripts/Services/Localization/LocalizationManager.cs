using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum Language : int
{
    English,
    Korean,
    Japanese,
    ChineseTraditional,
    ChineseSimplified,
    Spanish,

    Max,
}

public class LocalizationManager : MonoBehaviour {
    public static LocalizationManager sInstance;

    public Action OnChangeLocalization;

    private Language _Language;
    private Language _PrevLanguage;
    public Language Language
    {
        get { return _Language; }
        set
        {
            _Language = value;

            if (OnChangeLocalization != null)
                OnChangeLocalization();
        }
    }

    public List<Font> Fonts;
    public List<FontStyle> FontStyles;

    void Awake()
    {
        sInstance = this;

        _PrevLanguage = _Language;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (_Language != _PrevLanguage)
        {
            Language = _Language;
            _PrevLanguage = _Language;
        }
    }
#endif

    public void SetLocalizationKey(Text text, string key)
    {
        SetLocalizationValue(text, GetLocalizationValue(key));
    }

    public void SetLocalizationValue(Text text, string value)
    {
        if (text == null)
            return;

        text.font = GetLocalizationFont();
        text.fontStyle = GetLocalizationFontStyle();
        text.text = value;
    }

    public Font GetLocalizationFont()
    {
        return Fonts[(int)Language];
    }

    public FontStyle GetLocalizationFontStyle()
    {
        return FontStyles[(int)Language];
    }

    public string GetLocalizationValue(string key)
    {
        LanguagesXml xml = (LanguagesXml)XmlManager.sInstance.Find(XMLNAME.Languages);
        if (xml == null)
            return "";

        LanguagesXml.Data data = (LanguagesXml.Data)xml.FindKey(key);
        return data.Values[(int)Language];
    }

    public Sprite GetLocalizationSprite(string key)
    {
        string path = "LocalizationSprites/" + Language + "/" + key;

        Texture2D texture = Resources.Load(path) as Texture2D;
        if (texture == null)
        {   //디폴트 이미지
            path = "LocalizationSprites/English/" + key;
            texture = Resources.Load(path) as Texture2D;

            if (texture == null)
                return null;
        }

        return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
    }

#if UNITY_EDITOR
    public static List<object> GetLocalizationKeys()
    {
        LanguagesXml xml = (LanguagesXml)XmlManager.Find(new LanguagesXml(), XMLNAME.Languages);
        if (xml == null)
            return null;

        return xml.Find();
    }

    public static List<object> GetLocalizationKeys(int Dropdown)
    {
        LanguagesXml xml = (LanguagesXml)XmlManager.Find(new LanguagesXml(), XMLNAME.Languages);
        if (xml == null)
            return null;

        return xml.FindKeys(Dropdown);
    }

    public static object GetLocalizationKeys(string key)
    {
        LanguagesXml xml = (LanguagesXml)XmlManager.Find(new LanguagesXml(), XMLNAME.Languages);
        if (xml == null)
            return null;

        return xml.FindKey(key);
    }

    public static bool GetLocalizationSpriteEditor(string language, string key)
    {
        Texture2D texture = GetLocalizationTexture2D(language, key);
        return (texture != null);
    }

    public static Texture2D GetLocalizationTexture2D(string language, string key)
    {
        string path = "LocalizationSprites/" + language + "/" + key;

        return Resources.Load(path) as Texture2D;
    }
#endif
}
