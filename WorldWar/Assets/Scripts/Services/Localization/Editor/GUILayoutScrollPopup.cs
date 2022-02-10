using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GUILayoutScrollPopup : EditorWindow
{
    private static GUILayoutScrollPopup scrollpopup = null;

    private string[] keys;
    private Vector2 scrollPos;
    private int selectIndex;
    private int maxView;
    private float itemHeight;
    private Action<int> selectCallback;

    public static void Create(Action<GUILayoutScrollPopup> callback)
    {
        scrollpopup = GetWindow(typeof(GUILayoutScrollPopup)) as GUILayoutScrollPopup;
        callback(scrollpopup);
    }

    public void OpenPopup(string[] keys, int selectIndex, int maxView = 10, float itemHeight = 20f, Action<int> selectCallback = null)
    {
        this.keys = keys;
        this.selectIndex = selectIndex;
        this.maxView = maxView;
        this.itemHeight = itemHeight;
        this.selectCallback = selectCallback;

        Rect rect = position;
        rect.height = maxView * itemHeight;

        ShowPopup();
    }

    void OnGUI()
    {
        if (scrollpopup == null)
        {
            return;
        }

        GUILayout.BeginHorizontal();
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(maxView * itemHeight));
            {
                int maxkeys = keys.Length;
                int firstIndex = (int)(scrollPos.y / itemHeight);
                firstIndex = Mathf.Clamp(firstIndex, 0, Mathf.Max(0, maxkeys - maxView));

                GUILayout.Space(firstIndex * itemHeight);
                for (int i = firstIndex; i < Mathf.Min(maxkeys, firstIndex + maxView); ++i)
                {
                    if (GUILayout.Button(keys[i], GUILayout.Height(itemHeight)))
                    {
                        selectIndex = i;
                        if (selectCallback != null)
                            selectCallback(selectIndex);
                        scrollpopup = null;
                        Close();
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();
    }
}
