using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Random = System.Random;

public static class ListExtension
{
    static Random _random = new Random();

    // Fisher-Yates shuffle
    public static void Shuffle<T>(this List<T> list)
    {
        var random = _random;

        for (int i = list.Count; i > 1; i--)
        {
            // Pick random element to swap
            int j = random.Next(i); // 0 <= j <= i-1
            // Swap.
            T tmp = list[j];
            list[j] = list[i - 1];
            list[i - 1] = tmp;
        }
    }

    public static void MoveToEnd<T>(this List<T> list, T element)
    {
        if (list.Contains(element))
        {
            int from = list.IndexOf(element);
            list.RemoveAt(from);
            list.Insert(list.Count, element);
        }
    }

    /*

	void Test() 
    {
        List<int> list = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            list.Add(i);
        }

        list.Shuffle();

        for (int i = 0; i < 10; i++)
        {
            Debug.Log(String.Format("list[{0}] = {1}", i, list[i]));
        }
	}

    */
}
