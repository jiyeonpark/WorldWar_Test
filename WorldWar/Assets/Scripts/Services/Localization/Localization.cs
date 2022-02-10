using UnityEngine;

public class Localization<T> : MonoBehaviour
{
    protected T Value;

    void Start()
    {
        Value = GetComponent<T>();

        LocalizationManager.sInstance.OnChangeLocalization += OnChangeLocalization;

        OnChangeLocalization();

        Init();
    }

    void OnDestroy()
    {
        if(LocalizationManager.sInstance != null)
            LocalizationManager.sInstance.OnChangeLocalization -= OnChangeLocalization;
    }

    protected virtual void Init() { }
    protected virtual void OnChangeLocalization() { }
}
