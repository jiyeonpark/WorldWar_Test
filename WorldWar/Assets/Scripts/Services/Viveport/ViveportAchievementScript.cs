using Viveport;

public class ViveportAchievementScript : LocalAchievementScript
{
    public ViveportAchievementScript(StoreScriptId Id, object model) : base(Id, model) { }

    protected override void ResetAchievements()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            UserStats.SetStat(achievements[i].statID, 0);
            UserStats.ClearAchievement(achievements[i].achievementID.ToString());
        }

        UserStats.UploadStats((result) => { });
    }

    protected override bool RequestAchievements()
    {
        UserStats.DownloadStats((result) => {
            if (result == 0)
                IsStatsValid = true;
        });

        int defaultValue = 0;

        for (int i = 0; i < achievements.Count; i++)
        {
            achievements[i].currentCount = UserStats.GetStat(achievements[i].statID, defaultValue);
            achievements[i].achieved = UserStats.GetAchievement(achievements[i].achievementID.ToString());
        }

        return true;
    }

    protected override void UnlockAchievement(Achievement achievement)
    {
        base.UnlockAchievement(achievement);

        UserStats.SetAchievement(achievement.achievementID.ToString());
    }

    protected override bool StoreStat(Achievement achievement)
    {
        UserStats.SetStat(achievement.statID, achievement.currentCount);
        UserStats.UploadStats((result) => { });

        return true;
    }
}
