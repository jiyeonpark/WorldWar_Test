public class LocalUserScript : StoreScript
{
    public LocalUserScript(StoreScriptId Id, object model) : base(Id, model) { }

    private StoreUserModel GetModel()
    {
        return GetModel<StoreUserModel>();
    }

    public override void Init()
    {
        GetModel().ID = "LocalTester";
        GetModel().Name = "";
        GetModel().ServerID = GetModel().ID + ":" + (int)StoreTarget.Local;
    }
}
