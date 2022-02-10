using Newtonsoft.Json;
using WisecatAuthServer.Models;

public partial class AuthServer
{
    public void SteamAuth(string appid, string userid)
    {
        SteamAuthModel auth = new SteamAuthModel()
        {
            appid = appid,
            userid = userid,
        };

        http.Post(GetURL("api/auth/steam"), auth, (req, res) =>
        {
            AuthResponseModel sc = null;

            if (res.IsSuccess)
            {
                string data = res == null ? "" : res.DataAsText;

                sc = JsonConvert.DeserializeObject<AuthResponseModel>(data);
            }

            if (ActionAuth != null)
                ActionAuth(sc);
        });
    }
}
