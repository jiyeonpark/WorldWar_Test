using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Grenade : WeaponThrow 
{
    public Transform pin = null;

    private bool safetypin = true;
    private bool holdpin = false;
    private Vector3 orgPinPos = Vector3.zero;
    private Vector3 orgLPos = Vector3.zero;

    protected override void OnStart()
    {
        type = WeaponType.weapon_grenade;
        orgPinPos = pin.localPosition;

        base.OnStart();
	}

    protected override void OnUpdate()
    {
        bool touchTrigger = false;
        bool touchUpTrigger = false;
        if (PlayerInput.Instance.GetTouchTriggerR()) touchTrigger = true;
        if (PlayerInput.Instance.GetTouchUpTriggerR()) touchUpTrigger = true;
        if (touchTrigger)
        {
            CapsuleCollider[] colliders = GetComponents<CapsuleCollider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }
        }
        if (touchUpTrigger)
        {
            List<Vector3> listTopPositions = swinglist.GetSwingList();
            Vector3 swingVec = listTopPositions[listTopPositions.Count - 1] - listTopPositions[0];

            GameObject projectile = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.obj_Grenade, transform.position, Quaternion.identity) as GameObject;
            if (projectile.transform.GetComponent<Explosion>())
            {
                Explosion obj = projectile.transform.GetComponent<Explosion>();
                obj.safetypin = safetypin;
                obj.Initialize(damage);
            }
            if (StoreManager.sInstance.device == DeviceType.Steam)
                Shot(transform.position, swingVec, PlayerInput.Instance.TrackedObjR.rotation, projectile, true);
            else
                Shot(listTopPositions[listTopPositions.Count - 1], swingVec, PlayerInput.Instance.TrackedObjR.rotation, projectile, true);
            //pin.gameObject.SetActive(true);
            pin.parent = weaponObj;
            pin.GetComponent<Rigidbody>().isKinematic = true;
            pin.localPosition = orgPinPos;
            pin.localRotation = Quaternion.identity;
            safetypin = true;
            holdpin = false;
        }	

        if(PlayerInput.Instance.GetTouchDownTriggerL())
        {
            float dis = Vector3.Distance(PlayerInput.Instance.TrackedObjL.position, pin.transform.position);
            if (dis < 0.06f)
                holdpin = true;
        }
        if (holdpin == true && PlayerInput.Instance.GetTouchTriggerL())
        {
            pin.position = PlayerInput.Instance.TrackedObjL.position;
            float dis = Vector3.Distance(PlayerInput.Instance.TrackedObjL.position, transform.position);
            if (dis > 0.1f)
            {
                if (safetypin == true)
                {
                    PlayerInput.Instance.TriggerHapticPulseL(1000);
                    PlayerInput.Instance.TriggerHapticPulseR(1000);
                    SoundManager.Instance.PlayGrenadePinSound(pin.position);
                }
                safetypin = false;
            }
            orgLPos = PlayerInput.Instance.TrackedObjL.position;
        }
        if (safetypin == false && PlayerInput.Instance.GetTouchUpTriggerL())
        {
            pin.parent = null;
            pin.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 dirL = PlayerInput.Instance.TrackedObjL.position - orgLPos;
            pin.GetComponent<Rigidbody>().AddForce(dirL * 10000f);
            //pin.gameObject.SetActive(false);
            holdpin = false;
        }

        base.OnUpdate();
	}
}
