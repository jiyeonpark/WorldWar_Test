using BattleofRedCliffsVRService.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public partial class LobbyServer
{
    public static Action<List<StageViewModel>> ActionGetStages;
    public static Action<StageViewModel> ActionGetStage;
    public static Action<StageViewModel> ActionUpdateStage;
    public static Action<bool> ActionDeleteStage;

    public void GetStages(string userId)
    {
        http.Get(GetURL("api/users/" + userId + "/stages"), (req, res) =>
        {
            List<StageViewModel> sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;

                sc = JsonConvert.DeserializeObject<List<StageViewModel>>(data);
            }

            if (ActionGetStages != null)
                ActionGetStages(sc);
        });
    }

    public void GetStage(string userId, string stageId)
    {
        http.Get(GetURL("api/users/" + userId + "/stages/" + stageId), (req, res) =>
        {
            StageViewModel sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;

                sc = JsonConvert.DeserializeObject<StageViewModel>(data);
            }

            if (ActionGetStage != null)
                ActionGetStage(sc);
        });
    }

    public void UpdateStage(string userId, string stageId, int stageState, int stageScore)
    {
        StageViewModel stage = new StageViewModel()
        {
            Id = stageId,
            State = stageState,
            Score = stageScore,
        };

        http.Post(GetURL("api/users/" + userId + "/stages"), stage, (req, res) =>
        {
            StageViewModel sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;

                sc = JsonConvert.DeserializeObject<StageViewModel>(data);
            }

            if (ActionUpdateStage != null)
                ActionUpdateStage(sc);
        });
    }

    public void DeleteStage(string userId, string stageId)
    {
        http.Delete(GetURL("api/users/" + userId + "/stages/" + stageId), (req, res) =>
        {
            if (ActionDeleteStage != null)
                ActionDeleteStage(res.IsSuccess);
        });
    }
}
