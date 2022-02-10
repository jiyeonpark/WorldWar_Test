using UnityEngine;
using Steamworks;

public class SteamLeaderboardScript : LocalLeaderboardScript
{
    public SteamLeaderboardScript(StoreScriptId Id, object model) : base(Id, model) { }

    private SteamLeaderboard_t m_SteamLeaderboard;

    private CallResult<LeaderboardFindResult_t> LeaderboardFindResult;
    private CallResult<LeaderboardScoresDownloaded_t> LeaderboardScoresDownloaded;
    private CallResult<LeaderboardScoreUploaded_t> LeaderboardScoreUploaded;

    public override void Init()
    {
        base.Init();

        LeaderboardFindResult = CallResult<LeaderboardFindResult_t>.Create(OnLeaderboardFindResult);
        LeaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
        LeaderboardScoreUploaded = CallResult<LeaderboardScoreUploaded_t>.Create(OnLeaderboardScoreUploaded);

        IsStatsValid = true;
    }

    //public override void RenderOnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(Screen.width - 200, 0, 200, Screen.height));
    //    GUILayout.Label("m_SteamLeaderboard: " + m_SteamLeaderboard);

    //    if (m_SteamLeaderboard != new SteamLeaderboard_t(0))
    //    {
    //        GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardName(m_SteamLeaderboard));
    //        GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardEntryCount(m_SteamLeaderboard));
    //        GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardSortMethod(m_SteamLeaderboard));
    //        GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : " + SteamUserStats.GetLeaderboardDisplayType(m_SteamLeaderboard));
    //    }
    //    else
    //    {
    //        GUILayout.Label("GetLeaderboardName(m_SteamLeaderboard) : ");
    //        GUILayout.Label("GetLeaderboardEntryCount(m_SteamLeaderboard) : ");
    //        GUILayout.Label("GetLeaderboardSortMethod(m_SteamLeaderboard) : ");
    //        GUILayout.Label("GetLeaderboardDisplayType(m_SteamLeaderboard) : ");
    //    }

    //    GUILayout.EndArea();

    //    if (GUILayout.Button("FindLeaderboard"))
    //    {
    //        FindLeaderboard("Feet Traveled");
    //    }

    //    if (GUILayout.Button("DownloadLeaderboardEntries"))
    //    {
    //        DownloadLeaderboardEntries();
    //    }

    //    if (GUILayout.Button("UploadLeaderboardScore"))
    //    {
    //        UploadLeaderboardScore(LeaderboardID.LB_01_ExtremeScoreOh1, 10000);
    //    }
    //}

    protected override void FindLeaderboard(string pchLeaderboardName)
    {
        SteamAPICall_t handle = SteamUserStats.FindLeaderboard(pchLeaderboardName);
        LeaderboardFindResult.Set(handle);
    }

    protected override void DownloadLeaderboardEntries()
    {
        //FindLeaderboard(curLeaderboardID.ToString());
        SteamAPICall_t handle = SteamUserStats.DownloadLeaderboardEntries(m_SteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobalAroundUser, -4, 5);
        LeaderboardScoresDownloaded.Set(handle);
    }

    protected override void UploadLeaderboardScore(LeaderboardID leaderboardID, int score)
    {
        //FindLeaderboard(leaderboardID.ToString());
        SteamAPICall_t handle = SteamUserStats.UploadLeaderboardScore(m_SteamLeaderboard, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, score, null, 0);
        LeaderboardScoreUploaded.Set(handle);
    }

    private void OnLeaderboardFindResult(LeaderboardFindResult_t pCallback, bool bIOFailure)
    {
        Debug.Log("[" + LeaderboardFindResult_t.k_iCallback + " - LeaderboardFindResult] - " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_bLeaderboardFound);

        if (pCallback.m_bLeaderboardFound != 0)
        {
            m_SteamLeaderboard = pCallback.m_hSteamLeaderboard;
        }
    }

    private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
    {
        Debug.Log("[" + LeaderboardScoresDownloaded_t.k_iCallback + " - LeaderboardScoresDownloaded] - " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_hSteamLeaderboardEntries + " -- " + pCallback.m_cEntryCount);

        for (int i = 0; i < pCallback.m_cEntryCount; i++)
        {
            LeaderboardEntry_t LeaderboardEntry;
            bool ret = SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out LeaderboardEntry, null, 0);

            string Name = SteamFriends.GetFriendPersonaName(LeaderboardEntry.m_steamIDUser);
            string NickName = SteamFriends.GetPlayerNickname(LeaderboardEntry.m_steamIDUser);

            // 랭킹이 1부터 시작함...
            if (LeaderboardEntry.m_nGlobalRank < TOP_N_COUNT)
            {
                Leaderboard lb = GetLeaderboard(curLeaderboardID, LeaderboardEntry.m_nGlobalRank -1);
                lb.leaderboardID = curLeaderboardID;
                lb.name = Name;
                lb.score = LeaderboardEntry.m_nScore;
                lb.rank = LeaderboardEntry.m_nGlobalRank;
            }

            if (Name == userID)
            {
                Leaderboard lb = GetLeaderboard(curLeaderboardID);
                lb.leaderboardID = curLeaderboardID;
                lb.name = Name;
                lb.score = LeaderboardEntry.m_nScore;
                lb.rank = LeaderboardEntry.m_nGlobalRank;
            }

            Debug.Log("[" + ret + " - GetDownloadedLeaderboardEntry] - " + Name + " -- " + NickName + " -- " + LeaderboardEntry.m_steamIDUser + " -- " + LeaderboardEntry.m_nScore + " -- " + LeaderboardEntry.m_nGlobalRank);
        }
    }

    private void OnLeaderboardScoreUploaded(LeaderboardScoreUploaded_t pCallback, bool bIOFailure)
    {
        Debug.Log("[" + LeaderboardScoreUploaded_t.k_iCallback + " - LeaderboardScoreUploaded] - " + pCallback.m_bSuccess + " -- " + pCallback.m_hSteamLeaderboard + " -- " + pCallback.m_nScore + " -- " + pCallback.m_bScoreChanged + " -- " + pCallback.m_nGlobalRankNew + " -- " + pCallback.m_nGlobalRankPrevious);
    }
}
