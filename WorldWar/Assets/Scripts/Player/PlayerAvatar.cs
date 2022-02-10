using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour 
{
    public bool movekey = false;
    public Vector3 headPos = Vector3.zero;
    public Vector3 headDir = Vector3.zero;
    public Vector3 lHandPos = Vector3.zero;
    public Vector3 rHandPos = Vector3.zero;
    
    public Transform headBone = null;

    private Animator anim = null;
    private Vector3 prevpos = Vector3.zero;
    private Vector3 prevdir = Vector3.zero;

	void Start () 
    {
        anim = GetComponent<Animator>();
        Transform[] list = GetComponentsInChildren<Transform>();
        for (int i = 0; i < list.Length; i++)
        {
            switch (list[i].name)
            {
                case "Head": headBone = list[i]; break;
            }
        }
	}

	void Update () 
    {
        Vector3 pos = transform.position + headDir;
        transform.LookAt(Define.WCVector3(pos.x, transform.position.y, pos.z));
        transform.position = Define.WCVector3(headPos.x, transform.position.y, headPos.z) + Define.WCVector3(headDir.x, 0f, headDir.z).normalized;

        float dis = Vector3.Distance(prevpos, headPos);
        if (dis > 0.01f)
        {
            // move
            if (anim)
            {
                float angle = Vector3.Angle(headDir, (headPos - prevpos).normalized);
                Vector3 cross = Vector3.Cross(headDir, (headPos - prevpos).normalized);
                if (cross.y < 0f) angle = -angle;
                anim.SetBool("walk", true);
                anim.SetFloat("angle", angle);
                Debug.Log("true dis : " + dis.ToString() + " / angle : " + angle.ToString());
            }
        }
        else
            if (anim) anim.SetBool("walk", false);
        prevpos = headPos;
        prevdir = headDir;
	}
}
