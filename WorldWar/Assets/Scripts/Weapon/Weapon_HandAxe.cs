using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_HandAxe : WeaponThrow
{
    public float maxT = 0f;
    public float maxB = 0f;
    public float avgSpeedTop = 0f;
    public float avgSpeedBottom = 0f;

    protected override void OnStart()
    {
        type = WeaponType.weapon_handaxe;

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

            GameObject projectile = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.obj_HandAxe, transform.position, transform.rotation) as GameObject;
            if (projectile.transform.GetComponent<HandAxeObject>())
            {
                HandAxeObject obj = projectile.transform.GetComponent<HandAxeObject>();
                obj.InitializeProjectile(true);
            }
            if (StoreManager.sInstance.device == DeviceType.Steam)
                Shot(transform.position, swingVec, PlayerInput.Instance.TrackedObjR.rotation, projectile, true);
            else
                Shot(listTopPositions[listTopPositions.Count - 1], swingVec, PlayerInput.Instance.TrackedObjR.rotation, projectile, true);
        }

        base.OnUpdate();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter : " + collision.gameObject.name + " / " + collision.collider.gameObject.name);
        CheckEnemy(collision);
    }

    void CheckEnemy(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.collider.gameObject.layer == LayerMask.NameToLayer("EnemyWeapon"))
        {
            DamageCheck dc = collision.collider.gameObject.GetComponent<DamageCheck>();
            if (dc == null)
            {
                return;
            }

            if (!isSwing(avgSpeedTop * 1.3f, avgSpeedBottom * 1.3f))
            {
                if (dc.ai)
                    dc.ai.Damage(damage);
                EffectManager.Instance.DecalEffect(collision.contacts[0].point, collision.contacts[0].normal, collision.collider.tag);
                return;
            }
        }

    }

    bool isSwing(float speedTop, float speedBottom)
    {
        if (speedTop > maxT && speedBottom > maxB)
            return true;
        return false;
    }
}
