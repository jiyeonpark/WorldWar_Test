using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UITextArrangeEditorWindow : EditorWindow
{
    private Vector2 scrollPos;
    private int maxView = 50;
    private float itemWidth = 500;
    private float itemHeight = 100;

    public static void ShowWindow()
    {
        GetWindow(typeof(UITextArrangeEditorWindow));
    }

    void OnGUI()
    {
        List<LanguagesXml.Data> files = GetFiles();

        GUILayout.BeginHorizontal();
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            {
                int maxkeys = files.Count;
                int firstIndex = (int)(scrollPos.y / itemHeight);
                firstIndex = Mathf.Clamp(firstIndex, 0, Mathf.Max(0, maxkeys - maxView));

                GUILayout.Space(firstIndex * itemHeight);

                for (int i = firstIndex; i < Mathf.Min(maxkeys, firstIndex + maxView); ++i)
                {
                    GUILayout.BeginHorizontal();

                    string key = files[i].Key;
                    GUILayout.Label(key, GetStyle(), GUILayout.Width(500), GUILayout.Height(itemHeight));

                    for (int j = 0; j < files[i].Values.Count; j++)
                    {
                        string value = files[i].Values[j];
                        GUILayout.Label(value, GetStyle(), GUILayout.Width(itemWidth), GUILayout.Height(itemHeight));
                    }

                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
        GUILayout.EndHorizontal();
    }

    private GUIStyle GetStyle()
    {
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleLeft;
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        return style;
    }

    private List<LanguagesXml.Data> GetFiles()
    {
        List<object> datas = LocalizationManager.GetLocalizationKeys();

        List<LanguagesXml.Data> output = new List<LanguagesXml.Data>();
        for(int i = 0; i < datas.Count; i++)
            output.Add((LanguagesXml.Data)datas[i]);

        return output;
    }
}
