using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store
{
    public StoreTarget target;

    protected bool isInitialized;
    protected List<StoreScript> StoreScripts = new List<StoreScript>();

    public Store(StoreTarget target) { this.target = target; }

    public virtual IEnumerator Init(Action<bool> OnComplete)
    {
        isInitialized = true;

        if (OnComplete != null)
            OnComplete(isInitialized);

        yield return null;
    }

    //public virtual void Init(Action<bool> OnComplete)
    //{
    //    isInitialized = true;

    //    if (OnComplete != null)
    //        OnComplete(isInitialized);
    //}

    public virtual void Delete()
    {
        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].Delete();
    }

    public virtual void Release()
    {
        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].Release();
    }

    public virtual void Save()
    {
        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].SaveFile();
    }

    public virtual void Loop()
    {
        if (!isInitialized)
            return;

        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].Loop();
    }

#if UNITY_EDITOR
    public virtual void RenderOnGUI(StoreScriptId renderScript)
    {
        if (renderScript == StoreScriptId.None)
            return;

        if (!isInitialized)
        {
            GUILayout.Label("Store is not Initialized");
            return;
        }

        StoreScript script = GetScript(renderScript);
        if (script == null)
            return;

        script.RenderOnGUI();
    }
#endif

    public StoreScript GetScript(StoreScriptId Id)
    {
        return StoreScripts.Find(p => p.Id == Id);
    }

    public T GetModel<T>(StoreScriptId Id)
    {
        StoreScript script = GetScript(Id);
        if (script == null)
            return default(T);

        return script.GetModel<T>();
    }
}
