using Newtonsoft.Json;
using WisecatAuthServer.Models;

public partial class AuthServer
{
    public void OculusAuth(string appid, string userid, string ticket)
    {
        OculusAuthModel auth = new OculusAuthModel()
        {
            appid = appid,
            userid = userid,
            ticket = ticket,
        };

        http.Post(GetURL("api/auth/oculus"), auth, (req, res) =>
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
