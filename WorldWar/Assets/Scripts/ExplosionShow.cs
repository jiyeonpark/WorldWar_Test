using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionShow : MonoBehaviour 
{
    public float mintime = 10f;
    public float maxtime = 20f;

    private float time = 0f;
    private float temptime = 0f;

    void Awake()
    {
        Show();
    }
	
	void Update () 
    {
        temptime += Time.deltaTime;
		if(time < temptime)
        {
            Show();
        }
	}

    void Show()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        if(time > 0f) transform.GetChild(Random.Range(0, transform.childCount)).gameObject.SetActive(true);
        time = Random.Range(mintime, maxtime);
        temptime = 0f;
    }
}
