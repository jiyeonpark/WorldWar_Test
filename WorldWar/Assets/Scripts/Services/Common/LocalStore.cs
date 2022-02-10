using System;
using System.Collections;

public class LocalStore : Store
{
    public LocalStore(StoreTarget target) : base(target) { }

    public override IEnumerator Init(Action<bool> OnComplete)
    {
        StoreScripts.Add(new LocalUserScript(StoreScriptId.User, new StoreUserModel()));
        StoreScripts.Add(new LocalAchievementScript(StoreScriptId.Achievement, new StoreModel()));
        StoreScripts.Add(new LocalRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
        StoreScripts.Add(new LocalLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel()));

        for (int i = 0; i < StoreScripts.Count; i++)
        {
            StoreScripts[i].Init();
        }

        isInitialized = true;

        if (OnComplete != null)
            OnComplete(isInitialized);

        yield return null;
    }

    //public override void Init(Action<bool> OnComplete)
    //{
    //    StoreScripts.Add(new LocalUserScript(StoreScriptId.User, new StoreUserModel()));
    //    StoreScripts.Add(new LocalAchievementScript(StoreScriptId.Achievement, new StoreModel()));
    //    StoreScripts.Add(new LocalRemoteStorageScript(StoreScriptId.RemoteStorage, new StoreRemoteStorageModel()));
    //    StoreScripts.Add(new LocalLeaderboardScript(StoreScriptId.Leaderboard, new StoreModel()));

    //    for (int i = 0; i < StoreScripts.Count; i++)
    //    {
    //        StoreScripts[i].Init();
    //    }

    //    isInitialized = true;

    //    if (OnComplete != null)
    //        OnComplete(isInitialized);
    //}
}
