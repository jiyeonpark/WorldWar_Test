using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sniper : WeaponGun
{
    public enum realType
    {
        real_none = 0,
        real_half,
        real_only,
    }

    public bool scope = false;
    public Camera scopeCam = null;
    public Transform renderPlan = null;
    public Transform scopeObj = null;
    public realType realtype = realType.real_none;

    private bool firstSetting = false;
    private Camera main = null;
    private int orgMask = 0;
    private Vector3 orgPos = Vector3.zero;
    private Vector3 orgScale = Vector3.zero;
    private Quaternion orgRot = Quaternion.identity;
    private Transform orgParent = null;

    protected override void OnStart()
    {
        type = WeaponType.weapon_sniper;
        //scopeCam.gameObject.SetActive(false);
        //renderPlan.gameObject.SetActive(false);
        orgPos = scopeObj.localPosition;
        orgScale = scopeObj.localScale;
        orgRot = scopeObj.localRotation;
        orgParent = scopeObj.parent;

        if (realtype == realType.real_only)
        {
            scopeCam.fieldOfView = 5f;
        }
        else if (realtype == realType.real_half)
        {
            scopeCam.fieldOfView = 8f;
            renderPlan.localPosition = Define.WCVector3(renderPlan.localPosition.x, renderPlan.localPosition.y, 0.025f);
        }
        else
        {
            scopeCam.fieldOfView = 10f;
        }

        base.OnStart();
    }

    protected override void OnUpdate()
    {
        if (firstSetting == false)
        {
            if (PlayerInput.Instance.CamHead)
            {
                main = PlayerInput.Instance.CamHead.GetComponent<Camera>();
                orgMask = main.cullingMask;
                PlayerInput.Instance.TrackedObjL.gameObject.layer = LayerMask.NameToLayer("Render2");
                PlayerInput.Instance.TrackedObjR.gameObject.layer = LayerMask.NameToLayer("Render2");
                firstSetting = true;
            }
        }
        if (realtype == realType.real_none)
        {
            if (PlayerInput.Instance.CamHead)
            {
                float dis = Vector3.Distance(PlayerInput.Instance.CamHead.position, scopeCam.transform.position);
                float angle = Vector3.Angle(PlayerInput.Instance.CamHead.forward, (scopeCam.transform.position - PlayerInput.Instance.CamHead.position).normalized);
                if (dis < 0.2f && angle < 60f)
                {
                    if (scope == false)
                    {
                        scopeObj.parent = PlayerInput.Instance.CamHead;
                        scopeObj.localPosition = Define.WCVector3(-0.015f, -0.2f, 0f);
                        scopeObj.localScale = Vector3.one * 25f;
                        scopeObj.localEulerAngles = Define.WCVector3(0f, 180f, 0f);
                        renderPlan.localPosition = Define.WCVector3(renderPlan.localPosition.x, renderPlan.localPosition .y, -0.03f);
                        main.cullingMask = (1 << LayerMask.NameToLayer("Render1")) | (1 << LayerMask.NameToLayer("Render2"));
                        scope = true;
                    }
                }
                else
                {
                    if (scope == true)
                    {
                        scopeObj.parent = orgParent;
                        scopeObj.localPosition = orgPos;
                        scopeObj.localScale = orgScale;
                        scopeObj.localRotation = orgRot;
                        main.cullingMask = orgMask & ~(1 << LayerMask.NameToLayer("Render2"));
                        scope = false;
                    }
                }
            }
        }

        base.OnUpdate();
    }
}
