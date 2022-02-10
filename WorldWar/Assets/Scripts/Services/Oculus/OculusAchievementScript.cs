using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using UnityEngine;

public class OculusAchievementScript : LocalAchievementScript
{
    public OculusAchievementScript(StoreScriptId Id, object model) : base(Id, model) { }

    protected override bool RequestAchievements()
    {
        Achievements.GetAllDefinitions().OnComplete(OnAchievementDefinitionCallback);
        Achievements.GetAllProgress().OnComplete(OnAchievementProgressCallback);

        return true;
    }

    protected override void UnlockAchievement(Achievement achievement)
    {
        base.UnlockAchievement(achievement);

        Achievements.Unlock(achievement.achievementID.ToString()).OnComplete(OnAchievementUnlockCallback);
    }

    protected override bool StoreStat(Achievement achievement)
    {
        Achievements.AddCount(achievement.achievementID.ToString(), Convert.ToUInt64(achievement.currentCount)).OnComplete(OnAchievementCountCallback);

        return true;
    }

    private void OnAchievementDefinitionCallback(Message<AchievementDefinitionList> msg)
    {
        if (msg.IsError)
        {
            Debug.Log(msg.GetError().Message);
            return;
        }

        AchievementDefinitionList definitionList = msg.GetAchievementDefinitions();
        for (int i = 0; i < definitionList.Count; i++)
        {
            AchievementDefinition definition = definitionList[i];
            Achievement achievement = GetAchievement(definition.Name);
            if (achievement == null)
                continue;

            achievement.targetCount = (int)definition.Target;
        }

        IsStatsValid = true;
    }

    private void OnAchievementProgressCallback(Message<AchievementProgressList> msg)
    {
        if (msg.IsError)
        {
            Debug.Log(msg.GetError().Message);
            return;
        }

        AchievementProgressList progressList = msg.GetAchievementProgressList();
        for (int i = 0; i < progressList.Count; i++)
        {
            AchievementProgress progress = progressList[i];
            Achievement achievement = GetAchievement(progress.Name);
            if (achievement == null)
                continue;

            achievement.achieved = progress.IsUnlocked;
            achievement.currentCount = (int)progress.Count;
        }

        IsStatsValid = true;
    }

    private void OnAchievementUnlockCallback(Message msg)
    {
        if (!msg.IsError)
        {
            Debug.Log("Achievement unlocked");
        }
        else
        {
            Debug.Log("Received achievement unlock error : " + msg.GetError());
        }
    }

    private void OnAchievementCountCallback(Message msg)
    {
        if (!msg.IsError)
        {
            Debug.Log("Achievement count added");
        }
        else
        {
            Debug.Log("Received achievement count error : " + msg.GetError());
        }
    }
}
