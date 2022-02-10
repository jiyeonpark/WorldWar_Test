using UnityEngine;

public class StoreUserModel : StoreModel
{
    public string AppID { get; set; }
    public int BuildID { get; set; }
    public bool IsLogin { get; set; }
    public string ID { get; set; }
    public string Name { get; set; }
    public Texture2D Avatar { get; set; }
    public string Language { get; set; }
    public string ServerID { get; set; }
    public string AuthTicket { get; set; }
}
