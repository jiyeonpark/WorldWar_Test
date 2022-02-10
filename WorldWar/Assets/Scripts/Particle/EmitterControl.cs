using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterControl : MonoBehaviour 
{
    public float showTime = 1f;
    private EllipsoidParticleEmitter[] emitter = null;
    private float tempTime = 0f;

	void Awake()
    {
        emitter = GetComponentsInChildren<EllipsoidParticleEmitter>();
	}

	public void Initialize()
    {
        for (int i = 0; i < emitter.Length; i++)
            emitter[i].Emit();
        tempTime = 0f;
    }

    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime > showTime)
            gameObject.SetActive(false);
    }
}
