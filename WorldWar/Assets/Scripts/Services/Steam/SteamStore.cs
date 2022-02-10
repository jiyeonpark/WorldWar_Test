using Steamworks;
using System;
using System.Collections;
using UnityEngine;

public class SteamStore : Store
{
    public SteamStore(StoreTarget target) : base(target) { }

    SteamAPIWarningMessageHook_t SteamAPIWarningMessageHook;
    static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
    {
        Debug.LogWarning(pchDebugText);
    }

    private bool CheckSystem()
    {
        if (!Packsize.Test())
        {
            Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
            return false;
        }

        if (!DllCheck.Test())
        {
            Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
            return false;
        }

        try
        {
            if (SteamAPI.RestartAppIfNecessary((AppId_t)716400))
                return false;
        }
        catch (DllNotFoundException e)
        {
            Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e);
            return false;
        }

        return true;
    }

    public override IEnumerator Init(Action<bool> OnComplete)
    {
        bool isCheckSystem = CheckSystem();

        if (isCheckSystem)
        {
            isInitialized = SteamAPI.Init();

            if (isInitialized)
            {   //성공
                SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);

                StoreScripts.Add(new SteamUserScript(StoreScriptId.User, new StoreUserModel()));
                StoreScripts.Add(new SteamAchievementScript(StoreScriptId.Achievement, new StoreModel()));
                StoreScripts.Add(new SteamRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
                SteamLeaderboardScript leaderboardscript = new SteamLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel());
                StoreScripts.Add(leaderboardscript);

                for (int i = 0; i < StoreScripts.Count; i++)
                    StoreScripts[i].Init();

                leaderboardscript.userID = GetScript(StoreScriptId.User).GetModel<StoreUserModel>().Name;
            }
        }

        yield return new WaitForSeconds(2f);

        if (OnComplete != null)
            OnComplete(isInitialized);
    }

    //public override void Init(Action<bool> OnComplete)
    //{
    //    bool isCheckSystem = CheckSystem();

    //    if (isCheckSystem)
    //    {
    //        isInitialized = SteamAPI.Init();

    //        if (isInitialized)
    //        {   //성공
    //            SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
    //            SteamClient.SetWarningMessageHook(SteamAPIWarningMessageHook);

    //            StoreScripts.Add(new SteamUserScript(StoreScriptId.User, new StoreUserModel()));
    //            StoreScripts.Add(new SteamAchievementScript(StoreScriptId.Achievement, new StoreModel()));
    //            StoreScripts.Add(new SteamRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
    //            SteamLeaderboardScript leaderboardscript = new SteamLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel());
    //            StoreScripts.Add(leaderboardscript);

    //            for (int i = 0; i < StoreScripts.Count; i++)
    //                StoreScripts[i].Init();

    //            leaderboardscript.userID = GetScript(StoreScriptId.User).GetModel<StoreUserModel>().Name;
    //        }
    //    }

    //    if (OnComplete != null)
    //        OnComplete(isInitialized);
    //}

    public override void Release()
    {
        base.Release();

        SteamAPI.Shutdown();
    }

    public override void Loop()
    {
        if (!isInitialized)
            return;

        SteamAPI.RunCallbacks();

        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].Loop();
    }
}
