using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_Tank : Vehicle
{
    public Transform turret = null;
    public Transform body = null;
    public Transform[] path = null;
    public float movespeed = 1f;
    public float rotspeed = 1f;
    public float turretspeed = 1f;
    public bool inverse = false;

    private Vector3 delta = Vector3.zero;
    //private AudioSource sound = null;

    protected override void OnStart()
    {
        //if (path == null || path.Length <= 0)
        //{
        //    sound = GetComponent<AudioSource>();
        //    if(sound) sound.enabled = false;
        //}
        MoveAutoStart();

        base.OnStart();
    }

    protected override void OnUpdate()
    {
        Turrent();
        MoveAuto();

        if (PlayerInput.Instance.GetPressTouchpadL())
        {
            MoveTo();
            //if (sound && sound.enabled == false) sound.enabled = true;
        }
        //if (PlayerInput.Instance.GetPressUpTouchpadL())
        //{
        //    if (sound && sound.enabled == true) sound.enabled = false;
        //}

        base.OnUpdate();
    }

    void Turrent()
    {
        if (board == false) return;

        Vector3 t = -Define.WCVector3(turret.forward.x, 0f, turret.forward.z).normalized;
        Vector3 c = Define.WCVector3(PlayerInput.Instance.CamHead.forward.x, 0f, PlayerInput.Instance.CamHead.forward.z).normalized;
        Vector3 cross = Vector3.Cross(t, c);
        float angle = Vector3.Angle(t, c);
        if (angle > 5f)
        {
            if (cross.y < 0f)
                turret.localEulerAngles -= Define.WCVector3(0f, turretspeed, 0f);
            else
                turret.localEulerAngles += Define.WCVector3(0f, turretspeed, 0f);
        }

        // 포신움직임 (나중에 포신 따로만들면 적용..)
        //t = -Define.WCVector3(0f, turret.forward.y, turret.forward.z).normalized;
        //c = Define.WCVector3(0f, PlayerInput.Instance.CamHead.forward.y, PlayerInput.Instance.CamHead.forward.z).normalized;
        //angle = Vector3.Angle(t, c);
        //if (angle > 5f)
        //{
        //    float eulerangles = turret.localEulerAngles.x;
        //    if (eulerangles > 90f)
        //        eulerangles = eulerangles - 360f;
        //    if (PlayerInput.Instance.CamHead.forward.y <= 0f && eulerangles > -10f)
        //        turret.localEulerAngles -= Define.WCVector3(turretspeed, 0f, 0f);
        //    else if (PlayerInput.Instance.CamHead.forward.y > 0f && eulerangles < 15f)
        //        turret.localEulerAngles += Define.WCVector3(turretspeed, 0f, 0f);
        //}
    }


    void MoveTo()
    {
        if (board == false) return;
        if (path != null && path.Length > 0) return;

        float inv = 1;
        if (inverse) inv = -inv;
        Vector2 padAxis = PlayerInput.Instance.GetAxisTouchpadL();
        Vector3 forward = Define.WCVector3(PlayerInput.Instance.TrackedObjL.forward.x, 0f, PlayerInput.Instance.TrackedObjL.forward.z).normalized;

        float angle = Vector2.Angle(Define.WCVector2(0f, 1f), padAxis);
        Vector3 delta = Vector3.zero;
        if (angle < 60f) // 전진..
            delta += body.forward.normalized * movespeed * inv;
        else if (angle > 120f) // 후진..
            delta -= body.forward.normalized * movespeed * inv;
        if (angle > 30f && angle < 150f)
        {
            if (padAxis.x < 0f) // 좌측회전..
                body.localEulerAngles -= Define.WCVector3(0f, rotspeed, 0f);
            else // 우측회전..
                body.localEulerAngles += Define.WCVector3(0f, rotspeed, 0f);
        }
        body.position += delta;
        turret.position += delta;
        PlayerInput.Instance.CamRig.position += delta;
    }

    void MoveAutoStart()
    {
        if (path == null || path.Length <= 0) return;

        delta = turret.position - body.position;
        iTween.MoveTo(body.gameObject, iTween.Hash("path", path, "orienttopath", true, "speed", movespeed, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
    }

    void MoveAuto()
    {
        if (path == null || path.Length <= 0) return;

        turret.position = body.position + delta;
        if (board)
            PlayerInput.Instance.CamRig.position = point_in.position;
    }
}
