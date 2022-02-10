using BattleofRedCliffsVRService.ViewModels;
using Newtonsoft.Json;
using System;

public partial class LobbyServer
{
    public static Action<UserViewModel> ActionGetUser;
    public static Action<UserViewModel> ActionCreateUser;
    public static Action<UserViewModel> ActionUpdateUser;
    public static Action<bool> ActionDeleteUser;

    public void GetUser(string Id)
    {
        http.Get(GetURL("api/users/" + Id), (req, res) =>
        {
            if (req.State == BestHTTP.HTTPRequestStates.Error)
            {
                GameManager.Instance.ServerError(req.Exception.Message);
            }
            UserViewModel sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;
                
                sc = JsonConvert.DeserializeObject<UserViewModel>(data);
            }

            if (ActionGetUser != null)
                ActionGetUser(sc);
        });
    }

    public void CreateUser(string Id, string NickName)
    {
        UserViewModel user = new UserViewModel()
        {
            Id = Id,
            NickName = NickName,
        };

        http.Post(GetURL("api/users"), user, (req, res) =>
        {
            UserViewModel sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;

                sc = JsonConvert.DeserializeObject<UserViewModel>(data);
            }

            if (ActionCreateUser != null)
                ActionCreateUser(sc);
        });
    }

    public void UpdateUser(string Id, string NickName)
    {
        UserViewModel user = new UserViewModel()
        {
            Id = Id,
            NickName = NickName,
        };

        http.Put(GetURL("api/users/" + Id), user, (req, res) =>
        {
            UserViewModel sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;

                sc = JsonConvert.DeserializeObject<UserViewModel>(data);
            }

            if (ActionUpdateUser != null)
                ActionUpdateUser(sc);
        });
    }

    public void DeleteUser(string Id)
    {
        http.Delete(GetURL("api/users/" + Id), (req, res) =>
        {
            if (ActionDeleteUser != null)
                ActionDeleteUser(res.IsSuccess);
        });
    }
}
