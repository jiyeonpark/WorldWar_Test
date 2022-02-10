using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_AKM : WeaponGun
{
    public float shotTime = 0.2f;

    private float shotTimeTemp = 0f;

	protected override void OnStart()
    {
        type = WeaponType.weapon_akm;
        base.OnStart();
    }
	
	protected override void OnUpdate()
    {
        if (PlayerInput.Instance.GetTouchTriggerR())
        {
            shotTimeTemp += Time.deltaTime;
            if (shotTimeTemp >= shotTime)
            {
                shotTimeTemp = 0f;
                Shot();
            }
        }

        base.OnUpdate();
    }
}
