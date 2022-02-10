using UnityEngine;
using System;
using Viveport;

public class ViveportLeaderboardScript : LocalLeaderboardScript 
{
    public ViveportLeaderboardScript(StoreScriptId Id, object model) : base(Id, model) { }

    private bool bIsReady = false, downloading = false;
    
    public override void Init()
    {
        base.Init();

        bIsReady = true;
        IsStatsValid = true;
    }
    
    public override void Loop()
    {
        if (downloading == true)
        {
            downloading = false;
            FindLeaderboard(curLeaderboardID.ToString());
        }
    }

    protected override void FindLeaderboard(string pchLeaderboardName)
    {
        if (bIsReady == true)
        {
            int nResult = (int)UserStats.GetLeaderboardScoreCount();

            Viveport.Core.Logger.Log("GetLeaderboardScoreCount => " + nResult);

            for (int i = 0; i < nResult; i++)
            {
                Viveport.Leaderboard lbdata;
                lbdata = UserStats.GetLeaderboardScore(i);
                Viveport.Core.Logger.Log("UserName = " + lbdata.UserName + ", Score = " + lbdata.Score + ", Rank = " + lbdata.Rank);

                // 랭킹이 0부터 시작함...
                if (lbdata.Rank < TOP_N_COUNT)
                {
                    Leaderboard lb = GetLeaderboard(curLeaderboardID, lbdata.Rank);
                    lb.leaderboardID = curLeaderboardID;
                    lb.name = lbdata.UserName;
                    lb.score = lbdata.Score;
                    lb.rank = lbdata.Rank;
                }

                if (lbdata.UserName == userID)
                {
                    Leaderboard lb = GetLeaderboard(curLeaderboardID);
                    lb.leaderboardID = curLeaderboardID;
                    lb.name = lbdata.UserName;
                    lb.score = lbdata.Score;
                    lb.rank = lbdata.Rank;
                }
            }
        }
        else
            Viveport.Core.Logger.Log("Make sure init is successful.");
    }

    protected override void DownloadLeaderboardEntries()
    {
        if (bIsReady == true)
        {
            int value = UserStats.DownloadLeaderboardScores(DownloadLeaderboardHandler, curLeaderboardID.ToString(), UserStats.LeaderBoardRequestType.GlobalData, UserStats.LeaderBoardTimeRange.AllTime, 0, 10);
            Viveport.Core.Logger.Log("DownloadLeaderboardScores : " + value.ToString());
        }
        else
            Viveport.Core.Logger.Log("Make sure init is successful.");
    }

    protected override void UploadLeaderboardScore(LeaderboardID leaderboardID, int score)
    {
        if (bIsReady == true)
        {
            UserStats.UploadLeaderboardScore(UploadLeaderboardScoreHandler, leaderboardID.ToString(), score);
            Viveport.Core.Logger.Log("UploadLeaderboardScore : " + score.ToString());
        }
        else
            Viveport.Core.Logger.Log("Make sure init is successful.");
    }

    private void DownloadLeaderboardHandler(int nResult)
    {
        if (nResult == 0)
        {
            downloading = true;
            Viveport.Core.Logger.Log("DownloadLeaderboardHandler is successful");
        }
        else
            Viveport.Core.Logger.Log("DownloadLeaderboardHandler error: " + nResult);
    }

    private void UploadLeaderboardScoreHandler(int nResult)
    {
        if (nResult == 0)
            Viveport.Core.Logger.Log("UploadLeaderboardScoreHandler is successful.");
        else
            Viveport.Core.Logger.Log("UploadLeaderboardScoreHandler error : " + nResult);
    }

}
