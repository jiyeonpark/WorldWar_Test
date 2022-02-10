using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class LocalRemoteStorageScript : StoreScript
{
    public LocalRemoteStorageScript(StoreScriptId Id, object model) : base(Id, model) { }
    protected const string FileName = "data.gm";
    protected const int FileByte = 30000;
    protected string Password = "1234";
    private bool IsLoaded = false;
    private bool IsDeleted = false;

    private StoreRemoteStorageModel GetModel()
    {
        return GetModel<StoreRemoteStorageModel>();
    }

    public override void Init()
    {
        if (GameManager.IsCreated())
            Password = GameManager.Instance.userid;
        Load();
    }

    public override void Release()
    {
        FileSave();
    }

    public override void SaveFile()
    {
        FileSave();
    }

    public void FileSave()
    {
        bool infoIsInitialized = true;//g_Data.Info.IsInitialized;
        if (infoIsInitialized)
        {
            if (!IsDeleted && IsLoaded)
                Save();
        }
    }

    public override void Delete()
    {
        FileDelete(FileName, (result) =>
        {
            Debug.Log("FileDelete : " + result);
            IsDeleted = result;
        });
    }

    public override void RenderOnGUI()
    {
        if (GUILayout.Button("FileRead"))
        {
            FileRead(FileName, (json) => Debug.Log("FileRead : " + json));
        }

        if (GUILayout.Button("FileDelete"))
        {
            Delete();
        }

        if (GUILayout.Button("FileSave"))
        {
            Save();
        }
    }

    private void Load()
    {
        FileRead(FileName, (json) =>
        {
            StoreRemoteStorageModel datas = string.IsNullOrEmpty(json) ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<StoreRemoteStorageModel>(json);
            if (datas != null)
            {
                GetModel().Stages = datas.Stages;
                GetModel().ChallengeStages = datas.ChallengeStages;
                GetModel().ExtremeStages = datas.ExtremeStages;
                GetModel().ArcadeStages = datas.ArcadeStages;

                GetModel().FirstGame = datas.FirstGame;
                GetModel().SelectGraphic = datas.SelectGraphic;
                GetModel().SelectController = datas.SelectController;
                GetModel().SelectLanguage = datas.SelectLanguage;
                GetModel().SelectSoundFX = datas.SelectSoundFX;
                GetModel().SelectSoundBGM = datas.SelectSoundBGM;
            }
            else
            {
                GetModel().FirstGame = 1;
                GetModel().SelectGraphic = -1;
                GetModel().SelectController = -1;
                GetModel().SelectLanguage = -1;
                GetModel().SelectSoundFX = 1f;
                GetModel().SelectSoundBGM = 1f;
            }

            if (GetModel().Stages == null) GetModel().Stages = new List<StageDB>();
            if (GetModel().ChallengeStages == null) GetModel().ChallengeStages = new List<StageDB>();
            if (GetModel().ExtremeStages == null) GetModel().ExtremeStages = new List<StageDB>();
            if (GetModel().ArcadeStages == null) GetModel().ArcadeStages = new List<StageDB>();

            IsLoaded = true;
        });
    }

    private void Save()
    {
        //if (GameManager.Instance.serversave)
        //{
        //    GetModel().Stages.Clear();
        //    for (int i = 0; i < GameManager.Instance.stageInfoList.Count; i++)
        //        GetModel().Stages.Add(new StageDB() { Index = i, State = StageState.Lock, Score = 0 });
        //    GetModel().ChallengeStages.Clear();
        //    for (int i = 0; i < GameManager.Instance.challengeInfoList.Count; i++)
        //        GetModel().ChallengeStages.Add(new StageDB() { Index = i, State = StageState.Lock, Score = 0 });
        //    GetModel().ExtremeStages.Clear();
        //    for (int i = 0; i < GameManager.Instance.extremeInfoList.Count; i++)
        //        GetModel().ExtremeStages.Add(new StageDB() { Index = i, State = StageState.Lock, Score = 0 });
        //    GetModel().ArcadeStages.Clear();
        //    for (int i = 0; i < GameManager.Instance.arcadeInfoList.Count; i++)
        //        GetModel().ArcadeStages.Add(new StageDB() { Index = i, State = StageState.Lock, Score = 0, Rank = GameManager.Instance.arcadeInfoList[i].Rank, ScoreRank = GameManager.Instance.arcadeInfoList[i].ScoreRank });
        //}
        //else
        //{
        //    GetModel().Stages.Clear();
        //    for (int i = 0; i < GameManager.Instance.stageInfoList.Count; i++)
        //        GetModel().Stages.Add(GameManager.Instance.stageInfoList[i]);
        //    GetModel().ChallengeStages.Clear();
        //    for (int i = 0; i < GameManager.Instance.challengeInfoList.Count; i++)
        //        GetModel().ChallengeStages.Add(GameManager.Instance.challengeInfoList[i]);
        //    GetModel().ExtremeStages.Clear();
        //    for (int i = 0; i < GameManager.Instance.extremeInfoList.Count; i++)
        //        GetModel().ExtremeStages.Add(GameManager.Instance.extremeInfoList[i]);
        //    GetModel().ArcadeStages.Clear();
        //    for (int i = 0; i < GameManager.Instance.arcadeInfoList.Count; i++)
        //        GetModel().ArcadeStages.Add(GameManager.Instance.arcadeInfoList[i]);
        //}
        //GetModel().FirstGame = GameManager.Instance.firstGame;
        GetModel().SelectGraphic = (int)GameManager.Instance.graphicIdx;
        GetModel().SelectController = (int)GameManager.Instance.controllerIdx;
        GetModel().SelectLanguage = (int)GameManager.Instance.languageIdx;
        GetModel().SelectSoundFX = GameManager.Instance.soundfxValue;
        GetModel().SelectSoundBGM = GameManager.Instance.soundbgmValue;

        string json = Newtonsoft.Json.JsonConvert.SerializeObject(GetModel());
        FileWrite(FileName, json, (result) => { });
    }

    protected virtual void FileWrite(string fileName, string fileData, Action<bool> OnComplete)
    {
        bool result = false;

        try
        {
#if UNITY_EDITOR
            string encrypt = fileData;
#else
            string encrypt = fileData;
            if (StoreManager.sInstance != null)
            {
                if (StoreManager.sInstance.store == StoreTarget.Local)
                    encrypt = EncryptString(fileData, Password);
            }
#endif

            string[] splits = fileName.Split('.');
#if UNITY_EDITOR
            string filePath = Directory.GetCurrentDirectory() + "/" + splits[0] + ".txt";
#else
            string filePath = Application.dataPath + "/" + splits[0] + ".txt";
#endif
            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(encrypt);
            sw.Close();
            fs.Close();

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        if (OnComplete != null)
            OnComplete(result);
    }

    protected virtual void FileRead(string fileName, Action<string> OnComplete)
    {
        string decrypt = "";

        try
        {
            string[] splits = fileName.Split('.');
#if UNITY_EDITOR
            string filePath = Directory.GetCurrentDirectory() + "/" + splits[0] + ".txt";
#else
            string filePath = Application.dataPath + "/" + splits[0] + ".txt";
#endif
            StreamReader sr = new StreamReader(filePath);
            string fileData = sr.ReadLine();
            sr.Close();

#if UNITY_EDITOR
            decrypt = fileData;
#else
            decrypt = fileData;
            if(StoreManager.sInstance != null)
            {
                if(StoreManager.sInstance.store == StoreTarget.Local)
                    decrypt = DecryptString(fileData, Password);
            }
#endif
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        if (OnComplete != null)
            OnComplete(decrypt);
    }

    protected virtual void FileDelete(string fileName, Action<bool> OnComplete)
    {
        bool result = false;

        if (OnComplete != null)
            OnComplete(result);
    }

    protected string EncryptString(string InputText, string Password)
    {
        // Rihndael class를 선언하고, 초기화 합니다
        RijndaelManaged RijndaelCipher = new RijndaelManaged();

        // 입력받은 문자열을 바이트 배열로 변환
        byte[] PlainText = Encoding.Unicode.GetBytes(InputText);

        // 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서 
        // Salt를 사용합니다.
        byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

        // PasswordDeriveBytes 클래스를 사용해서 SecretKey를 얻습니다.
        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

        // Create a encryptor from the existing SecretKey bytes.
        // encryptor 객체를 SecretKey로부터 만듭니다.
        // Secret Key에는 32바이트
        // (Rijndael의 디폴트인 256bit가 바로 32바이트입니다)를 사용하고, 
        // Initialization Vector로 16바이트
        // (역시 디폴트인 128비트가 바로 16바이트입니다)를 사용합니다
        ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

        // 메모리스트림 객체를 선언,초기화 
        MemoryStream memoryStream = new MemoryStream();

        // CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

        // 암호화 프로세스가 진행됩니다.
        cryptoStream.Write(PlainText, 0, PlainText.Length);

        // 암호화 종료
        cryptoStream.FlushFinalBlock();

        // 암호화된 데이터를 바이트 배열로 담습니다.
        byte[] CipherBytes = memoryStream.ToArray();

        // 스트림 해제
        memoryStream.Close();
        cryptoStream.Close();

        // 암호화된 데이터를 Base64 인코딩된 문자열로 변환합니다.
        string EncryptedData = Convert.ToBase64String(CipherBytes);

        // 최종 결과를 리턴
        return EncryptedData;
    }

    protected string DecryptString(string InputText, string Password)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();

        byte[] EncryptedData = Convert.FromBase64String(InputText);
        byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);

        // Decryptor 객체를 만듭니다.
        ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

        MemoryStream memoryStream = new MemoryStream(EncryptedData);

        // 데이터 읽기(복호화이므로) 용도로 cryptoStream객체를 선언, 초기화
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

        // 복호화된 데이터를 담을 바이트 배열을 선언합니다.
        // 길이는 알 수 없지만, 일단 복호화되기 전의 데이터의 길이보다는
        // 길지 않을 것이기 때문에 그 길이로 선언합니다
        byte[] PlainText = new byte[EncryptedData.Length];

        // 복호화 시작
        int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

        memoryStream.Close();
        cryptoStream.Close();

        // 복호화된 데이터를 문자열로 바꿉니다.
        string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

        // 최종 결과 리턴
        return DecryptedData;
    }
}
