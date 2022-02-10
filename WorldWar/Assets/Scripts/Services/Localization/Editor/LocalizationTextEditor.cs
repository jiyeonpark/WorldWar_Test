using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LocalizationText))]
public class LocalizationTextEditor : Editor
{
    private int selectIndex = 0;

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        DrawKeys((LocalizationText)target);
        DrawValues((LocalizationText)target);
    }

    private void DrawKeys(LocalizationText target)
    {
        int dropdown = 1;
        List<object> datas = LocalizationManager.GetLocalizationKeys(dropdown);
        if (datas == null)
            return;

        string[] keys = new string[datas.Count];
        for (int i = 0; i < datas.Count; i++)
        {
            LanguagesXml.Data data = (LanguagesXml.Data)datas[i];
            keys[i] = data.Key;
        }

        var keyId = GetIdByKey(target.Key, keys);
        if (keyId == -1)
        {
            keyId = 0;
            //GUILayout.HelpBox("KEY not found in localization file. ", MessageType.Error);
        }

        selectIndex = keyId;

        GUILayout.BeginHorizontal();
        {
            GUILayout.Label("Keys", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
            if (GUILayout.Button(keys[selectIndex], GUILayout.ExpandWidth(true)))
            {
                GUILayoutScrollPopup.Create((scrollpopup) =>
                {
                    scrollpopup.OpenPopup(keys, selectIndex, 40, 20f, selectCallback: (index) =>
                    {
                        selectIndex = index;

                        if (keyId != selectIndex || string.IsNullOrEmpty(target.Key))
                            target.Key = keys[selectIndex];
                    });
                });
            }
        }
        GUILayout.EndHorizontal();

        if (keyId != selectIndex || string.IsNullOrEmpty(target.Key))
            target.Key = keys[selectIndex];
    }

    //private void DrawKeys(LocalizationText target)
    //{
    //    List<object> datas = LocalizationManager.GetLocalizationKeys();
    //    if (datas == null)
    //        return;

    //    string[] keys = new string[datas.Count];
    //    for (int i = 0; i < datas.Count; i++)
    //    {
    //        LanguagesXml.Data data = (LanguagesXml.Data)datas[i];
    //        keys[i] = data.Key;
    //    }

    //    var keyId = GetIdByKey(target.Key, keys);
    //    if (keyId == -1)
    //    {
    //        keyId = 0;
    //        EditorGUILayout.HelpBox("KEY not found in localization file. ", MessageType.Error);
    //    }

    //    intPopup = keyId;

    //    var listId = new int[keys.Length];
    //    for (var i = 0; i < keys.Length; i++)
    //        listId[i] = i;

    //    intPopup = EditorGUILayout.IntPopup("Key", intPopup, keys, listId);

    //    if (keyId != intPopup || string.IsNullOrEmpty(target.Key))
    //        target.Key = keys[intPopup];
    //}

    private void DrawValues(LocalizationText target)
    {
        if (target.Key == null)
            return;

        LanguagesXml.Data datas = (LanguagesXml.Data)LocalizationManager.GetLocalizationKeys(target.Key);

        GUILayout.Label("Values", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));

        for (var i = 0; i < (int)Language.Max; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(((Language)i).ToString(), GUILayout.Width(110));
            GUILayout.Label(datas.Values[i], GUILayout.Width(110));
            GUILayout.EndHorizontal();
        }
    }

    private int GetIdByKey(string key, string[] keys)
    {
        for (int index = 0; index < keys.Length; index++)
        {
            if (keys[index] == key)
                return index;
        }

        return -1;
    }
}
