using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class UIArrangeEditorWindow : EditorWindow
{
    private Vector2 scrollPos;
    private int maxView = 50;
    private float itemWidth = 300;
    private float itemHeight = 200;

    public static void ShowWindow()
    {
        GetWindow(typeof(UIArrangeEditorWindow));
    }

    void OnGUI()
    {
        List<LocalizationLoadImages> files = GetFiles();
        
        GUILayout.BeginHorizontal();
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos/*, GUILayout.Height(maxView * itemHeight)*/);
            {
                int maxkeys = files.Count;
                int firstIndex = (int)(scrollPos.y / itemHeight);
                firstIndex = Mathf.Clamp(firstIndex, 0, Mathf.Max(0, maxkeys - maxView));

                GUILayout.Space(firstIndex * itemHeight);

                for (int i = firstIndex; i < Mathf.Min(maxkeys, firstIndex + maxView); ++i)
                {
                    GUILayout.BeginHorizontal();

                    for (int j = 0; j < files[i].Images.Count; j++)
                    {
                        string image = files[i].Images[j];
                        if (j == 0)
                        {
                            GUILayout.Label(image, GetStyle(), GUILayout.Width(400), GUILayout.Height(itemHeight));
                        }

                        if (image != null)
                        {
                            string value = image.Remove(image.Length - 4, 4);
                            Texture2D texture = LocalizationManager.GetLocalizationTexture2D(((Language)j).ToString(), value);
                            GUILayout.Label(texture, GetStyle(), GUILayout.Width(itemWidth), GUILayout.Height(itemHeight));
                        }
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
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        return style;
    }

    //private void GetWarningFiles()
    //{
    //    List<string> englishs = GetFiles("English");
    //    List<string> koreans = GetFiles("Korean");
    //    List<string> japaneses = GetFiles("Japanese");
    //    List<string> chineses = GetFiles("Chinese");

    //    for(int i = 0; i < koreans.Count; i++)
    //    {
    //        string korean = koreans[i];
    //        string english = englishs.Find(p => p == korean);

    //        if (english == null)
    //        {
    //            string value = korean.Remove(korean.Length - 4, 4);
    //            Texture2D texture = LocalizationManager.GetLocalizationTexture2D(Language.Korean.ToString(), value);
    //            Debug.LogWarning("not match korean", texture);
    //        }
    //    }
    //}

    private List<LocalizationLoadImages> GetFiles()
    {
        List<string> englishs = GetFiles("English");
        List<string> koreans = GetFiles("Korean");
        List<string> japaneses = GetFiles("Japanese");
        List<string> chinesestraditionals = GetFiles("ChineseTraditional");
        List<string> chinesessimplifieds = GetFiles("ChineseSimplified");
        List<string> spanishs = GetFiles("Spanish");

        List<LocalizationLoadImages> datas = new List<LocalizationLoadImages>();

        for (int i = 0; i < englishs.Count; i++)
        {   //영어 기준 정렬
            string english = englishs[i];
            string korean = koreans.Find(p => p == english);
            string japanese = japaneses.Find(p => p == english);
            string chinesestraditional = chinesestraditionals.Find(p => p == english);
            string chinesessimplified = chinesessimplifieds.Find(p => p == english);
            string spanish = spanishs.Find(p => p == english);

            LocalizationLoadImages data = new LocalizationLoadImages();
            data.Images = new List<string>();
            data.Images.Add(english);
            data.Images.Add(korean);
            data.Images.Add(japanese);
            data.Images.Add(chinesestraditional);
            data.Images.Add(chinesessimplified);
            data.Images.Add(spanish);

            datas.Add(data);
        }

        return datas;
    }

    private List<string> GetFiles(string path)
    {
        string fullpath = Directory.GetCurrentDirectory() + "/Assets/Resources/LocalizationSprites/" + path;
        string[] files = Directory.GetFileSystemEntries(fullpath);

        List<string> output = new List<string>();

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i].Remove(0, fullpath.Length + 1);
            if (file.Contains(".meta"))
                continue;

            output.Add(file);
        }

        return output;
    }
}
