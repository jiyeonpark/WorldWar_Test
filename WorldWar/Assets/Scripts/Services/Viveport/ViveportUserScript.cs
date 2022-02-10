using UnityEngine;
using Viveport;

public class ViveportUserScript : StoreScript
{
    public ViveportUserScript(StoreScriptId Id, object model) : base(Id, model) { }

    private WWW downloadAvatar;

    private StoreUserModel GetModel()
    {
        return GetModel<StoreUserModel>();
    }

    public override void Init()
    {
        GetModel().AppID = ViveportStore.APP_ID;
        GetModel().ID = User.GetUserId();
        GetModel().Name = User.GetUserName();
        GetModel().ServerID = GetModel().ID + ":" + (int)StoreTarget.Viveport;

        if(GameManager.IsCreated())
            GameManager.Instance.userid = GetModel().ID;

        string avatarurl = User.GetUserAvatarUrl();
        if (avatarurl != "")
            downloadAvatar = new WWW(avatarurl);
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

        if (downloadAvatar.isDone)
        {
            GetModel().Avatar = downloadAvatar.texture;
            downloadAvatar = null;
        }
    }
}
