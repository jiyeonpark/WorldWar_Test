using Steamworks;
using System.Text;
using System;
using UnityEngine;

public class SteamRemoteStorageScript : LocalRemoteStorageScript
{
    public SteamRemoteStorageScript(StoreScriptId Id, object model) : base(Id, model) { }
    
    protected override void FileWrite(string fileName, string fileData, Action<bool> OnComplete)
    {
        bool result = false;

        try
        {
            string encrypt = EncryptString(fileData, SteamUser.GetSteamID().ToString());
            byte[] data = new byte[Encoding.UTF8.GetByteCount(encrypt)];
            Encoding.UTF8.GetBytes(encrypt, 0, encrypt.Length, data, 0);
            result = SteamRemoteStorage.FileWrite(fileName, data, data.Length);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (OnComplete != null)
            OnComplete(result);
    }

    protected override void FileRead(string fileName, Action<string> OnComplete)
    {
        string decrypt = "";

        try
        {
            byte[] data = new byte[FileByte];
            int result = SteamRemoteStorage.FileRead(fileName, data, data.Length);
            string fileData = Encoding.UTF8.GetString(data, 0, result);
            decrypt = DecryptString(fileData, SteamUser.GetSteamID().ToString());
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (OnComplete != null)
            OnComplete(decrypt);
    }

    protected override void FileDelete(string fileName, Action<bool> OnComplete)
    {
        bool result = false;

        try
        {
            result = SteamRemoteStorage.FileDelete(fileName);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        if (OnComplete != null)
            OnComplete(result);
    }
}
