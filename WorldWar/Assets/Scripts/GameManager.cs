using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using BattleofRedCliffsVRService.ViewModels;

public enum SceneState
{
    Lobby,
    Game,
}

public enum StageState
{
    Lock,
    UnLock,
    Success,
}

public class StageDB
{
    public int Index { get; set; }
    public StageState State { get; set; }
    public int Score { get; set; }
    public int Rank { get; set; }
    public List<int> ScoreRank { get; set; }
}

public class GameManager : AutoSingleton<GameManager> 
{
    public bool isInitialized = false;

    // 옵션
    [Space]
    public Define.GraphicIdx graphicIdx = Define.GraphicIdx.Hight;
    public Define.ControllerIdx controllerIdx = Define.ControllerIdx.Right;
    public Define.LanguageIdx languageIdx = Define.LanguageIdx.English;
    public float soundfxValue = 0f;
    public float soundbgmValue = 0f;

    [Space]
    [HideInInspector]
    public string userid = "1234WisecatPasswordCode4321";
    [HideInInspector]
    public bool serversave = false;

    private bool settingstart = false;
    private List<StageViewModel> stagesSD = null;
    private bool servererror = false;

    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);
        StoreManager.sInstance.Init((isInitialized) =>
        {
            if (!isInitialized)
            {
                Application.Quit();
                return;
            }
            else
                settingstart = true;
        });

        base.OnAwake();
    }

    void Auth()
    {
        AuthServer.ActionAuth += (sc) =>
        {
            if (sc == null || !sc.result)
            {   //인증 실패
                Debug.LogError("auth error");
                Application.Quit();
            }
            else
            {   //인증 성공
                Debug.Log("ActionAuthLogin");
                if (StoreManager.sInstance.store != StoreTarget.Local
                    && StoreManager.sInstance.store != StoreTarget.Steam
                    && StoreManager.sInstance.store != StoreTarget.Oculus
                    && StoreManager.sInstance.store != StoreTarget.VRoadcast)
                {
                    serversave = true;
                    Login();
                }
                else
                    Setting();
            }
        };

        AuthServer.sInstance.Auth();
    }

    void Login()
    {
        StoreUserModel model = StoreManager.sInstance.GetModel<StoreUserModel>(StoreScriptId.User);

        string Id = model.ServerID;
        string NickName = model.Name;

#if UNITY_EDITOR
        Debug.Log("Wisecat server login : ID = " + Id.Length + " : NickName = " + NickName);
#endif

        LobbyServer.ActionGetUser = (sc) =>
        {
            if (sc == null)
            {   //생성
                CreateUser(Id, NickName);
            }
            else
            {
                //시작
                LoginSuccess(sc);
            }
        };

        LobbyServer.sInstance.GetUser(Id);
    }

    void CreateUser(string Id, string NickName)
    {
        LobbyServer.ActionCreateUser = (sc) =>
        {
            if (sc == null)
            {
                Debug.LogError("CreateUser");
            }
            else
            {
                //시작
                LoginSuccess(sc);
            }
        };

        LobbyServer.sInstance.CreateUser(Id, NickName);
    }

    void LoginSuccess(UserViewModel user)
    {
        stagesSD = user.Stages;
        Setting();
        Debug.Log("SD login success!");
    }

    public void ServerError(string message)
    {
        //if (messagebox)
        //{
        //    Transform tr = Instantiate(messagebox).transform;
        //    tr.localPosition = Vector3.zero;
        //    tr.localRotation = Quaternion.identity;
        //    tr.localScale = Vector3.one;
        //    UnityEngine.UI.Text text = tr.GetComponentInChildren<UnityEngine.UI.Text>();
        //    text.text = message;
        //}
        Debug.LogError(message);
        servererror = true;
    }

    void SDSetting()
    {// Server data setting..
        if (stagesSD != null)
        {
            for (int i = 0; i < stagesSD.Count; i++)
            {
            }
        }
    }

    void SDUpdate()
    {
        if (serversave == false) return;

        Debug.Log("Call SDUpdate!");

        LobbyServer.ActionUpdateStage += (sc) =>
        {
            if (sc == null)
            {   //실패
                Debug.Log("error SDUpdate()");
            }
            else
            {
                //갱신

            }
        };
    }

    void Setting()
    {
        if (StoreManager.sInstance != null)
        {
            isInitialized = true;
            GameEvents.IsInitialized();

            // SD Setting..
            SDSetting();
        }
    }

    public void CamClippingSetting()
    {
        if (PlayerInput.Instance.CamHead != null)
        {
            Camera cam = PlayerInput.Instance.CamHead.GetComponent<Camera>();
            if (cam)
                cam.farClipPlane = 1200f;
        }
    }
}
