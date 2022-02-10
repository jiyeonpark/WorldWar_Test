public enum StoreScriptId : int
{
    User,
    Achievement,
    RemoteStorage,
    Leaderboard,
    
    None,
}

public class StoreModel { }
public class StoreScript
{
    public StoreScriptId Id;
    protected object Model;
    public T GetModel<T>() { return (T)Model; }

    public StoreScript(StoreScriptId Id, object model) { this.Id = Id; Model = model; }

    public virtual void Init() { }
    public virtual void Delete() { }
    public virtual void Release() { }
    public virtual void SaveFile() { }
    public virtual void Loop() { }
    public virtual void RenderOnGUI() { }
}
