using System;
using UnityEngine;
using WisecatAuthServer.Models;

public partial class AuthServer : MonoBehaviour
{
    public static AuthServer sInstance;

    public bool local = false;
    private BaseHttp http = new BaseHttp();

    public static Action<AuthResponseModel> ActionAuth;

    void Awake()
    {
        sInstance = this;
    }

    void OnDestroy()
    {
        sInstance = null;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        Auth();
    //    }
    //}

    private Uri GetURL(string url)
    {
        return new Uri(GetURL() + url);
    }

    private string GetURL()
    {
        return local ? "http://localhost:53821/" : "http://wisecat-auth.azurewebsites.net/";
    }

    public void Auth()
    {
        StoreUserModel storeModel = StoreManager.sInstance.GetModel<StoreUserModel>(StoreScriptId.User);
        if (storeModel == null)
        {
            if (ActionAuth != null)
                ActionAuth(null);
            return;
        }

        string appid = storeModel.AppID;
        string ticket = storeModel.AuthTicket;
        string userid = storeModel.ID;

        switch (StoreManager.sInstance.store)
        {
            case StoreTarget.Steam: SteamAuth(appid, userid); break;
            case StoreTarget.Oculus: OculusAuth(appid, userid, ticket); break;
            default: if (ActionAuth != null) ActionAuth(new AuthResponseModel() { Id = appid, result = true }); break;
        }
    }
}
