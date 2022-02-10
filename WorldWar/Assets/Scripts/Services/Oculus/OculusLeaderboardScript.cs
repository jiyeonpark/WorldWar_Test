using UnityEngine;
using System.Collections.Generic;
using Oculus.Platform;
using Oculus.Platform.Models;

public class OculusLeaderboardScript : LocalLeaderboardScript
{
    public OculusLeaderboardScript(StoreScriptId Id, object model) : base(Id, model) { }

    public override void Init()
    {
        base.Init();

        IsStatsValid = true;
    }

    void MostWinsGetEntriesCallback(Message<LeaderboardEntryList> msg)
    {
        if (!msg.IsError)
        {
            if (userID == "")
                userID = StoreManager.sInstance.GetScript(StoreScriptId.User).GetModel<StoreUserModel>().Name;
            Debug.Log("Download leaderboardEntries is successful : " + msg.Type.ToString() + " count : " + msg.Data.Count.ToString() + " myname : " + userID);
            foreach (LeaderboardEntry entry in msg.Data)
            {
                // 랭킹이 1부터 시작함...
                if (entry.Rank < TOP_N_COUNT)
                {
                    Leaderboard lb = GetLeaderboard(curLeaderboardID, entry.Rank -1);
                    lb.leaderboardID = curLeaderboardID;
                    lb.name = entry.User.OculusID;
                    lb.score = (int)entry.Score;
                    lb.rank = entry.Rank;
                }

                if (entry.User.OculusID.ToString() == userID)
                {
                    Leaderboard lb = GetLeaderboard(curLeaderboardID);
                    lb.leaderboardID = curLeaderboardID;
                    lb.name = entry.User.OculusID;
                    lb.score = (int)entry.Score;
                    lb.rank = entry.Rank;
                }
            }
        }
        else
            Debug.Log("Not download leaderboardEntries! : " + msg.GetError().Message);
    }

    void UploadLeaderboardScoreCallback(Message<bool> msg)
    {
        Debug.Log(msg.ToString());
    }

    protected override void FindLeaderboard(string pchLeaderboardName)
    {
        Leaderboards.GetEntries(pchLeaderboardName, TOP_N_COUNT, LeaderboardFilterType.None,
            LeaderboardStartAt.Top).OnComplete(MostWinsGetEntriesCallback);
    }

    protected override void DownloadLeaderboardEntries()
    {
        FindLeaderboard(curLeaderboardID.ToString());
    }

    protected override void UploadLeaderboardScore(LeaderboardID leaderboardID, int score)
    {
        if (score > 0)
        {
            Leaderboards.WriteEntry(leaderboardID.ToString(), score).OnComplete(UploadLeaderboardScoreCallback);
            Debug.Log("UploadLeaderboardScore Call : " + leaderboardID.ToString() + " score : " + score.ToString());
        }
    }
}
