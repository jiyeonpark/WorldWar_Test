using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;

public static class XMLNAME
{   //XML 파일 네임
    public const string Languages = "Languages";
    public const string Stages = "Stages";
    public const string Items = "Items";
}

public class XmlManager : MonoBehaviour
{
    public static XmlManager sInstance;

    private List<Xml> _xml = new List<Xml>();

    void Awake()
    {
        sInstance = this;

        Load(new LanguagesXml(), XMLNAME.Languages);
        //Load(new StagesXml(), XMLNAME.Stages);
        //Load(new ItemsXml(), XMLNAME.Items);
    }

    public bool Load(Xml xml, string key)
    {
        try
        {
            string fullpath = "Xml/" + key;
            TextAsset textAsset = Resources.Load(fullpath) as TextAsset;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);
            if (xmlDoc == null)
                return false;

            xml.key = key;
            if (!xml.LoadXML(xmlDoc.DocumentElement))
                return false;

            //중복 키 XML 존재시 덮처쓰기
            Xml tempxml = Find(key);
            if (tempxml != null)
            {
                _xml.Remove(tempxml);
            }

            _xml.Add(xml);

            return true;
        }
        catch (Exception e)
        {
            string message = e.Message;
            Debug.LogError(message);
        }

        return false;
    }

    public Xml Find(string key)
    {
        return _xml.Find(p => p.key == key);
    }

#if UNITY_EDITOR
    public static Xml Find(Xml xml, string key)
    {
        try
        {
            string fullpath = "Xml/" + key;
            TextAsset textAsset = Resources.Load(fullpath) as TextAsset;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);
            if (xmlDoc == null)
                return null;

            xml.key = key;
            if (!xml.LoadXML(xmlDoc.DocumentElement))
                return null;

            return xml;
        }
        catch (Exception e)
        {
            string message = e.Message;
            Debug.LogError(message);
        }

        return null;
    }
#endif
}