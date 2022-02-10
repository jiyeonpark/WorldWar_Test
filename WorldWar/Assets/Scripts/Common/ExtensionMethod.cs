using UnityEngine;
using System.Collections;

/// <summary>
///  익스텐션 메쏘드 모음 
/// </summary>
public static class GameObjectExtensionMethod 
{
    public static T	GetSafeComponent<T>(this GameObject obj ) where T : MonoBehaviour
    {
        T component = obj.GetComponent<T>();

        if (component == null)
        {
            Debug.Log("Expected o find component of type" + typeof(T) + "but found none", obj);
        }

        return component;
    }

    public static T GetSafeComponentInChildren<T>(this GameObject obj) where T : MonoBehaviour
    {
        T component;
        if (obj != null)
        {
            component = obj.GetComponentInChildren<T>();

            if (component == null)
            {
                Debug.Log("Expected o find component of type" + typeof(T) + "but found none", obj);
            }
            return component;
        }

        return default(T);
    }

    public static void SafeSetActive( this GameObject obj, bool active )
    {
        if (obj != null)
            obj.SetActive(active);
        else
            Debug.LogWarning("object is null");
    }
}

public static class AnimationExtensionMethod
{
    public static void PlayActive(this Animation anim, string animname, PlayMode mode, float speed = 1.0f)
    {
        anim.gameObject.SafeSetActive(true);
        anim[animname].speed = speed;
        if (anim.Play(animname, mode))
            Debug.Log("PLay anim " + animname);
    }
}
