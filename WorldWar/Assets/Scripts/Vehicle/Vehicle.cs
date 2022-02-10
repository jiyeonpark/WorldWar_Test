using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour 
{
    public Transform topObj = null;
    public Transform point_in = null;
    public Transform point_out = null;
    public Transform grid = null;
    public bool board = false;      // 탑승중..

    protected bool focus = false;
    protected bool outlineshow = false;
    protected bool setting = false;
    protected cakeslice.Outline[] outlineList = null;

    void Start()
    {
        if (StoreManager.sInstance.device == DeviceType.Normal)
            return;

        OnStart();
    }

    void Update()
    {
        if (PlayerInput.Instance.CamRig == null)
            return;

        if (focus || board)
        {
            if (PlayerInput.Instance.GetPressUpMenuR() || PlayerInput.Instance.GetPressUpMenuL())
            {
                OnVehicle();
            }
        }

        OnUpdate();
    }

    void LateUpdate()
    {
        OnLateUpdate();
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { FocusSetting(); Over(); }
    protected virtual void OnLateUpdate() {
        //if (grid && board)
        if (grid)
        {
            grid.position = PlayerInput.Instance.CamRig.position;
        }
    }

    protected void FocusSetting()
    {
        if (setting == true) return;

        // Outline..
        if (PlayerInput.Instance.CamHead)
        {
            Camera cam = PlayerInput.Instance.CamHead.GetComponent<Camera>();
            if (cam && cam.GetComponent<cakeslice.OutlineEffect>() == null)
            {
                cakeslice.OutlineEffect outlineEffect = cam.gameObject.AddComponent<cakeslice.OutlineEffect>();
                outlineEffect.sourceCamera = cam;
            }
        }

        Renderer[] list = topObj.GetComponentsInChildren<Renderer>();
        outlineList = new cakeslice.Outline[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            outlineList[i] = list[i].gameObject.AddComponent<cakeslice.Outline>();
            outlineList[i].enabled = false;
        }
        focus = false;
        outlineshow = false;
        setting = true;
    }

    protected void Over()
    {
        if (board == true) Focus(false);
        if (outlineList == null || outlineList.Length <= 0) return;
        if (focus == outlineshow) return;

        outlineshow = focus;
        for (int i = 0; i < outlineList.Length; i++)
            outlineList[i].enabled = outlineshow;

        Debug.Log(topObj.gameObject.name + " Focus = " + focus.ToString());
    }

    public virtual void OnVehicle() 
    {
        // 위치 이동 ..
        if (board)  // 탑승중에는 내리기..
        {
            PlayerInput.Instance.CamRig.position = point_out.position;
            Player.Instance.gameObject.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else        // 비탑승중에는 탑승시키기..
        {
            PlayerInput.Instance.CamRig.position = point_in.position;
            Player.Instance.gameObject.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("Board");
        }
        board = !board;
        Player.Instance.VehicleBoard(board);
        if (CamChoice.Instance.effect == true) CamChoice.Instance.frostEffect.FrostAmount = 0f;
        //if(grid) grid.gameObject.SetActive(board);
    }

    public void Focus(bool value) { focus = value; }
}
