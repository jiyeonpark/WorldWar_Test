using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrow : Weapon
{
    public float projectileForce = 30f;

    protected HandSwingList swinglist = null;

    protected override void OnStart()
    {
        swinglist = weaponObj.GetComponent<HandSwingList>();

        base.OnStart();
    }

    Ray weaponRay;
    public bool Shot(Vector3 curpos, Vector3 swingVec, Quaternion quat, GameObject obj, bool isMine = true, bool isPin = false)
    {
        weaponRay.origin = curpos;
        weaponRay.direction = swingVec.normalized;
        Vector3 pos = weaponRay.origin;
        float projectileForceAmt = projectileForce * (swingVec.magnitude) * (swingVec.magnitude);
        Vector3 force = weaponRay.direction * projectileForceAmt * 70;
        if (StoreManager.sInstance.device == DeviceType.Oculus)
            force *= 0.5f;

        force.x *= 1.5f;
        force.z *= 1.5f;

        //Debug.Log("force = " + force.magnitude);
        if (force.magnitude < 30f)
            return false;

        obj.transform.position = pos;
        obj.transform.rotation = quat;

        StopCoroutine(FireOneShot(isMine, pos, quat, force, swingVec, obj, isPin));
        StartCoroutine(FireOneShot(isMine, pos, quat, force, swingVec, obj, isPin));
        return true;
    }

    IEnumerator FireOneShot(bool isMine, Vector3 pos, Quaternion quat, Vector3 force, Vector3 swingVec, GameObject obj, bool isPin)
    {
        for (float i = 0; i < 1; i++)
        {
            Rigidbody projBody = obj.GetComponent<Rigidbody>();
            projBody.velocity = Vector3.zero;
            projBody.angularVelocity = Vector3.zero;

            Vector3 targetpos = Vector3.zero;
            if (targetpos == Vector3.zero)
                projBody.AddForce(force);
            else
            {// 보정..
                float dis = Vector3.Distance(transform.position, targetpos);
                float time = Mathf.Lerp(0.1f, 1f, dis / 20f);
                float fx = 0f; float fy = 0f; float fz = 0f;
                BallUtil.TrajectoryCalculate.GetForceValue(ref fx, ref fy, ref fz, pos, targetpos, 0f, 0f, 0f, 1f, time);
                projBody.AddForce(fx, fy, fz);
            }
        }
        yield return null;

    }
}
