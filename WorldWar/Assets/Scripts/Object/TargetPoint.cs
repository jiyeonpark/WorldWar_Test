using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour 
{
    public GameObject[] objlist = null;
    public float randomMin = 2f;
    public float randomMax = 5f;
    public Vector3 moveLocalPos = Vector3.zero;
    public float moveTime = 0f;

    private BoxCollider boxCol = null;
    private Animator ani = null;
    private float time = 0f;
    private float deltatime = 0f;
    private bool show = false;
    private float allTime = 0f;
    private Vector3 orgPos = Vector3.zero;

    void Start()
    {
        boxCol = GetComponent<BoxCollider>();
        ani = GetComponent<Animator>();
        orgPos = transform.position;
        time = Random.Range(randomMin, randomMax);
        if (boxCol) boxCol.enabled = false;
    }

    void Update()
    {
        if (show == false)
        {
            deltatime += Time.deltaTime;
            if (deltatime >= time)
            {
                show = true;
                deltatime = 0f;
                time = Random.Range(randomMin, randomMax);
                ShowTarget();
            }
        }
        MoveTarget();
    }

    void ShowTarget()
    {
        if (boxCol == null) return;

        int random = Random.Range(0, objlist.Length);
        for(int i=0; i<objlist.Length; i++)
        {
            if(i == random)
                objlist[i].SetActive(true);
            else
                objlist[i].SetActive(false);
        }
        boxCol.enabled = true;
        boxCol.size = Vector3.one - (Vector3.one * random * 0.2f);
        if (ani) ani.SetTrigger("start");
    }

    void MoveTarget()
    {
        if (moveTime == 0f || moveLocalPos == Vector3.zero) return;

        allTime += Time.deltaTime;
        float value = allTime * 1 / moveTime;
        transform.position = Vector3.Lerp(orgPos, orgPos + moveLocalPos, value);
        if(value >= 1f)
        {
            allTime = 0f;
            moveLocalPos *= -1f;
            orgPos = transform.position;
        }
    }

    public void ShotTarget()
    {
        show = false;
        if (boxCol) boxCol.enabled = false;
        if (ani) ani.SetTrigger("shot");
    }
}
