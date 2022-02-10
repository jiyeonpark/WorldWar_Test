using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    weapon_none = 0,
    weapon_akm,
    weapon_sniper,
    weapon_m911,
    weapon_grenade,
    weapon_bow,
    weapon_handaxe,
    weapon_knife,
    weapon_sword,

    weapon_max,
}

public class Weapon : MonoBehaviour 
{
    public WeaponType type = WeaponType.weapon_none;
    public Transform weaponObj = null;
    public bool righthand = true;
    public bool equip = false;              // 플레이어가 이 무기를 장착했는지 여부..
    public float damage = 10f;
    
    void Start()
    {
        OnChangeWeapon();
        if (StoreManager.sInstance.device == DeviceType.Normal)
            return;

        GameEvents.ChangeWeaponEvent += OnChangeWeapon;

        OnStart();
    }

    void OnDestroy()
    {
        GameEvents.ChangeWeaponEvent -= OnChangeWeapon;
    }

    void Update()
    {
        if (PlayerInput.Instance.CamRig == null)
            return;

        OnUpdate();
    }

    void LateUpdate()
    {
        if (PlayerInput.Instance.CamRig == null)
            return;

        if (righthand)
        {
            transform.position = PlayerInput.Instance.TrackedObjR.position;
            transform.rotation = PlayerInput.Instance.TrackedObjR.rotation;
        }
        else
        {
            transform.position = PlayerInput.Instance.TrackedObjL.position;
            transform.rotation = PlayerInput.Instance.TrackedObjL.rotation;
        }
        transform.Rotate(Vector3.right, 60f);

        OnLateUpdate();
    }

    void OnChangeWeapon()
    {
        if (Player.IsCreated())
        {
            if (Player.Instance.weaponType != type)
            {
                equip = false;
                gameObject.SetActive(false);
            }
            else
            {
                equip = true;
                gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnLateUpdate() { }
}
