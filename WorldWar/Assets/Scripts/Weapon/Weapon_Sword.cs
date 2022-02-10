using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword : Weapon
{
    public float maxT = 0f;
    public float maxB = 0f;
    public float avgSpeedTop = 0f;
    public float avgSpeedBottom = 0f;

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
