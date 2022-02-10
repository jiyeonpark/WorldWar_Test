//using JBooth.VertexPainterPro;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationLoadImages
{
    public List<string> Images { get; set; }
}

public class UIArrange : MonoBehaviour
{
#if UNITY_EDITOR
    public void Arrange()
    {
        Transform[] trans = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < trans.Length; i++)
        {
            GameObject obj = trans[i].gameObject;

            CheckLocalizationImage(obj);
            //RemoveVertexInstanceStream(obj);
        }
    }

    public void FindLocalizationError()
    {
        Transform[] trans = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < trans.Length; i++)
        {
            GameObject obj = trans[i].gameObject;

            LocalizationText[] texts = obj.GetComponents<LocalizationText>();
            if (texts.Length > 1)
            {
                Debug.Log("LocalizationText Find Length error : " + obj.name, obj);
            }

            LocalizationLoadImage[] images = obj.GetComponents<LocalizationLoadImage>();
            if (images.Length > 1)
            {
                Debug.Log("LocalizationLoadImage Find Length error : " + obj.name, obj);
            }
        }
    }

    public void FindLocalizationText()
    {
        Transform[] trans = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < trans.Length; i++)
        {
            GameObject obj = trans[i].gameObject;

            LocalizationText[] texts = obj.GetComponents<LocalizationText>();
            if(texts.Length > 1)
            {
                Debug.Log("LocalizationText Find Length error : " + obj.name, obj);
            }

            LocalizationText text = obj.GetComponent<LocalizationText>();
            if (text != null)
            {
                Debug.Log("LocalizationText Find : " + text.Key + " / " + obj.name, obj);
            }
        }
    }

    public void FindLocalizationLoadImage()
    {
        Transform[] trans = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < trans.Length; i++)
        {
            GameObject obj = trans[i].gameObject;
            LocalizationLoadImage image = obj.GetComponent<LocalizationLoadImage>();
            if (image != null)
            {
                Debug.Log("LocalizationLoadImage Find : " + obj.name, obj);
            }
        }
    }

    private void CheckLocalizationImage(GameObject obj)
    {
        Image image = obj.GetComponent<Image>();
        if (image == null)
            return;

        LocalizationLoadImage component = obj.GetComponent<LocalizationLoadImage>();
        if (image.sprite == null)
        {
            //Debug.LogWarning("CheckSprite Error << none sprite : " + obj.name, obj);
            return;
        }

        string spritename = image.sprite.name;
        bool Islocalization = LocalizationManager.GetLocalizationSpriteEditor("English", spritename);
        if (Islocalization)
        {
            if (component == null)
            {
                obj.AddComponent<LocalizationLoadImage>();
                Debug.LogWarning("CheckLocalizationImage Add : " + obj.name, obj);
            }
        }
        else
        {
            if (component != null)
            {
                Debug.LogWarning("CheckLocalizationImage Del : " + obj.name, obj);
                DestroyImmediate(component);
            }
        }
    }

    public void CheckWarningFiles()
    {
        List<string> englishs = GetFiles("English");
        List<string> koreans = GetFiles("Korean");
        List<string> japaneses = GetFiles("Japanese");
        List<string> chinesestraditional = GetFiles("ChineseTraditional");
        List<string> chinesessimplified = GetFiles("ChineseSimplified");
        List<string> spanish = GetFiles("Spanish");

        CheckWarningFiles(Language.Korean, englishs, koreans);
        CheckWarningFiles(Language.Japanese, englishs, japaneses);
        CheckWarningFiles(Language.ChineseTraditional, englishs, chinesestraditional);
        CheckWarningFiles(Language.ChineseSimplified, englishs, chinesessimplified);
        CheckWarningFiles(Language.Spanish, englishs, spanish);
    }

    private void CheckWarningFiles(Language language, List<string> lefts, List<string> rights)
    {
        for (int i = 0; i < rights.Count; i++)
        {
            string right = rights[i];
            string left = lefts.Find(p => p == right);

            if (left == null)
            {
                string value = right.Remove(right.Length - 4, 4);
                Texture2D texture = LocalizationManager.GetLocalizationTexture2D(language.ToString(), value);
                Debug.LogWarning("not match " + language, texture);
            }
        }
    }

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

    //private void RemoveVertexInstanceStream(GameObject obj)
    //{
    //    VertexInstanceStream vertexInstanceStream = obj.GetComponent<VertexInstanceStream>();
    //    if (vertexInstanceStream == null)
    //        return;

    //    Renderer renderer = obj.GetComponent<Renderer>();
    //    if (renderer == null || renderer.sharedMaterial == null)
    //    {
    //        DestroyImmediate(vertexInstanceStream);
    //        Debug.LogWarning("RemoveVertexInstanceStream : " + obj.name, obj);
    //        return;
    //    }

    //    string shadername = renderer.sharedMaterial.shader.name;

    //    if (shadername.Contains("VertexPainter"))
    //    {
    //        obj.isStatic = false;
    //        return;
    //    }

    //    DestroyImmediate(vertexInstanceStream);
    //    Debug.LogWarning("RemoveVertexInstanceStream : " + obj.name, obj);
    //}
#endif
}
