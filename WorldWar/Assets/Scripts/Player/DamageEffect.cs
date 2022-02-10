using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour 
{
	void Start () 
    {
        gameObject.SetActive(false);
        GameEvents.EffectDamageEvent += OnEffectDamage;
	}

    void OnDestroy()
    {
        GameEvents.EffectDamageEvent -= OnEffectDamage;
    }
	
	void OnEffectDamage()
    {
        gameObject.SetActive(true);
        this.After(0.1f,
            () =>
            {
                gameObject.SetActive(false);
            });
    }
}
