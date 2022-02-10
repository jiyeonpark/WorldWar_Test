using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIArrange))]
public class UIArrangeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIArrange arrange = target as UIArrange;

        if (GUILayout.Button("정리"))
            arrange.Arrange();

        if (GUILayout.Button("비교 LocalizationText"))
            UITextArrangeEditorWindow.ShowWindow();

        if (GUILayout.Button("비교 LocalizationLoadImage"))
            UIArrangeEditorWindow.ShowWindow();

        if (GUILayout.Button("매칭 안됨 LocalizationLoadImage"))
            arrange.CheckWarningFiles();

        GUILayout.Space(10f);

        if (GUILayout.Button("FindLocalizationText"))
            arrange.FindLocalizationText();

        if (GUILayout.Button("FindLocalizationLoadImage"))
            arrange.FindLocalizationLoadImage();

        if (GUILayout.Button("FindLocalizationError"))
            arrange.FindLocalizationError();

        base.OnInspectorGUI();
    }
}