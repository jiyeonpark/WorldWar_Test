using System.Xml;
using System.Collections.Generic;

public class LanguagesXml : Xml
{
    public struct Data
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
        public int Dropdown { get; set; }
    }

    public override bool Set(XmlNode node)
    {
        string Key = GetAttributes(node, "Key");
        string English = GetAttributes(node, "English");
        string Korean = GetAttributes(node, "Korean");
        string Japanese = GetAttributes(node, "Japanese");
        string ChineseTraditional = GetAttributes(node, "ChineseTraditional");
        string ChineseSimplified = GetAttributes(node, "ChineseSimplified");
        string Spanish = GetAttributes(node, "Spanish");
        string Dropdown = GetAttributes(node, "dropdown");

        Data data = new Data();
        data.Key = Key;
        data.Values = new List<string>();
        data.Values.Add(English);
        data.Values.Add(Korean);
        data.Values.Add(Japanese);
        data.Values.Add(ChineseTraditional);
        data.Values.Add(ChineseSimplified);
        data.Values.Add(Spanish);
        data.Dropdown = Parseint(Dropdown);

        _datas.Add(data);
        return true;
    }

    public object FindKey(string Key)
    {   //Key 리스트 추출
        return _datas.Find(p => ((Data)p).Key == Key);
    }

    public List<object> FindKeys(int Dropdown)
    {   //Key 리스트 추출
        return _datas.FindAll(p => ((Data)p).Dropdown == Dropdown);
    }
}

