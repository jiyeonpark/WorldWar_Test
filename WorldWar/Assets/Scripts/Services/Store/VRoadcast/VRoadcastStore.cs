using HttpAuth;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRoadcastStore : Store
{
    public VRoadcastStore(StoreTarget target) : base(target) { }

    private BaseHttp http = new BaseHttp();

    public class VRoadcastResponseModel
    {
        public int result { get; set; }
        public string success_yn { get; set; }
        public string result_msg { get; set; }
    }

    public override IEnumerator Init(Action<bool> OnComplete)
    {
        GameAuthInfo authinfo = new GameAuthInfo()
        {
            developCode = "wise",
            gameCode = "batt",
            versionNumber = "0001",
        };

        GameAuth auth = new GameAuth(authinfo);
        string strEncrypt = auth.GetURLParamString();

        Dictionary<string, string> postString = new Dictionary<string, string>();
        postString.Add("GameCode", strEncrypt);

        isInitialized = false;

        string url = auth.GetConnectURL();
        if (url.Length == 0)
        {
            Debug.LogError("WebAuth Connect Error!!");
        }
        else
        {
            http.PostField(new Uri(url), postString, (req, res) =>
            {
                if (res.IsSuccess)
                {
                    string data = res == null ? "" : res.DataAsText;

                    VRoadcastResponseModel sc = JsonConvert.DeserializeObject<VRoadcastResponseModel>(data);

                    if (sc.success_yn == "Y")
                    {
                        StoreScripts.Add(new LocalUserScript(StoreScriptId.User, new StoreUserModel()));
                        StoreScripts.Add(new VRoadcastRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));

                        for (int i = 0; i < StoreScripts.Count; i++)
                        {
                            StoreScripts[i].Init();
                        }

                        isInitialized = res.IsSuccess;
                    }
                }
            });
        }

        yield return new WaitForSeconds(2f);

        if (OnComplete != null)
            OnComplete(isInitialized);
    }

    //public override void Init(Action<bool> OnComplete)
    //{
    //    GameAuthInfo authinfo = new GameAuthInfo()
    //    {
    //        developCode = "wise",
    //        gameCode = "batt",
    //        versionNumber = "0001",
    //    };

    //    GameAuth auth = new GameAuth(authinfo);
    //    string strEncrypt = auth.GetURLParamString();

    //    Dictionary<string, string> postString = new Dictionary<string, string>();
    //    postString.Add("GameCode", strEncrypt);

    //    isInitialized = false;

    //    string url = auth.GetConnectURL();
    //    if(url.Length == 0)
    //    {
    //        Debug.LogError("WebAuth Connect Error!!");

    //        if (OnComplete != null)
    //            OnComplete(isInitialized);
    //        return;
    //    }

    //    http.PostField(new Uri(url), postString, (req, res) =>
    //    {
    //        if (res.IsSuccess)
    //        {
    //            string data = res == null ? "" : res.DataAsText;

    //            VRoadcastResponseModel sc = JsonConvert.DeserializeObject<VRoadcastResponseModel>(data);

    //            if (sc.success_yn == "Y")
    //            {
    //                StoreScripts.Add(new LocalUserScript(StoreScriptId.User, new StoreUserModel()));
    //                StoreScripts.Add(new VRoadcastRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));

    //                for (int i = 0; i < StoreScripts.Count; i++)
    //                {
    //                    StoreScripts[i].Init();
    //                }

    //                isInitialized = res.IsSuccess;
    //            }
    //        }

    //        if (OnComplete != null)
    //            OnComplete(isInitialized);
    //    });
    //}
}
