using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class StringUtil
{
    static StringBuilder _text = new StringBuilder();

    /// <summary>
    /// 뭐!! 그냥 필요해서 만들었다...
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ExtractDigit( string value )
    {
        string temp = string.Empty;

        if (string.IsNullOrEmpty(value))
            return temp;        

        for (int i = 0; i < value.Length; i++)
        {
            if (char.IsDigit(value[i]))
            {
                temp = value.Remove(0, i);
                break;
            }
        }
        return temp;
    }


    /// <summary>
    ///  우리 쓰는 미니 제이슨이 정말 기능이 미니해서  :(
    ///  한글이 인코딩이 안됨.. 아스키 값으로 변환해서 저장함 :)
    /// </summary>
    /// <param name="ascii"></param>
    /// <returns></returns>
    public static string DecodeASCIIChars(string ascii)
    {
        string result = string.Empty;

        string[] parts = ascii.Split(new string[] { "&#" }, StringSplitOptions.None);

        for (int i = 0; i < parts.Length; i++)
        {
            if( !string.IsNullOrEmpty(parts[i]))
                result += (char)Convert.ToInt32(parts[i]);
        }
        return result;
    }

    public static string DecodeStringASCII( string value )
    {
//        Debug.Log("value = " + value);
        string result = string.Empty;
        char[] charArray = value.ToCharArray();

        for (int i = 0; i < charArray.Length; i++)
        {
            result += "&#" + Convert.ToInt32(charArray[i]);
        }
        return result;
    }

    // Source: http://answers.unity3d.com/questions/244911/decode-html-charactersin-c.html
    public static string DecodeHtmlChars(string text)
    {
        string[] parts = text.Split(new string[]{"&#x"}, StringSplitOptions.None);

        for (int i = 1; i < parts.Length; i++)
        {
            int n = parts[i].IndexOf(';');
            string number = parts[i].Substring(0, n);
            try
            {
                int unicode = Convert.ToInt32(number, 16);
                parts[i] = ((char)unicode) + parts[i].Substring(n + 1);
            } catch {}
        }

        return String.Join("", parts);
    }

    public static string Trunc(string toTrunc, int maxLenght)
    {
        if (toTrunc != null && toTrunc.Length > maxLenght)
        {
            return toTrunc.Substring(0, maxLenght) + ".";
        }

        return toTrunc;
    }

    // Delete everything after the first space
    public static string TruncateUsername(string toTrunc, int maxLenght)
    {
        if (toTrunc != null && toTrunc.Length > maxLenght)
        {
            int i = toTrunc.IndexOf(' ');
            if (i > 0)
            {
                toTrunc = toTrunc.Substring(0, i);
            }

            return Trunc(toTrunc, maxLenght);
        }

        // No need to truncate anything, so...
        return toTrunc;
    }
    
    public static string Cleanup(string toClean)
    {
        if (toClean != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            foreach (char c in toClean.ToCharArray())
            {
                if (!char.IsControl(c) && !char.IsSurrogate(c))
                {
                    sb.Append(c);
                }
            }
            
            return sb.ToString();
        }
        
        return toClean;
    }

    public static string UppercaseFirst(string s)
    {
        if (!string.IsNullOrEmpty(s))
        {
            if (s.Length > 1)
            {
                return char.ToUpper(s[0]) + s.Substring(1);
            }

            return char.ToUpper(s[0]).ToString();
        }

        return s;
    }

    public static string ExportJsonString( string s )
    {
        if (!string.IsNullOrEmpty(s))
        {
            s = s.Replace("\\\"", "\"");
            s = s.Replace("\"{", "{");
            s = s.Replace("}\"", "}");
            s = s.Replace("\"[", "[");
            s = s.Replace("]\"", "]");
            return s;
        }
        return s;
    }
}
