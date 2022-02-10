using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour 
{
    public float time = 5f;
    public GameObject obj = null;
    public GameObject effect = null;
    [Tooltip("Radius of explosion.")]
    public float radius = 3.0f;
    [Tooltip("Layers that will be hit by explosion.")]
    public LayerMask blastMask;
    public bool safetypin = true;

    private Rigidbody rigid = null;
    private float damagevalue = 10f;

	void Start () 
    {
        if (effect) effect.SetActive(false);
        rigid = GetComponent<Rigidbody>();
	}

    public void Initialize(float damage)
    {
        if (obj) obj.SetActive(true);
        if (effect) effect.SetActive(false);
        if (rigid) rigid.isKinematic = false;
        if (safetypin == false)
        {
            this.After(time,
                () =>
                {
                    StopCoroutine("ExplosionShow");
                    StartCoroutine("ExplosionShow", 5f);
                });
        }
        else
        {
            this.After(time + 5f,
                () =>
                {
                    gameObject.SetActive(false);
                });
        }
        damagevalue = damage;
    }

    IEnumerator ExplosionShow(float endtime)
    {
        yield return null;

        if (obj) obj.SetActive(false);
        if (effect) effect.SetActive(true);
        if (rigid) rigid.isKinematic = true;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, blastMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            DamageCheck dc = hitColliders[i].transform.GetComponent<DamageCheck>();
            if (dc)
            {
                if (dc.ai)
                    dc.ai.Damage(damagevalue);
            }
        }

        yield return new WaitForSeconds(endtime);

        gameObject.SetActive(false);
    }
}
