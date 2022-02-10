using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Define
{
    //----------------------------------------------------------//
    // 기본값..                                                 //
    //----------------------------------------------------------//

    public struct INVALID
    {
        public const int ZERO = 0;
        public const int INT = -1;
        public const float FLOAT = -1f;
    }

    public struct BUNDLE
    {
        public const string SoundExt = ".ogg";
        public const string SoundWavExt = ".wav";
        public const string ABExt = ".unity3d";
        public const string prefabExt = ".prefab";
        public const string xmlExt = ".xml";
        public const string bytesExt = ".bytes";
        public const string txtExt = ".txt";
        public const string matExt = ".mat";
        public const string PatchXmlName = "AssetBundleInfo.xml";
        public const string ServerConnectXml = "server.xml";
    }
    public const float Fade_Duration = .2f;

    //----------------------------------------------------------//
    // enum..                                                 //
    //----------------------------------------------------------//
    public enum AniState
    {
        state_idle = 0,
    }

    public enum InfoType
    {
        type_Stage = 0,
        type_Graphic,
        type_Controller,
        type_Language,
        type_SoundFX,
        type_SoundBGM,

        type_Max,
    }

    public enum StageIdx
    {
        // storymode
        stage_tutorial = 0,
        stage_oh1,
        stage_oh2,
        stage_oh3,
        stage_we1,
        stage_we2,
        stage_we3,

        stage_max,
    }

    public enum GraphicIdx
    {
        Hight = 0,
        Middle,
        Low,
    }

    public enum ControllerIdx
    {
        Left = 0,
        Right,
    }

    public enum LanguageIdx
    {
        English = 0,
        Korean,
        Japanese,
        ChineseTraditional,
        ChineseSimplified,
        Spanish,
    }

    public enum LevelIdx
    {
        Easy = 0,
        Normal,
        Hard,
    }

    public enum PoolIdx
    {
        // model 0~99
        //model_Power                 = 0,

        // object 100~199
        obj_Bullet                    = 100,         // 총알
        obj_Grenade                   = 101,         // 수류탄
        obj_Arrow                     = 102,         // 화살
        obj_HandAxe                   = 103,         // 손도끼
        obj_Item_AKM                  = 104,         // 무기아이템
        obj_Item_Sniper               = 105,         // 무기아이템
        obj_Item_M1911                = 106,         // 무기아이템
        obj_Item_Grenade              = 107,         // 무기아이템
        obj_Item_Bow                  = 108,         // 무기아이템
        obj_Item_HandAxe              = 109,         // 무기아이템
        obj_Item_Knife                = 110,         // 무기아이템
        obj_Item_Sword                = 111,         // 무기아이템

        // etc 200~299
        decal_Metal                   = 200,         // 총 벽에 decal
        decal_Wood                    = 201,
        decal_Dirt                    = 202,

        // effect 300~
        effect_GunFire                = 300,        // 총 발사 이펙트
        effect_Metal                  = 301,        // 총 벽에 파편
        effect_Wood                   = 302,
        effect_Dirt                   = 303,
        effect_Concrete               = 304,
        effect_Blood                  = 305,
        
    }

    //----------------------------------------------------------//
    // Convert                                                  //
    //----------------------------------------------------------//
    #region Convert
    // Vector 재정의 (new 하지 않기위해)..
    static private Vector3 _wcvector3 = new Vector3();
    static public Vector3 WCVector3(float x, float y, float z)
    {
        _wcvector3.x = x; _wcvector3.y = y; _wcvector3.z = z;
        return _wcvector3;
    }

    static private Vector2 _wcvector2 = new Vector2();
    static public Vector2 WCVector2(float x, float y)
    {
        _wcvector2.x = x; _wcvector2.y = y;
        return _wcvector2;
    }

    // Quaternion 재정의 (new 하지 않기위해)..
    static private Quaternion _wcquaternion = new Quaternion();
    static public Quaternion WCQuaternion(float x, float y, float z, float w)
    {
        _wcquaternion.x = x; _wcquaternion.y = y; _wcquaternion.z = z; _wcquaternion.w = w;
        return _wcquaternion;
    }

    // String 조합..
    static public StringBuilder _wctext = new StringBuilder();
    static public string getText(params string[] text)
    {
        _wctext.Length = 0;

        for (int i = 0; i < text.Length; i++)
            _wctext.Append(text[i]);

        return _wctext.ToString();
    }

    static public Transform FindChild(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = FindChild(parent.GetChild(i), name);
            if (child != null)
                return child;
        }

        return null;
    }

    // layer변경
    static public void LayerChild(Transform obj, int layername)
    {
        for(int i=0; i<obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);
            child.gameObject.layer = layername;
            Define.LayerChild(child, layername);
        }
    }

    // 수치값 -> 문자열(,)
    static public string CalIntToString(int value)
    {
        int num = 1000;
        List<int> list = new List<int>();
        while (value > 0)
        {
            int remainder = value % num;
            value -= remainder;
            value /= num;
            list.Add(remainder);
        }
        string text = "";
        for(int i=0; i<list.Count; i++)
        {
            int remainder = list[list.Count - (i + 1)];
            string zero = "";
            if (i > 0)
            {
                if (remainder < 10) zero = "00";
                else if (remainder < 100) zero = "0";
            }
            if(i >= list.Count - 1)
                text += (zero + remainder.ToString());
            else
                text += (zero + remainder.ToString() + ",");
        }
        return text;
    }
    #endregion

}