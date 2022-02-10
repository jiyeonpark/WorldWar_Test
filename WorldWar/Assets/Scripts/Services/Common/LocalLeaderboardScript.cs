using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeaderboardID : int
{
    LB_01_ExtremeScoreOh1,
    LB_02_ExtremeScoreOh2,
    LB_03_ExtremeScoreOh3,
    LB_04_ExtremeScoreWe1,
    LB_05_ExtremeScoreWe2,
    LB_06_ExtremeScoreWe3,
};

public class LocalLeaderboardScript : StoreScript 
{
    public LocalLeaderboardScript(StoreScriptId Id, object model) : base(Id, model) { }

    private StoreModel GetModel()
    {
        return GetModel<StoreModel>();
    }

    public class Leaderboard
    {
        public LeaderboardID leaderboardID;
        public string name;
        public int score;
        public int rank;

        public Leaderboard(LeaderboardID leaderboard)
        {
            leaderboardID = leaderboard;
            name = "";
            score = 0;
            rank = 0;
        }

        public Leaderboard(LeaderboardID leaderboard, string id, int s, int r)
        {
            leaderboardID = leaderboard;
            name = id;
            score = s;
            rank = r;
        }
    }

    protected Dictionary<LeaderboardID, Leaderboard[]> leaderboards = new Dictionary<LeaderboardID, Leaderboard[]>();
    protected Dictionary<LeaderboardID, Leaderboard> leaderboard = new Dictionary<LeaderboardID, Leaderboard>();

    protected bool IsStatsValid;
    protected LeaderboardID curLeaderboardID = LeaderboardID.LB_01_ExtremeScoreOh1;
    protected const int TOP_N_COUNT = 10;
    public string userID = "";

    private string leaderboardScore = "0";

    public override void Init()
    {
        IsStatsValid = false;

        AddLeaderboard();
    }

    public override void Delete()
    {

    }

    protected void AddLeaderboard()
    {
        leaderboards.Add(LeaderboardID.LB_01_ExtremeScoreOh1, new Leaderboard[TOP_N_COUNT]);
        leaderboards.Add(LeaderboardID.LB_02_ExtremeScoreOh2, new Leaderboard[TOP_N_COUNT]);
        leaderboards.Add(LeaderboardID.LB_03_ExtremeScoreOh3, new Leaderboard[TOP_N_COUNT]);
        leaderboards.Add(LeaderboardID.LB_04_ExtremeScoreWe1, new Leaderboard[TOP_N_COUNT]);
        leaderboards.Add(LeaderboardID.LB_05_ExtremeScoreWe2, new Leaderboard[TOP_N_COUNT]);
        leaderboards.Add(LeaderboardID.LB_06_ExtremeScoreWe3, new Leaderboard[TOP_N_COUNT]);

        leaderboard.Add(LeaderboardID.LB_01_ExtremeScoreOh1, new Leaderboard(LeaderboardID.LB_01_ExtremeScoreOh1));
        leaderboard.Add(LeaderboardID.LB_02_ExtremeScoreOh2, new Leaderboard(LeaderboardID.LB_02_ExtremeScoreOh2));
        leaderboard.Add(LeaderboardID.LB_03_ExtremeScoreOh3, new Leaderboard(LeaderboardID.LB_03_ExtremeScoreOh3));
        leaderboard.Add(LeaderboardID.LB_04_ExtremeScoreWe1, new Leaderboard(LeaderboardID.LB_04_ExtremeScoreWe1));
        leaderboard.Add(LeaderboardID.LB_05_ExtremeScoreWe2, new Leaderboard(LeaderboardID.LB_05_ExtremeScoreWe2));
        leaderboard.Add(LeaderboardID.LB_06_ExtremeScoreWe3, new Leaderboard(LeaderboardID.LB_06_ExtremeScoreWe3));
    }

