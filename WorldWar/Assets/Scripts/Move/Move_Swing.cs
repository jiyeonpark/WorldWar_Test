using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Swing : MoveControl
{
    public bool focus = false;
    public float power = 1f;

    private Vector3 prevposL = Vector3.zero;
    private Vector3 prevposR = Vector3.zero;
    private Move_Contrller moveCtrlScript = null;
    private Vector3 movedelta = Vector3.zero;
    private float footTime = 0f;
    private Rigidbody playerRigidbody = null;

    protected override void OnStart()
    {
        type = MoveType.move_swing;
        moveCtrlScript = GetComponent<Move_Contrller>();
        if (moveCtrlScript.enabled == false)
            moveCtrlScript = null;

        base.OnStart();
    }

    protected override void OnUpdate()
    {
        // 위치 이동 ..
        transform.position = PlayerInput.Instance.CamRig.position;

        if (isMove)
        {
            if (moveCtrlScript != null)
                moveCtrlScript.enabled = true;
            movedelta = Vector3.zero;
            bool move = false;
            if (PlayerInput.Instance.GetPressGripR())
            {
                bool m = SwingR();
                if (move == false)
                    move = m;
            }

            if (PlayerInput.Instance.GetPressGripL())
            {
                bool m = SwingL();
                if (move == false)
                    move = m;
            }

            if (move) Move(movedelta);
            if (Player.Instance.playerAvatar) Player.Instance.playerAvatar.movekey = move;

            if (PlayerInput.Instance.GetPressUpGripR() || PlayerInput.Instance.GetPressUpGripL())
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
        prevposL = PlayerInput.Instance.TrackedObjL.localPosition;
        prevposR = PlayerInput.Instance.TrackedObjR.localPosition;

        base.OnUpdate();
    }

    bool SwingL()
    {
        if (prevposL == Vector3.zero) prevposL = PlayerInput.Instance.TrackedObjL.localPosition;
        float dis = Vector3.Distance(prevposL, PlayerInput.Instance.TrackedObjL.localPosition);
        if (dis > 0.01f)
        {
            //Debug.Log("SwingL : " + dis.ToString());
            float changespeed = Time.deltaTime * power;
            //if(dis > 0.05f)
            //    changespeed = (Time.deltaTime * speed) * 2f;
            if (focus)
                movedelta += Define.WCVector3(PlayerInput.Instance.CamHead.forward.x, 0f, PlayerInput.Instance.CamHead.forward.z).normalized * changespeed;
            else
                movedelta += Define.WCVector3(PlayerInput.Instance.TrackedObjL.forward.x, 0f, PlayerInput.Instance.TrackedObjL.forward.z).normalized * changespeed;
            return true;
        }
        return false;
    }
    bool SwingR()
    {
        if (prevposR == Vector3.zero) prevposR = PlayerInput.Instance.TrackedObjR.localPosition;
        float dis = Vector3.Distance(prevposR, PlayerInput.Instance.TrackedObjR.localPosition);
        if (dis > 0.01f)
        {
            //Debug.Log("SwingR : " + dis.ToString());
            float changespeed = Time.deltaTime * power;
            //if (dis > 0.05f)
            //    changespeed = (Time.deltaTime * speed) * 2f;
            if (focus)
                movedelta += Define.WCVector3(PlayerInput.Instance.CamHead.forward.x, 0f, PlayerInput.Instance.CamHead.forward.z).normalized * changespeed;
            else
                movedelta += Define.WCVector3(PlayerInput.Instance.TrackedObjR.forward.x, 0f, PlayerInput.Instance.TrackedObjR.forward.z).normalized * changespeed;
            return true;
        }
        return false;
    }

    void Move(Vector3 delta)
    {
        if (playerRigidbody == null)
        {
            playerRigidbody = PlayerInput.Instance.CamRig.GetComponent<Rigidbody>();
            return;
        }
        //PlayerInput.Instance.CamRig.position += delta;
        playerRigidbody.AddForce(delta);
        if (moveCtrlScript != null && moveCtrlScript.enabled == true)
            moveCtrlScript.enabled = false;
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
        if (footTime > 0.3f)
        {
            SoundManager.Instance.PlayFootStepSound(PlayerInput.Instance.CamHead.position);
            footTime = 0f;
        }
    }
}
