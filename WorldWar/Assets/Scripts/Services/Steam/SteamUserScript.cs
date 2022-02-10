using UnityEngine;
using Steamworks;

public class SteamUserScript : StoreScript
{
    public SteamUserScript(StoreScriptId Id, object model) : base(Id, model) { }

    private StoreUserModel GetModel()
    {
        return GetModel<StoreUserModel>();
    }

    public override void Init()
    {
        GetModel().AppID = SteamUtils.GetAppID().ToString();
        GetModel().BuildID = SteamApps.GetAppBuildId();
        GetModel().IsLogin = SteamUser.BLoggedOn();
        GetModel().ID = SteamUser.GetSteamID().ToString();
        GetModel().Name = SteamFriends.GetPersonaName();
        GetModel().Avatar = GetAvater(SteamUser.GetSteamID());
        GetModel().Language = SteamUtils.GetSteamUILanguage();
        GetModel().ServerID = GetModel().ID + ":" + (int)StoreTarget.Steam;
    }

    private Texture2D GetAvater(CSteamID steamID)
    {
        int avatar = SteamFriends.GetMediumFriendAvatar(steamID);

        uint ImageWidth = 0;
        uint ImageHeight = 0;
        bool result = SteamUtils.GetImageSize(avatar, out ImageWidth, out ImageHeight);

        Texture2D texture = null;

        if (ImageWidth > 0 && ImageHeight > 0)
        {
            byte[] Image = new byte[ImageWidth * ImageHeight * 4];
            result = SteamUtils.GetImageRGBA(avatar, Image, (int)(ImageWidth * ImageHeight * 4));
            if (result)
            {
                texture = new Texture2D((int)ImageWidth, (int)ImageHeight, TextureFormat.RGBA32, false, true);
                texture.LoadRawTextureData(Image);
                texture.Apply();
            }
        }

        return texture;
    }

    public string GetLanguage()
    {
        return GetModel().Language;
    }
}
