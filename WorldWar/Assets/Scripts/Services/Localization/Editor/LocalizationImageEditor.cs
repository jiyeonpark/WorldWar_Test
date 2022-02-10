using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(LocalizationImage))]
public class LocalizationImageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        DrawSprites((LocalizationImage)target);
    }

    private void DrawSprites(LocalizationImage target)
    {
        EditorGUILayout.LabelField("Sprites", EditorStyles.boldLabel);

        if (target.Sprites == null)
            target.Sprites = new List<Sprite>();

        for (var i = 0; i < (int)Language.Max; i++)
        {
            if (target.Sprites.Count <= i)
                target.Sprites.Add(null);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(((Language)i).ToString(), GUILayout.Width(110));
            target.Sprites[i] = (Sprite)EditorGUILayout.ObjectField(target.Sprites[i], typeof(Sprite), true);
            EditorGUILayout.EndHorizontal();
        }
    }
}
