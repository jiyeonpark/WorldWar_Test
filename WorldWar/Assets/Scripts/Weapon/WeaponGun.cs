using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGun : Weapon
{
    public float clingDis = 0.3f;

    protected Transform LHandPoint = null;
    protected Transform gunPoint = null;
    protected Animator ani = null;
    protected bool bothHand = false;

    private Vector3 targetpoint = Vector3.zero;
    private Vector3 targetnormal = Vector3.zero;
    private string targettag = "";

    protected override void OnStart()
    {
        for (int i = 0; i < weaponObj.childCount; i++)
        {
            if (weaponObj.GetChild(i).name.Contains("LHandPoint")) LHandPoint = weaponObj.GetChild(i);
            else if (weaponObj.GetChild(i).name.Contains("GunPoint")) gunPoint = weaponObj.GetChild(i);
        }
        if (weaponObj)
            ani = weaponObj.GetComponent<Animator>();
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        if (PlayerInput.Instance.GetTouchDownTriggerR())
            Shot();

        base.OnUpdate();
    }

    protected override void OnLateUpdate()
    {
        ClingHandL();

        base.OnLateUpdate();
    }

    protected virtual void Shot()
    {
        if (ani) ani.SetTrigger("shot");
        Transform gun = null;
        if (bothHand) gun = gunPoint;
        else gun = weaponObj;
        SoundManager.Instance.PlayAKMShotSound(LHandPoint.position);
        GameObject bullet = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.obj_Bullet, gun.position, gun.rotation);
        GameObject gunfireeffect = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.effect_GunFire,
            gunPoint.position + gunPoint.forward * 0.1f + Define.WCVector3(0f, -0.03f, 0f), gunPoint.rotation);
        gunfireeffect.transform.parent = weaponObj;
        this.After(0.3f,
            () =>
            {
                gunfireeffect.SetActive(false);
                gunfireeffect.transform.parent = AzuObjectPool.instance.transform;
            });

        RaycastHit hit;
        if (Physics.Raycast(gun.position, gun.forward, out hit))
        {
            //if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Board"))
            //{// 탑승중인 객체는 충돌x
            //    targetpoint = Vector3.zero;
            //}
            //else
            {
                targetpoint = hit.point;
                targetnormal = hit.normal;
                targettag = hit.collider.tag;
                TargetPoint tp = hit.collider.gameObject.GetComponent<TargetPoint>();
                if (tp)
                    tp.ShotTarget();
                DamageCheck dc = hit.collider.gameObject.GetComponent<DamageCheck>();
                if (dc)
                {
                    if (dc.ai)
                        dc.ai.Damage(damage);
                }
            }
        }
        else
        {
            targetpoint = Vector3.zero;
            targetnormal = Vector3.zero;
            targettag = "";
        }
        StartCoroutine("ShotMove", bullet.transform);
        StartCoroutine("ShotPulse", 0.03f);
    }

    protected IEnumerator ShotMove(Transform obj)
    {
        //LineRenderer line = obj.GetComponentInChildren<LineRenderer>();
        LineRenderer line = null;
        Vector3 bulletPos = obj.position;
        Vector3 point = targetpoint;
        Vector3 normal = targetnormal;
        string tag = targettag;
        if (point == Vector3.zero)
            obj.position = Vector3.Lerp(bulletPos, bulletPos + obj.forward * 10f, 0.1f);
        else
            obj.position = Vector3.Lerp(bulletPos, point, 0.1f);
        yield return null;

        //float pulsetime = 0f;
        float dis = Vector3.Distance(point, bulletPos);
        if (point == Vector3.zero) dis = 10f;
        float time = dis / 600f;            // AKM speed = 600~715m / 1sec
        float alldis = 0f;
        while (alldis < dis)
        {
            float value = dis * Time.deltaTime / time;
            alldis += value;
            if (value > dis)
                value = dis;
            obj.position += obj.forward * value;

            if (line)
            {
                line.enabled = true;
                line.SetPosition(0, gunPoint.position);
                line.SetPosition(1, targetpoint);
            }

            //pulsetime += Time.deltaTime;
            //if(pulsetime < 0.03f) PlayerInput.Instance.TriggerHapticPulseR(1000);
            yield return null;
        }

        EffectManager.Instance.DecalEffect(point, normal, tag);
        obj.gameObject.SetActive(false);

        //yield return new WaitForSeconds(0.1f);
        if (line) line.enabled = false;
        //Debug.Log("shot gun ! = dis : " + dis.ToString() + " , time : " + time.ToString());
    }

    protected IEnumerator ShotPulse(float time)
    {
        yield return null;

        //float pulsetime = 0f;
        //while (pulsetime < time)
        //{
        //    Debug.Log("pulsetime = " + pulsetime.ToString());
        //    pulsetime += Time.deltaTime;
        //    PlayerInput.Instance.TriggerHapticPulseR(3500);
        //    if (bothHand) PlayerInput.Instance.TriggerHapticPulseL(3500);
        //    yield return null;
        //}

        float alltime = 0;
        while (alltime < 0.01f)
        {
            alltime += Time.deltaTime;
            float value = alltime * 1 / time;
            ushort pulse = (ushort)Mathf.Lerp(500f, 30000f, value);
            PlayerInput.Instance.TriggerHapticPulseR(pulse);
            if (bothHand) PlayerInput.Instance.TriggerHapticPulseL(pulse);
            yield return null;
        }

        PlayerInput.Instance.TriggerHapticPulseR(3500);
        if (bothHand) PlayerInput.Instance.TriggerHapticPulseL(3500);
        yield return null;

        alltime = 0;
        while (alltime < 0.02f)
        {
            alltime += Time.deltaTime;
            float value = alltime * 1 / time;
            ushort pulse = (ushort)Mathf.Lerp(3000f, 500f, value);
            PlayerInput.Instance.TriggerHapticPulseR(pulse);
            if (bothHand) PlayerInput.Instance.TriggerHapticPulseL(pulse);
            yield return null;
        }
    }

    protected void ClingHandL()
    {
        if (PlayerInput.Instance.TrackedObjL == null) return;
        float dis = Vector3.Distance(PlayerInput.Instance.TrackedObjL.position, LHandPoint.position);
        if (dis < clingDis)
        {
            if (type != WeaponType.weapon_m911)
            {
                // 오른손에 의해서 z축 변경이 되지 않게 하기 위함..
                Vector3 target = PlayerInput.Instance.TrackedObjL.position;
                transform.LookAt(target);
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
            }
            bothHand = true;
        }
        else
            bothHand = false;
        if (ani)
        {
            if (ani.GetBool("bothhand") != bothHand)
                ani.SetBool("bothhand", bothHand);
        }
    }
}
