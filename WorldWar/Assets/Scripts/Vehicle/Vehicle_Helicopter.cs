using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyType
{
    fly_rot = 0,        // 타겟중심으로 크게 회전..
    fly_path,           // path 지점 이동..
    fly_boardmove,      // 탑승시 이동..
}

public enum FlyMode
{
    fly_none = 0,
    fly_up,
    fly_move,
    fly_down,
}

public class Vehicle_Helicopter : Vehicle
{
    [Space]
    public FlyType type = FlyType.fly_rot;
    public float speed = 1f;

    [Space]
    public Transform target = null;
    public float hight = 5f;

    [Space]
    public Transform[] path = null;
    public iTween.EaseType easetype = iTween.EaseType.linear;

    [Space]
    public Transform rayObj = null;

    private Vector3 orgRot = Vector3.zero;
    private int count = 0;
    private float disTemp = 0f;
    private FlyMode flymode = FlyMode.fly_none;
    private Vector3 startpos = Vector3.zero;
    private Vector3 endpos = Vector3.zero;
    private float movedelta = 0f;
    private float downdis = 0f;

    protected override void OnStart()
    {
        switch(type)
        {
            case FlyType.fly_rot:
                {
                    topObj.position = Define.WCVector3(topObj.transform.position.x, hight, topObj.transform.position.z);
                }break;
            case FlyType.fly_boardmove:
                {
                    topObj.LookAt(Define.WCVector3(path[count].position.x, topObj.position.y, path[count].position.z));
                }break;
        }
        if (PlayerInput.Instance.CamRig)
            orgRot = PlayerInput.Instance.CamRig.eulerAngles;
        FlyTargetMove();

        base.OnStart();
    }

	protected override void OnUpdate()
    {
        FlyTargetRot();
        FlyBoardMove();

        if (board)
            PlayerInput.Instance.CamRig.position = point_in.position;
        point_out.position = Define.WCVector3(200f, 4f, 120f);

        base.OnUpdate();
    }

    public override void OnVehicle()
    {
        switch (type)
        {
            case FlyType.fly_boardmove:
                {
                    if (board == false)
                    {
                        flymode = FlyMode.fly_up;
                        topObj.LookAt(Define.WCVector3(path[count].position.x, topObj.position.y, path[count].position.z));
                    }
                }break;
        }

        base.OnVehicle();
    }

    void FlyTargetRot()
    {
        if (type != FlyType.fly_rot) return;
        if (target == null) return;

        topObj.transform.LookAt(target);
        topObj.transform.position += -Define.WCVector3(topObj.transform.right.x, 0f, topObj.transform.right.z).normalized * (Time.deltaTime * speed);
        if (board)
            PlayerInput.Instance.CamRig.eulerAngles = Define.WCVector3(0f, transform.eulerAngles.y, 0f);
        else if (orgRot != PlayerInput.Instance.CamRig.eulerAngles)
            PlayerInput.Instance.CamRig.eulerAngles = orgRot;
    }

    void FlyTargetMove()
    {
        if (type != FlyType.fly_path) return;
        if (path == null || path.Length <= 0) return;

        iTween.MoveTo(topObj.gameObject, iTween.Hash("path", path, "orienttopath", true, "speed", speed, "easetype", easetype, "looptype", iTween.LoopType.loop));
    }

    void FlyBoardMove()
    {
        if (type != FlyType.fly_boardmove) return;
        if (rayObj == null) return;

        if (flymode == FlyMode.fly_up)
        {
            if (disTemp < hight)
            {
                float delata = Time.deltaTime * 5f;
                disTemp += delata;
                topObj.position += Define.WCVector3(0f, delata, 0f);
            }
            else
            {
                flymode = FlyMode.fly_move;
                movedelta = 0f;
                startpos = topObj.position;
                endpos = Define.WCVector3(path[count].position.x, topObj.position.y, path[count].position.z);
            }
        }
        else if (flymode == FlyMode.fly_move)
        {
            movedelta += Time.deltaTime * speed;
            if (movedelta < 1f)
                topObj.position = Vector3.Lerp(startpos, endpos, movedelta);
            else
            {
                flymode = FlyMode.fly_down;
                downdis = 0f;
                disTemp = 0f;
            }
        }
        else if (flymode == FlyMode.fly_down)
        {
            if(downdis == 0f)
            {
                RaycastHit hit;
                if (Physics.Raycast(rayObj.position, -rayObj.up, out hit))
                    downdis = hit.point.y + 2f;
            }
            else
            {
                if (topObj.position.y > downdis)
                {
                    topObj.position -= Define.WCVector3(0f, Time.deltaTime * 5f, 0f);
                }
                else
                {
                    flymode = FlyMode.fly_none;
                    disTemp = 0f;

                    count++;
                    if (path.Length <= count) count = 0;
                    //OnVehicle();
                }
            }
        }
    }
}
