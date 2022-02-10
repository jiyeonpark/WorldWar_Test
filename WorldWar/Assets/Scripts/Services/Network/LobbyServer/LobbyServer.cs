using System;
using UnityEngine;

public partial class LobbyServer : MonoBehaviour
{
    public static LobbyServer sInstance;

    public bool local = false;
    private BaseHttp http = new BaseHttp();

    void Awake()
    {
        sInstance = this;
    }

    void OnDestroy()
    {
        sInstance = null;
    }

    private Uri GetURL(string url)
    {
        return new Uri(GetURL() + url);
    }

    private string GetURL()
    {
        return local ? "http://localhost:57068/" : "http://battleofredcliffsvr-lobby.azurewebsites.net/";
    }
}
