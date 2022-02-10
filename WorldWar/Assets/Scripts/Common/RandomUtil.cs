using UnityEngine;
using System;

public class RandomUtil
{
    static readonly System.Random _rng = new System.Random((int)DateTime.Now.Ticks);

    const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string RandomString(int size)
    {
        char[] buffer = new char[size];

        for (int i = 0; i < size; i++)
        {
            buffer[i] = _chars[_rng.Next(_chars.Length)];
        }

        return new string(buffer);
    }

    public static int RandomPow(float f, int max)
    {
        f = Mathf.Clamp01(f);

        int i = 0;

        while (i < max && UnityEngine.Random.value < f)
        {
            i++;
        }

        return i;
    }

    // test
    /*
    static RandomUtil()
    {
        Debug.Log("1- RandomPow(.5f, 5): "  + RandomPow(.5f, 5));
        Debug.Log("2- RandomPow(.5f, 5): "  + RandomPow(.5f, 5));
        Debug.Log("3- RandomPow(.5f, 5): "  + RandomPow(.5f, 5));
        Debug.Log("4- RandomPow(.5f, 5): "  + RandomPow(.5f, 5));
    }
    */
}
