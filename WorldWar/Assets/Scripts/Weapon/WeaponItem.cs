using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour 
{
    public WeaponType type = WeaponType.weapon_none;
    public float dis = 1f;
    public Transform ui = null;

    [Space]

    private bool firstsetting = false;
    private bool outlineshow = false;
    private cakeslice.Outline[] outlineList = null;
    private LineRenderer line = null;

    void Start()
    {
        line = ui.GetComponent<LineRenderer>();
    }
	
	void Update () 
    {
        if (firstsetting == false)
        {
            FocusSetting();
        }
        else
        {
            if (PlayerInput.Instance.TrackedObjR != null)
            {
                if (Vector3.Distance(ui.position, PlayerInput.Instance.TrackedObjR.position) < dis)
                    Outline(true);
                else
                    Outline(false);
            }
            if (outlineshow == true)
            {
                if (PlayerInput.Instance.GetTouchDownTriggerR())
                {
                    if (Player.Instance.weaponType != type)
                    {
                        int itemidx = (int)Define.PoolIdx.obj_Item_AKM;
                        switch (Player.Instance.weaponType)
                        {
                            case WeaponType.weapon_akm: itemidx = (int)Define.PoolIdx.obj_Item_AKM; break;
                            case WeaponType.weapon_sniper: itemidx = (int)Define.PoolIdx.obj_Item_Sniper; break;
                            case WeaponType.weapon_m911: itemidx = (int)Define.PoolIdx.obj_Item_M1911; break;
                            case WeaponType.weapon_grenade: itemidx = (int)Define.PoolIdx.obj_Item_Grenade; break;
                            case WeaponType.weapon_bow: itemidx = (int)Define.PoolIdx.obj_Item_Bow; break;
                            case WeaponType.weapon_handaxe: itemidx = (int)Define.PoolIdx.obj_Item_HandAxe; break;
                            case WeaponType.weapon_knife: itemidx = (int)Define.PoolIdx.obj_Item_Knife; break;
                            case WeaponType.weapon_sword: itemidx = (int)Define.PoolIdx.obj_Item_Sword; break;
                        }
                        AzuObjectPool.instance.SpawnPooledObj(itemidx, PlayerInput.Instance.TrackedObjR.position, PlayerInput.Instance.TrackedObjR.rotation);
                        Player.Instance.weaponType = type;
                        GameEvents.ChangeWeapon();
                        gameObject.SetActive(false);
                    }
                }
            }
            if (line)
            {
                ui.LookAt(transform.position);
                ui.position = Define.WCVector3(transform.position.x, PlayerInput.Instance.CamRig.position.y + 1f, transform.position.z);
                line.SetPosition(0, ui.position);
                line.SetPosition(1, transform.position);
            }
        }
	}

    void FocusSetting()
    {
        // Outline..
        if (PlayerInput.Instance.CamHead)
        {
            Camera cam = PlayerInput.Instance.CamHead.GetComponent<Camera>();
            if (cam == null || cam.GetComponent<cakeslice.OutlineEffect>() == null)
                return;

            Renderer[] list = transform.GetComponentsInChildren<Renderer>();
            outlineList = new cakeslice.Outline[list.Length];
            for (int i = 0; i < list.Length; i++)
            {
                outlineList[i] = list[i].gameObject.AddComponent<cakeslice.Outline>();
                outlineList[i].color = 1;
                outlineList[i].enabled = false;
            }
            outlineshow = true;
            firstsetting = true;
        }
    }

    void Outline(bool show)
    {
        if (outlineList == null || outlineshow == show) return;

        outlineshow = show;
        for (int i = 0; i < outlineList.Length; i++)
            outlineList[i].enabled = outlineshow;

        Debug.Log(gameObject.name + " Change weapon : " + show.ToString());
    }
}
