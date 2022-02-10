using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;

public class OculusUserScript : StoreScript
{
    public OculusUserScript(StoreScriptId Id, object model) : base(Id, model) { }

    private WWW downloadAvatar;

    private StoreUserModel GetModel()
    {
        return GetModel<StoreUserModel>();
    }

    public override void Init()
    {
        downloadAvatar = null;

        Users.GetLoggedInUser().OnComplete((msg) =>
        {
            if (msg.IsError)
            {
                Debug.Log(msg.GetError().Message);
                return;
            }

            User user = msg.GetUser();

            GetModel().AppID = PlatformSettings.AppID;
            GetModel().IsLogin = (user.PresenceStatus == UserPresenceStatus.Online);
            GetModel().ID = user.ID.ToString();
            GetModel().Name = user.OculusID;
            GetModel().ServerID = GetModel().ID + ":" + (int)StoreTarget.Oculus;

            downloadAvatar = new WWW(user.ImageURL);
        });

        Users.GetUserProof().OnComplete((msg) =>
        {
            if (msg.IsError)
            {
                Debug.Log(msg.GetError().Message);
                return;
            }

            UserProof userProof = msg.GetUserProof();
            GetModel().AuthTicket = userProof.Value;
        });
    }

    public override void Loop()
    {
        DownloadAvatar();
    }

    private void DownloadAvatar()
    {
        if (downloadAvatar == null)
            return;

        if (downloadAvatar.error != null)
        {
            Debug.Log("downloadAvatar.error : " + downloadAvatar.error);
            downloadAvatar = null;
            return;
        }

        if(downloadAvatar.isDone)
        {
            GetModel().Avatar = downloadAvatar.texture;
            downloadAvatar = null;
        }
    }
}
