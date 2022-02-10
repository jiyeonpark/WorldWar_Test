#define DEBUG

using UnityEngine;
using System.Collections;

public class AutoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static bool _bIsAplicationQuit = false;
    static T _instance = default(T);
    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            OnAwake();
        }
        else
        {
            Debug.LogWarning("Creating more than one instance of a singleton: " + typeof(T).ToString());
            Destroy(gameObject);
        }
    }

    public static bool IsCreated()
    {
        return _instance != null;
    }

    public void Create()
    {
        // nothing to do!
    }

    public void Start()
    {
        OnStart();
    }

    protected virtual void OnAwake()
    {
    }

    void OnApplicationQuit()
    {
        _bIsAplicationQuit = true;
    }

    public static bool IsAplicationQuit()
    {
        return _bIsAplicationQuit;
    }

    protected virtual void OnStart()
    {
    }

    public virtual void Destroy()
    {
        _instance = null;

        Destroy(gameObject);
    }

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //if (_bIsAplicationQuit)
                //    return null;
                GameObject go = new GameObject(typeof(T).ToString());
                _instance = go.AddComponent<T>();
            }

            return _instance;
        }
    }
}
