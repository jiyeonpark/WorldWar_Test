using UnityEngine;
using System;
using System.Collections;

public static class EnumUtil
{
    public static T Parse<T> (string s, T defaultValue) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
        {
            Debug.LogWarning("Invalid Enum: " + s);
            return defaultValue;
        }

        if( string.IsNullOrEmpty( s ) )
        {
            return defaultValue;
        }        

        T val = defaultValue;
        
        try 
        {
            T t = (T)Enum.Parse(typeof(T), s);
            if (Enum.IsDefined(typeof(T), t))
            {
                val = t;
            }
        }
        catch (ArgumentException)
        {
            Debug.LogWarning("Invalid Enum: " + s);
        }
        
        return val;
    }
}
