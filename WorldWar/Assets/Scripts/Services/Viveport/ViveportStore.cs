using System;
using System.Collections;
using UnityEngine;
using Viveport;

public class ViveportStore : Store
{
    private Action<bool> OnComplete;

    public ViveportStore(StoreTarget target) : base(target) { }

    public static string APP_ID = "cd4637a8-7d91-4b5e-b32a-e6fb48b8d04a";
    public static string APP_KEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCVHwdZ6ayurxwn2JlIFrhs/"
                          + "2FhyaZ81i+fSQLmFfaUFQ2Dh8PvDAVWVhCpFFqHo2Zrg9opXcT4gSRVbEn+"
                          + "0L1GwtSZdagVBKw0jutDHdXkN+BqyuttyJQAbjaWwsz8aiUE7vuPK2k+"
                          + "QTaP62Gw8o43WI8apXW8gWneZIkrOZWurwIDAQAB";

    class ViveportLicenseChecker : Api.LicenseChecker
    {
        private Action<bool> OnComplete;

        public ViveportLicenseChecker(Action<bool> OnComplete)
        {
            this.OnComplete = OnComplete;
        }

        public override void OnSuccess(long issueTime, long expirationTime, int latestVersion, bool updateRequired)
        {
            if(OnComplete != null)
                OnComplete(true);
        }

        public override void OnFailure(int errorCode, string errorMessage)
        {
            if (OnComplete != null)
                OnComplete(false);
        }
    }

    public override IEnumerator Init(Action<bool> OnComplete)
    {
        Api.Init((result) =>
        {
            if (result == 0)
            {   //성공
                UserStats.IsReady((readyresult) =>
                {
                    if (readyresult == 0)
                    {
                        this.OnComplete = OnComplete;

                        Api.GetLicense(new ViveportLicenseChecker(OnCompleteCallback), APP_ID, APP_KEY);
                    }
                });
            }
        }, APP_ID);

        yield return null;
    }

    //public override void Init(Action<bool> OnComplete)
    //{
    //    Api.Init((result) =>
    //    {
    //        if (result == 0)
    //        {   //성공
    //            UserStats.IsReady((readyresult) =>
    //            {
    //                if (readyresult == 0)
    //                {
    //                    this.OnComplete = OnComplete;

    //                    Api.GetLicense(new ViveportLicenseChecker(OnCompleteCallback), APP_ID, APP_KEY);
    //                }
    //            });
    //        }
    //    }, APP_ID);
    //}

    public override void Release()
    {
        base.Release();

        Api.Shutdown((result) => { Viveport.Core.Logger.Log("ShutdownHandler: " + result); });
    }

    public override void Loop()
    {
        if (!isInitialized)
            return;

        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].Loop();
    }

    private void OnCompleteCallback(bool isInitialized)
    {
        this.isInitialized = isInitialized;

        if (isInitialized)
        {
            StoreScripts.Add(new ViveportUserScript(StoreScriptId.User, new StoreUserModel()));
            StoreScripts.Add(new ViveportAchievementScript(StoreScriptId.Achievement, new StoreModel()));
            //StoreScripts.Add(new LocalRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
            StoreScripts.Add(new ViveportRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
            ViveportLeaderboardScript leaderboardscript = new ViveportLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel());
            StoreScripts.Add(leaderboardscript);

            for (int i = 0; i < StoreScripts.Count; i++)
                StoreScripts[i].Init();

            leaderboardscript.userID = GetScript(StoreScriptId.User).GetModel<StoreUserModel>().Name;
        }

        if (OnComplete != null)
            OnComplete(this.isInitialized);
    }
}
