using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LocalizationManager))]
public class LocalizationManagerEditor : Editor
{
    private int intPopup = 0;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        DrawLanguage((LocalizationManager)target);
        DrawFonts((LocalizationManager)target);
    }

    private void DrawLanguage(LocalizationManager target)
    {
        string[] keys = new string[] { Language.English.ToString(), Language.Korean.ToString(), Language.Japanese.ToString(), Language.ChineseTraditional.ToString(), Language.ChineseSimplified.ToString(), Language.Spanish.ToString() };

        intPopup = (int)target.Language;

        var listId = new int[keys.Length];
        for (var i = 0; i < keys.Length; i++)
            listId[i] = i;

        intPopup = EditorGUILayout.IntPopup("Language", intPopup, keys, listId);

        if ((int)target.Language != intPopup || string.IsNullOrEmpty(target.Language.ToString()))
            target.Language = (Language)intPopup;
    }

    private void DrawFonts(LocalizationManager target)
    {
        EditorGUILayout.LabelField("Fonts");

        if (target.Fonts == null)
            target.Fonts = new List<Font>();

        if (target.FontStyles == null)
            target.FontStyles = new List<FontStyle>();

        for (var i = 0; i < (int)Language.Max; i++)
        {
            if (target.Fonts.Count <= i)
                target.Fonts.Add(null);

            if (target.FontStyles.Count <= i)
                target.FontStyles.Add(FontStyle.Normal);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(((Language)i).ToString(), EditorStyles.boldLabel, GUILayout.Width(70));

            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Font", GUILayout.Width(70));
            target.Fonts[i] = (Font)EditorGUILayout.ObjectField(target.Fonts[i], typeof(Font), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("FontStyle", GUILayout.Width(70));
            target.FontStyles[i] = (FontStyle)EditorGUILayout.EnumPopup(target.FontStyles[i]);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
    }
}
