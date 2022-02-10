using UnityEngine;
using System.Collections;

public class LocalView : MonoBehaviour 
{
    public bool first = true;
    public changeY_type changeYType = changeY_type.type_changeup;
    public float localUIDis = 5f;
    public Vector3 offsetPos = Vector3.zero;
    public Vector3 offsetRot = Vector3.zero;

    public enum changeY_type
    {
        type_none = 0,
        type_changeup,
        type_change,
        type_slowface,
    }

    private Vector3 orgCamRigPos = Vector3.zero;
    private bool firstView = false;
    private float lerp = 0f;

	void Start () 
    {
        firstView = false;
        transform.localRotation = Quaternion.identity;
	}

    void Update()
    {
        if(PlayerInput.Instance.CamRig)
        {
            if (PlayerInput.Instance.CamRig.position != orgCamRigPos || first == false)
                ShowLocalView();
        }
    }

    public void ShowLocalView()
    {
        if (PlayerInput.Instance.CamRig == null || PlayerInput.Instance.CamHead == null) return;
        if (first == true && firstView == true) return;

        orgCamRigPos = PlayerInput.Instance.CamRig.position;
        Vector3 dir = Vector3.zero;
        if (changeYType == changeY_type.type_changeup)
        {
            dir = PlayerInput.Instance.CamHead.forward;
            if (dir.y < 0)
                dir = Define.WCVector3(PlayerInput.Instance.CamHead.forward.x, 0f, PlayerInput.Instance.CamHead.forward.z).normalized;
            transform.position = PlayerInput.Instance.CamHead.position + (dir * localUIDis) + offsetPos;
            transform.LookAt(transform.position + dir * localUIDis);
        }
        else if (changeYType == changeY_type.type_change)
        {
            dir = PlayerInput.Instance.CamHead.forward;
            transform.position = PlayerInput.Instance.CamHead.position + (dir * localUIDis) + offsetPos;
            transform.LookAt(transform.position + dir * localUIDis);
        }
        else if (changeYType == changeY_type.type_slowface)
        {
            // rot 좌우..
            float speed = 0.1f;
            float angle = Vector3.Angle(transform.forward, PlayerInput.Instance.CamHead.forward);
            if (angle > 5f)
            {
                lerp += Time.deltaTime * speed;
                if (lerp > 1f) lerp = 1f;
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerInput.Instance.CamHead.rotation, lerp);
            }
            else
                lerp = 0f;
        }
        else
        {
            dir = Define.WCVector3(PlayerInput.Instance.CamHead.forward.x, 0f, PlayerInput.Instance.CamHead.forward.z).normalized;
            //transform.position = PlayerInput.Instance.CamRig.position + (dir * localUIDis) + offsetPos;
            transform.position = Define.WCVector3(PlayerInput.Instance.CamHead.position.x, PlayerInput.Instance.CamRig.position.y, PlayerInput.Instance.CamHead.position.z) + (dir * localUIDis) + offsetPos;
            transform.LookAt(transform.position + dir * localUIDis);
        }
        
        transform.localEulerAngles += offsetRot;
        firstView = true;
    }

    void LateUpdate()
    {
        if (PlayerInput.Instance.CamRig == null || PlayerInput.Instance.CamHead == null) return;
        if (first == true && firstView == true) return;

        if (changeYType == changeY_type.type_slowface)
        {
            // pos
            transform.position = PlayerInput.Instance.CamHead.position;
        }
    }
}
