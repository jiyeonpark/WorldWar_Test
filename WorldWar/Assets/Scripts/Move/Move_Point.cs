using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Point : MoveControl
{
    private Transform rayCube = null;
    private LineRenderer lineRender = null;
    private Transform hitObj = null;

    protected override void OnStart() 
    { 
        type = MoveType.move_point;

        rayCube = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        rayCube.GetComponent<Collider>().enabled = false;
        rayCube.localScale = Vector3.one * 0.1f;
        lineRender = gameObject.GetComponent<LineRenderer>();
        if (lineRender == null)
        {
            lineRender = new GameObject("LinePrediction ").AddComponent<LineRenderer>();
            lineRender.transform.parent = transform;
            lineRender.SetWidth(0.03f, 0.03f);
            //lineRender.SetVertexCount(200);
            lineRender.enabled = false;
        }

        base.OnStart(); 
    }
    
    protected override void OnUpdate() 
    { 
        // 위치 이동 ..
        transform.position = PlayerInput.Instance.CamRig.position;

        if (isMove)
        {
            if (PlayerInput.Instance.GetPressDownTouchpadR())
            {
                lineRender.enabled = true;
            }
            else if (PlayerInput.Instance.GetPressTouchpadR())
            {
                CurveLine(PlayerInput.Instance.TrackedObjR, 5f);
            }
            else if (PlayerInput.Instance.GetPressUpTouchpadR())
            {
                lineRender.enabled = false;
                PlayerInput.Instance.CamRig.position = rayCube.position;
            }

            if (PlayerInput.Instance.GetPressDownTouchpadL())
            {
                lineRender.enabled = true;
            }
            else if (PlayerInput.Instance.GetPressTouchpadL())
            {
                //RayLine(PlayerInput.Instance.TrackedObjL);
                CurveLine(PlayerInput.Instance.TrackedObjL, 5f);
            }
            else if (PlayerInput.Instance.GetPressUpTouchpadL())
            {
                lineRender.enabled = false;
                PlayerInput.Instance.CamRig.position = rayCube.position;
            }
        }

        base.OnUpdate();
    }

    void RayLine(Transform obj, bool move = false)
    {
        RaycastHit hit;
        if (Physics.Raycast(obj.position, obj.forward, out hit))
        {
            hitObj = null;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer(movelayer))
            {
                rayCube.position = hit.point;
                lineRender.SetPosition(0, obj.transform.position);
                lineRender.SetPosition(1, hit.point);
                hitObj = hit.transform;
            }
        }
    }

    void CurveLine(Transform obj, float force)
    {
        if (obj)
        {
            Vector3 shotOrigin = obj.position;
            int predictionCount = 200;
            float predictionGap = 0.01f;
            Vector3 displacePos = Vector3.zero;
            Vector3 forceVector = obj.forward * force * 70f;

            lineRender.enabled = true;
            lineRender.SetVertexCount(200);

            bool trigger = false;
            int pointCount = 0;
            for (float i = predictionGap; i < predictionGap * predictionCount; i += predictionGap)
            {
                BallUtil.TrajectoryCalculate.GetDisplacement(ref displacePos.x, ref displacePos.y, ref displacePos.z,
                    shotOrigin, forceVector.x, forceVector.y, forceVector.z, 0, 0, 0, 0.25f, i);
                lineRender.SetPosition(pointCount, displacePos);
                if (Physics.CheckBox(displacePos, Vector3.one * 0.1f, Quaternion.identity, 1 << LayerMask.NameToLayer(movelayer)) == true)
                {
                    rayCube.position = displacePos;
                    lineRender.SetVertexCount(pointCount);
                    trigger = true;
                    break;
                }
                pointCount++;
            }
            if (trigger == false)
                lineRender.SetVertexCount(5);
        }
    }
}
