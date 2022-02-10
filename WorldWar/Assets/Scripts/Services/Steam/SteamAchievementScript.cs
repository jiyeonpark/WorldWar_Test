using Steamworks;
using UnityEngine;

public class SteamAchievementScript : LocalAchievementScript
{
    public SteamAchievementScript(StoreScriptId Id, object model) : base(Id, model) { }

    private CGameID gameID;

    protected Callback<UserStatsReceived_t> userStatsReceived;
    protected Callback<UserStatsStored_t> userStatsStored;
    protected Callback<UserAchievementStored_t> userAchievementStored;

    public override void Init()
    {
        base.Init();

        gameID = new CGameID(SteamUtils.GetAppID());
        userStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        userStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        userAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
    }

    protected override void ResetAchievements()
    {
        SteamUserStats.ResetAllStats(true);
        SteamUserStats.RequestCurrentStats();
    }

    protected override bool RequestAchievements()
    {
        return SteamUserStats.RequestCurrentStats();
    }

    protected override void UnlockAchievement(Achievement achievement)
    {
        base.UnlockAchievement(achievement);

        SteamUserStats.SetAchievement(achievement.achievementID.ToString());
    }

    protected override bool StoreStat(Achievement achievement)
    {
        SteamUserStats.SetStat(achievement.statID, achievement.currentCount);
        return SteamUserStats.StoreStats();
    }

    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if ((ulong)gameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                IsStatsValid = true;

                for (int i = 0; i < achievements.Count; i++)
                {
                    Achievement achievement = achievements[i];

                    bool result = SteamUserStats.GetAchievement(achievement.achievementID.ToString(), out achievement.achieved);
                    if (result)
                    {
                        achievement.name = SteamUserStats.GetAchievementDisplayAttribute(achievement.achievementID.ToString(), "name");
                        achievement.description = SteamUserStats.GetAchievementDisplayAttribute(achievement.achievementID.ToString(), "desc");

                        SteamUserStats.GetStat(achievement.statID, out achievement.currentCount);
                    }
                    else
                    {
                        Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement.achievementID + "\nIs it registered in the Steam Partner site?");
                    }
                }
            }
            else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        if ((ulong)gameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - success");
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)gameID;
                OnUserStatsReceived(callback);
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - some failed to validate");
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)gameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        if ((ulong)gameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }
}
