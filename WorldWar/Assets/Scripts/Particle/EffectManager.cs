using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : AutoSingleton<EffectManager> 
{
    public void DecalEffect(Vector3 point, Vector3 normal, string tag, WeaponType type = WeaponType.weapon_none)
    {
        int decalIdx = (int)Define.PoolIdx.decal_Dirt;
        int effectIdx = (int)Define.PoolIdx.effect_Dirt;
        switch (tag)
        {
            case "metal": decalIdx = (int)Define.PoolIdx.decal_Metal; effectIdx = (int)Define.PoolIdx.effect_Metal; break;
            case "wood": decalIdx = (int)Define.PoolIdx.decal_Wood; effectIdx = (int)Define.PoolIdx.effect_Wood; break;
            case "concrete": decalIdx = (int)Define.PoolIdx.decal_Metal; effectIdx = (int)Define.PoolIdx.effect_Concrete; break;
            case "Player": decalIdx = -1; effectIdx = (int)Define.PoolIdx.effect_Blood; break;
        }
        if (decalIdx != -1)
        {
            // decal
            GameObject decal = AzuObjectPool.instance.SpawnPooledObj(decalIdx, point + (normal * 0.025f), Quaternion.identity);
            GameObject cloneDecal = decal.transform.GetChild(0).gameObject;
            cloneDecal.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        }
        // effect
        GameObject effectdirt = AzuObjectPool.instance.SpawnPooledObj(effectIdx, point, Quaternion.identity);
        if (normal != Vector3.zero)
            effectdirt.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        else
            effectdirt.transform.rotation = Quaternion.Euler(Vector3.up);
        effectdirt.GetComponent<EmitterControl>().Initialize();
    }
}
