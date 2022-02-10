using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Contrller : MoveControl
{
    public bool padcontrol = false;
    public float speed = 1f;

    private float footTime = 0f;

    protected override void OnStart()
    {
        type = MoveType.move_controller;

        base.OnStart();
    }

    protected override void OnUpdate()
    {
        // 위치 이동 ..
        transform.position = PlayerInput.Instance.CamRig.position;

        if (isMove)
        {
            if (Player.Instance.playerAvatar) Player.Instance.playerAvatar.movekey = false;
            if (PlayerInput.Instance.GetPressTouchpadR())
            {
                MoveTo(true);
            }

            if (PlayerInput.Instance.GetPressTouchpadL())
            {
                MoveTo(false);
            }

            if (PlayerInput.Instance.GetPressUpTouchpadR() || PlayerInput.Instance.GetPressUpTouchpadL())
            {
                if (CamChoice.Instance.effect == true)
                {
                    //CamChoice.Instance.frostEffect.FrostAmount = 0f;
                    StopCoroutine("EffectFadeIn");
                    StopCoroutine("EffectFadeOut");
                    StartCoroutine("EffectFadeOut", effecttime);
                }
            }
        }

        base.OnUpdate();
    }

    void MoveTo(bool right)
    {
        if (Player.Instance.playerAvatar) Player.Instance.playerAvatar.movekey = true;
        if (padcontrol == false)
        {
            if (right)
            {
                PlayerInput.Instance.CamRig.position += Define.WCVector3(PlayerInput.Instance.TrackedObjR.forward.x, 0f,
                    PlayerInput.Instance.TrackedObjR.forward.z).normalized * (Time.deltaTime * speed);
            }
            else
            {
                PlayerInput.Instance.CamRig.position += Define.WCVector3(PlayerInput.Instance.TrackedObjL.forward.x, 0f,
                    PlayerInput.Instance.TrackedObjL.forward.z).normalized * (Time.deltaTime * speed);
            }
        }
        else
        {
            Vector2 padAxis;
            Vector3 forward;
            if (right)
            {
                padAxis = PlayerInput.Instance.GetAxisTouchpadR();
                forward = Define.WCVector3(PlayerInput.Instance.TrackedObjR.forward.x, 0f, PlayerInput.Instance.TrackedObjR.forward.z).normalized;
            }
            else
            {
                padAxis = PlayerInput.Instance.GetAxisTouchpadL();
                forward = Define.WCVector3(PlayerInput.Instance.TrackedObjL.forward.x, 0f, PlayerInput.Instance.TrackedObjL.forward.z).normalized;
            }
            float angle = Vector2.Angle(Define.WCVector2(0f, 1f), padAxis);
            if (padAxis.x < 0f) angle = -angle;
            WMath.Inst.RotateY(ref forward, Mathf.Deg2Rad * angle);
            PlayerInput.Instance.CamRig.position += forward.normalized * (Time.deltaTime * speed);
        }
        if (CamChoice.Instance.effect == true)
        {
            //CamChoice.Instance.frostEffect.FrostAmount = Mathf.Lerp(0.4f, 0f, value);
            if (CamChoice.Instance.frostEffect.FrostAmount == 0f)
            {
                StopCoroutine("EffectFadeIn");
                StopCoroutine("EffectFadeOut");
                StartCoroutine("EffectFadeIn", effecttime);
            }
            //CamChoice.Instance.frostEffect.FrostAmount = effectvalue;
        }
        footTime += Time.deltaTime;
        if (footTime > 0.8f)
        {
            SoundManager.Instance.PlayFootStep2Sound(PlayerInput.Instance.CamHead.position, 0.2f);
            footTime = 0f;
        }
    }
}
