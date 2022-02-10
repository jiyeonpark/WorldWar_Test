using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class JsonUtil
{
    static public float ToFloat(object o, float def)
    {
        if (o is long)
        {
            return (long)o;
        }

        if (o is int)
        {
            return (int)o;
        }

        if (o is double)
        {
            float f = (float)((double)o);
            return f;
        }

        if (o is float)
        {
            return (float)o;
        }


        Debug.LogWarning("Unable to convert value to float: " + (o == null ? "null" : o.ToString()));

        return def;
    }

    static long ToLong(object o, long def)
    {
        if (o is long)
        {
            return (long)o;
        }

        if (o is int)
        {
            return (long)(int)o;
        }

        if (o is double)
        {
            return (long)((double)o);
        }

        if (o is float)
        {
            return (long)((float)o);
        }

        Debug.LogWarning("Unable to convert value to long: " + (o == null ? "null" : o.ToString()));

        return def;
    }

    static public int ToInt(object o, int def)
    {        
        return Mathf.RoundToInt(ToFloat(o, def));
    }

    public static ArrayList ExtractArrayList(Hashtable data, string key, ArrayList def = null, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return (data[key] as ArrayList) ?? def;
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract array list " + key);
        }

        return def;
    }

    public static List<object> ExtractList(Hashtable data, string key, List<object> def = null, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return (data[key] as List<object>) ?? def;
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract array list " + key);
        }

        return def;
    }

    public static Hashtable ExtractHashtable(Hashtable data, string key, Hashtable def = null, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return (data[key] as Hashtable) ?? def;
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract hashtable " + key);
        }

        return def;
    }

    public static Dictionary<string, object> ExtractDictionary(Hashtable data, string key, Dictionary<string, object> def = null, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return (data[key] as Dictionary<string, object>) ?? def;
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract dictionary " + key);
        }

        return def;
    }

    public static float ExtractFloat(Hashtable data, string key, float def, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return ToFloat(data[key], def);
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract float " + key);
        }

        return def;
    }

    public static long ExtractLong(Hashtable data, string key, long def, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return ToLong(data[key], def);
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract long " + key);
        }

        return def;
    }

    public static int ExtractInt(Hashtable data, string key, int def, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            return ToInt(data[key], def);
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract int " + key);
        }

        return def;
    }

    public static bool ExtractBool(Hashtable data, string key, bool def, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            object o = data[key];

            if (o is bool)
            {
                return (bool)o;
            }
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract bool " + key);
        }

        return def;
    }

    public static string ExtractString(Hashtable data, string key, string def, bool logWarning = true)
    {
        if (data != null && data.ContainsKey(key))
        {
            object o = data[key];

            if (o is string)
            {
                return o as string;
            }
        }

        if (logWarning)
        {
            Debug.LogWarning("JsonUtil: Could not extract string " + key);
        }

        return def;
    }
}