    public override void RenderOnGUI()
    {
        int nWidth = 200, nHeight = 40;
        int nXStart = 10, nYStart = 35;
        GUIStyle CustButton = new GUIStyle("button");

        leaderboardScore = GUI.TextField(new Rect(nXStart, nYStart, 160, 20), leaderboardScore, 50);

        if (GUI.Button(new Rect(nXStart, nYStart + 1 * (nHeight + 10), nWidth, nHeight), "LB_01_ExtremeScoreOh1", CustButton))
        {
            OnGameLeaderboardChange(LeaderboardID.LB_01_ExtremeScoreOh1, int.Parse(leaderboardScore));
        }
        if (GUI.Button(new Rect(nXStart, nYStart + 2 * (nHeight + 10), nWidth, nHeight), "LB_02_ExtremeScoreOh2", CustButton))
        {
            OnGameLeaderboardChange(LeaderboardID.LB_02_ExtremeScoreOh2, int.Parse(leaderboardScore));
        }
        if (GUI.Button(new Rect(nXStart, nYStart + 3 * (nHeight + 10), nWidth, nHeight), "LB_03_ExtremeScoreOh3", CustButton))
        {
            OnGameLeaderboardChange(LeaderboardID.LB_03_ExtremeScoreOh3, int.Parse(leaderboardScore));
        }

        if (GUI.Button(new Rect(nXStart, nYStart + 4 * (nHeight + 10), nWidth, nHeight), "DownloadLeaderboard", CustButton))
        {
            DownloadLeaderboardEntries();
        }
        if (GUI.Button(new Rect(nXStart, nYStart + 5 * (nHeight + 10), nWidth, nHeight), "GetLeaderboards", CustButton))
        {
            Leaderboard[] lblist = GetLeaderboards(curLeaderboardID);
            Debug.Log("*****" + curLeaderboardID.ToString() + "*****");
            for (int i = 0; i < lblist.Length; i++)
                Debug.Log("Rank:" + lblist[i].rank.ToString() + " Name:" + lblist[i].name + " Score:" + lblist[i].score.ToString());
            Leaderboard lb = GetLeaderboard(curLeaderboardID);
            Debug.Log("My Rank:" + lb.rank.ToString() + " Name:" + lb.name + " Score:" + lb.score.ToString());
        }
    }

    public void OnGameLeaderboardChange(LeaderboardID leaderboardID, int score)
    {
        if (!IsStatsValid)
            return;

        Leaderboard leaderboard = GetLeaderboard(leaderboardID);
        if (leaderboard == null)
            return;

        curLeaderboardID = leaderboardID;
        leaderboard.score = score;
        Debug.Log("Clear Leaderboard " + leaderboardID.ToString());
        CheckUpdateLeaderboard(leaderboardID, score);
    }

    public Leaderboard[] GetLeaderboards(LeaderboardID leaderboardID)
    {
        for (int i = 0; i < leaderboards[leaderboardID].Length; i++)
            if (leaderboards[leaderboardID][i] == null)
                leaderboards[leaderboardID][i] = new Leaderboard(leaderboardID);

        return leaderboards[leaderboardID];
    }

    public Leaderboard GetLeaderboard(LeaderboardID leaderboardID)
    {
        return leaderboard[leaderboardID];
    }

    public Leaderboard GetLeaderboard(LeaderboardID leaderboardID, int rank)
    {
        if (rank >= TOP_N_COUNT)
            return null;
        if (leaderboards[leaderboardID][rank] == null)
            leaderboards[leaderboardID][rank] = new Leaderboard(curLeaderboardID);

        return leaderboards[leaderboardID][rank];
    }

    protected void CheckUpdateLeaderboard(LeaderboardID leaderboardID, int score)
    {
        //성공
        UploadLeaderboardScore(leaderboardID, score);
    }

    protected virtual void FindLeaderboard(string pchLeaderboardName) { }
    protected virtual void DownloadLeaderboardEntries() { }
    protected virtual void UploadLeaderboardScore(LeaderboardID leaderboardID, int score) { }
}
