using System;

public class VRoadcastRemoteStorageScript : LocalRemoteStorageScript
{
    public VRoadcastRemoteStorageScript(StoreScriptId Id, object model) : base(Id, model) { }

    protected new const string FileName = "data.vp";

    protected override void FileWrite(string fileName, string fileData, Action<bool> OnComplete)
    {
        string encrypt = EncryptString(fileData, Password);

        base.FileWrite(fileName, encrypt, (result) =>
        {
            if (OnComplete != null)
                OnComplete(result);
        });
    }

    protected override void FileRead(string fileName, Action<string> OnComplete)
    {
        base.FileRead(fileName, (fileData) =>
        {
            if (OnComplete != null)
                OnComplete(DecryptString(fileData, Password));
        });
    }
}
