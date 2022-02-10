using Oculus.Platform;
using System;
using System.Collections;
using UnityEngine;

public class OculusStore : Store
{
    public OculusStore(StoreTarget target) : base(target) { }

    public override IEnumerator Init(Action<bool> OnComplete)
    {
        Core.Initialize();
        Entitlements.IsUserEntitledToApplication().OnComplete((msg) =>
        {
            isInitialized = !msg.IsError;

            if (isInitialized)
            {   //성공
                StoreScripts.Add(new OculusUserScript(StoreScriptId.User, new StoreUserModel()));
                StoreScripts.Add(new OculusAchievementScript(StoreScriptId.Achievement, new StoreModel()));
                StoreScripts.Add(new OculusRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
                StoreScripts.Add(new OculusLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel()));

                for (int i = 0; i < StoreScripts.Count; i++)
                    StoreScripts[i].Init();
            }
        });

        yield return new WaitForSeconds(2f);

        if (OnComplete != null)
            OnComplete(isInitialized);
    }

    //public override void Init(Action<bool> OnComplete)
    //{
    //    Core.Initialize();
    //    Entitlements.IsUserEntitledToApplication().OnComplete((msg) =>
    //    {
    //        isInitialized = !msg.IsError;

    //        if (isInitialized)
    //        {   //성공
    //            StoreScripts.Add(new OculusUserScript(StoreScriptId.User, new StoreUserModel()));
    //            StoreScripts.Add(new OculusAchievementScript(StoreScriptId.Achievement, new StoreModel()));
    //            StoreScripts.Add(new OculusRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
    //            StoreScripts.Add(new OculusLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel()));

    //            for (int i = 0; i < StoreScripts.Count; i++)
    //                StoreScripts[i].Init();
    //        }

    //        if (OnComplete != null)
    //            OnComplete(isInitialized);
    //    });
    //}

    public override void Loop()
    {
        if (!isInitialized)
            return;

        Request.RunCallbacks();

        for (int i = 0; i < StoreScripts.Count; i++)
            StoreScripts[i].Loop();
    }
}
