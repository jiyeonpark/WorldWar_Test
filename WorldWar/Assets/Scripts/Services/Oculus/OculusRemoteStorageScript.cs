using Oculus.Platform;
using Oculus.Platform.Models;
using System;
using System.Text;
using UnityEngine;

public class OculusRemoteStorageScript : LocalRemoteStorageScript
{
    public OculusRemoteStorageScript(StoreScriptId Id, object model) : base(Id, model) { }
    private const string CloudStorageBucket = "GameModels";
    private const string CloudStorageKey = "models";

    protected override void FileWrite(string fileName, string fileData, Action<bool> OnComplete)
    {
        try
        {
            string encrypt = EncryptString(fileData, Password);
            byte[] data = new byte[Encoding.UTF8.GetByteCount(encrypt)];
            Encoding.UTF8.GetBytes(encrypt, 0, encrypt.Length, data, 0);
            CloudStorage.Save(CloudStorageBucket, CloudStorageKey, data, 0, "").OnComplete((msg) => 
            {
                if (msg.IsError)
                {
                    Debug.Log(msg.GetError().Message);
                }

                if (OnComplete != null)
                    OnComplete(!msg.IsError);
            });
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);

            if (OnComplete != null)
                OnComplete(false);
        }
    }

    protected override void FileRead(string fileName, Action<string> OnComplete)
    {
        string decrypt = "";

        try
        {
            CloudStorage.Load(CloudStorageBucket, CloudStorageKey).OnComplete((msg) =>
            {
                if (msg.IsError)
                {
                    Debug.Log(msg.GetError().Message);
                }
                else
                {
                    CloudStorageData storageData = msg.GetCloudStorageData();

                    string fileData = Encoding.UTF8.GetString(storageData.Data, 0, (int)storageData.DataSize);
                    decrypt = DecryptString(fileData, Password);
                }

                if (OnComplete != null)
                    OnComplete(decrypt);
            });
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);

            if (OnComplete != null)
                OnComplete(decrypt);
        }
    }

    protected override void FileDelete(string fileName, Action<bool> OnComplete)
    {
        bool result = false;

        try
        {
            CloudStorage.Delete(CloudStorageBucket, CloudStorageKey).OnComplete((msg) =>
            {
                result = !msg.IsError;

                if (msg.IsError)
                {
                    Debug.Log(msg.GetError().Message);
                }

                if (OnComplete != null)
                    OnComplete(result);
            });
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);

            if (OnComplete != null)
                OnComplete(result);
        }
    }
}
