using System;
using UnityEngine;

public enum StoreTarget
{
    Local,
    Steam,
    Oculus,
    Viveport,
    VRoadcast,
}

public enum DeviceType
{
    Steam,
    Oculus,
    Normal,
}

public class StoreManager : MonoBehaviour
{
    public static StoreManager sInstance;
    public StoreTarget store;
    public DeviceType device;
    public StoreScriptId renderScript;

    private bool isInitialized;
    public static bool IsInitialized
    {
        get
        {
            return sInstance.isInitialized;
        }
    }

    private Store Store;

    public static bool IsCreated()
    {
        return sInstance != null;
    }

    void Awake()
    {
        sInstance = this;
        switch (store)
        {
            case StoreTarget.Local: Store = new LocalStore(StoreTarget.Local); break;
            case StoreTarget.Steam: Store = new SteamStore(StoreTarget.Steam); break;
            case StoreTarget.Oculus: Store = new OculusStore(StoreTarget.Oculus); break;
            case StoreTarget.Viveport: Store = new ViveportStore(StoreTarget.Viveport); break;
            case StoreTarget.VRoadcast: Store = new VRoadcastStore(StoreTarget.VRoadcast); break;
        }
        Debug.Log("store = " + store.ToString());
        if (OVRManager.isHmdPresent)
        {
            Debug.Log("Device = Oculus");
            device = DeviceType.Oculus;
        }
        else if (SteamVR.usingNativeSupport)
        {
            Debug.Log("Device = Steam");
            device = DeviceType.Steam;
        }
        else
        {
            device = DeviceType.Normal;
        }
    }

    void OnApplicationQuit()
    {
        if (Store != null)
            Store.Release();
    }

    void OnDestroy()
    {
        sInstance = null;
    }

    void Update()
    {
        if (Store != null)
            Store.Loop();
#if DEBUG_BUILDTEST || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Delete();
        }
#endif
    }

#if UNITY_EDITOR
    void OnGUI()
    {
        if (Store != null)
            Store.RenderOnGUI(renderScript);
    }
#endif

    public void Init(Action<bool> OnComplete)
    {
        if (Store == null)
            return;

        StartCoroutine(Store.Init((isInitialized) =>
        {
            this.isInitialized = isInitialized;

            if (OnComplete != null)
                OnComplete(isInitialized);
        }));

        //Store.Init((isInitialized) =>
        //{
        //    this.isInitialized = isInitialized;

        //    if (OnComplete != null)
        //        OnComplete(isInitialized);
        //});
    }

    public void Delete()
    {
        if (Store == null)
            return;

        Store.Delete();
    }

    public void Save()
    {
        if (Store == null)
            return;

        Store.Save();
    }

    public StoreScript GetScript(StoreScriptId Id)
    {
        if (Store == null)
            return null;

        return Store.GetScript(Id);
    }

    public T GetModel<T>(StoreScriptId Id)
    {
        StoreScript script = GetScript(Id);
        if (script == null)
            return default(T);

        return script.GetModel<T>();
    }
}
