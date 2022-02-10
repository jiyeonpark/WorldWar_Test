using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public enum AchievementID : int
{
    ACH_00_FirstGame,
    ACH_01_CompleteOh1,
    ACH_02_CompleteOh2,
    ACH_03_CompleteOh3,
    ACH_04_CompleteWe1,
    ACH_05_CompleteWe2,
    ACH_06_CompleteWe3,
    ACH_07_CounterAttack,
    ACH_08_HeadShot,
    ACH_09_CompleteTuto,
    ACH_10_CompleteAllStageNoDeath,
    ACH_11_KillJanghapNoDamage,
    ACH_12_KillHeojeoNoDamage,
    ACH_13_KillHahudonNoDamage,
    ACH_14_KillJootaeNoDamage,
    ACH_15_KillTaesajaNoDamage,
    ACH_16_KillJuyuNoDamage,
};

public class LocalAchievementScript : StoreScript
{
    public LocalAchievementScript(StoreScriptId Id, object model) : base(Id, model) { }

    private StoreModel GetModel()
    {
        return GetModel<StoreModel>();
    }

    protected class Achievement
    {
        public AchievementID achievementID;
        public string name;
        public string description;
        public bool achieved;
        public string statID;
        public int targetCount;
        public int currentCount;

        public Achievement(AchievementID achievement, string stat, int target)
        {
            achievementID = achievement;
            name = "";
            description = "";
            achieved = false;
            statID = stat;
            targetCount = target;
            currentCount = 0;
        }
    }

    protected List<Achievement> achievements = new List<Achievement>();

    protected bool IsStatsValid;

    public override void Init()
    {
        IsStatsValid = false;

        AddAchievement();
        RequestAchievements();
    }

    public override void Delete()
    {
        ResetAchievements();
    }

    protected void AddAchievement()
    {
        achievements.Add(new Achievement(AchievementID.ACH_00_FirstGame, "STAT_00_FirstGame", 1));
        achievements.Add(new Achievement(AchievementID.ACH_01_CompleteOh1, "STAT_01_CompleteOh1", 1));
        achievements.Add(new Achievement(AchievementID.ACH_02_CompleteOh2, "STAT_02_CompleteOh2", 1));
        achievements.Add(new Achievement(AchievementID.ACH_03_CompleteOh3, "STAT_03_CompleteOh3", 1));
        achievements.Add(new Achievement(AchievementID.ACH_04_CompleteWe1, "STAT_04_CompleteWe1", 1));
        achievements.Add(new Achievement(AchievementID.ACH_05_CompleteWe2, "STAT_05_CompleteWe2", 1));
        achievements.Add(new Achievement(AchievementID.ACH_06_CompleteWe3, "STAT_06_CompleteWe3", 1));
        achievements.Add(new Achievement(AchievementID.ACH_07_CounterAttack, "STAT_07_CounterAttack", 100));
        achievements.Add(new Achievement(AchievementID.ACH_08_HeadShot, "STAT_08_HeadShot", 100));
        achievements.Add(new Achievement(AchievementID.ACH_09_CompleteTuto, "STAT_09_CompleteTuto", 1));
        achievements.Add(new Achievement(AchievementID.ACH_10_CompleteAllStageNoDeath, "STAT_10_CompleteAllStageNoDeath", 1));
        achievements.Add(new Achievement(AchievementID.ACH_11_KillJanghapNoDamage, "STAT_11_KillJanghapNoDamage", 1));
        achievements.Add(new Achievement(AchievementID.ACH_12_KillHeojeoNoDamage, "STAT_12_KillHeojeoNoDamage", 1));
        achievements.Add(new Achievement(AchievementID.ACH_13_KillHahudonNoDamage, "STAT_13_KillHahudonNoDamage", 1));
        achievements.Add(new Achievement(AchievementID.ACH_14_KillJootaeNoDamage, "STAT_14_KillJootaeNoDamage", 1));
        achievements.Add(new Achievement(AchievementID.ACH_15_KillTaesajaNoDamage, "STAT_15_KillTaesajaNoDamage", 1));
        achievements.Add(new Achievement(AchievementID.ACH_16_KillJuyuNoDamage, "STAT_16_KillJuyuNoDamage", 1));

    }

    Vector2 scrollPos;

    public override void RenderOnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        {
            foreach (Achievement achievement in achievements)
            {
                GUILayout.Label(achievement.achievementID.ToString());
                GUILayout.Label(achievement.name + " - " + achievement.description);
                GUILayout.Label("Achieved: " + achievement.achieved);
                GUILayout.Label("StatID: " + achievement.statID);
                GUILayout.Label(achievement.currentCount + " / " + achievement.targetCount);
                GUILayout.Space(20);
            }
        }
        GUILayout.EndScrollView();

        if (GUILayout.Button("ResetAchievements"))
        {
            Delete();
        }

        if (GUILayout.Button("OnGameStateChange ACH_00_FirstGame"))
        {
            OnGameStateChange(AchievementID.ACH_00_FirstGame);
        }

        if (GUILayout.Button("OnGameStateChange ACH_01_CompleteOh1"))
        {
            OnGameStateChange(AchievementID.ACH_01_CompleteOh1);
        }
    }

    public void OnGameStateChange(AchievementID achievementID)
    {
        if (!IsStatsValid)
            return;

        Achievement achievement = GetAchievement(achievementID);
        if (achievement == null)
            return;

        if (achievement.achieved)
            return; //이미 성공 함

        achievement.currentCount++;
        Debug.Log("Clear Achievement " + achievementID.ToString());
        CheckUnlockAchievement(achievement);
        StoreStat(achievement);
    }

    protected Achievement GetAchievement(AchievementID achievementID)
    {
        return achievements.Find(p => p.achievementID == achievementID);
    }

    protected Achievement GetAchievement(string achievementID)
    {
        return achievements.Find(p => p.achievementID.ToString() == achievementID);
    }

    protected void CheckUnlockAchievement(Achievement achievement)
    {
        if (achievement.currentCount >= achievement.targetCount)
        {   //성공
            UnlockAchievement(achievement);
        }
    }

    protected virtual void ResetAchievements() { }
    protected virtual bool RequestAchievements() { return false; }
    protected virtual bool StoreStat(Achievement achievement) { return false; }
    protected virtual void UnlockAchievement(Achievement achievement) { achievement.achieved = true; }
}
