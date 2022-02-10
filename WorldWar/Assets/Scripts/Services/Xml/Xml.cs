using System.Collections.Generic;
using System.Xml;

public class Xml {
    public const string IsNullOrEmpty = "";
    public string key;

    protected List<object> _datas = new List<object>();

    public bool LoadXML(XmlElement xmlElement)
    {
        int nodecount = xmlElement.ChildNodes.Count;
        for (int i = 0; i < nodecount; i++)
        {
            XmlNode node = xmlElement.ChildNodes[i];
            if (!Set(node))
                return false;
        }

        return true;
    }

    public virtual bool Set(XmlNode node) { return true; }
    public virtual List<object> Find() { return _datas; }
    public virtual List<object> Find(object key) { return null; }

    public string GetAttributes(XmlNode node, string key) { return node.Attributes[key] == null ? IsNullOrEmpty : node.Attributes[key].Value; }
    public int Parseint(string key) { return key == IsNullOrEmpty ? 0 : int.Parse(key); }
    public float Parsefloat(string key) { return key == IsNullOrEmpty ? 0f : float.Parse(key); }
    public bool Parsebool(string key) { return key == IsNullOrEmpty ? false : bool.Parse(key); }
    public byte Parsebyte(string key) { return key == IsNullOrEmpty ? (byte)0 : byte.Parse(key); }
}
