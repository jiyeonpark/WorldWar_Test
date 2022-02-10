using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AutoSingleton<Player> 
{
    public WeaponType weaponType = WeaponType.weapon_none;
    public PlayerAvatar playerAvatar = null;

    private Vehicle vehicle = null;
    private bool firstsetting = false;
    private Rigidbody moveRigidbody = null;
    private Transform moveCollider = null;
    private CapsuleCollider moveColHead = null;

    void FixedUpdate()
    {
        if (moveCollider && PlayerInput.Instance.CamHead)
        {
            moveCollider.localPosition = Define.WCVector3(PlayerInput.Instance.CamHead.localPosition.x, 0f, PlayerInput.Instance.CamHead.localPosition.z);
            if (moveRigidbody.velocity.y > 0f)
                moveRigidbody.velocity = Vector3.zero;
            else
                moveRigidbody.velocity = Define.WCVector3(0f, moveRigidbody.velocity.y, 0f);
            if (moveColHead)
            {
                moveColHead.center = Define.WCVector3(0f, PlayerInput.Instance.CamHead.localPosition.y - 0.4f, 0f);
                //moveColHead.radius = Mathf.Clamp(PlayerInput.Instance.CamHead.localPosition.y / 2f, 0.25f, 0.75f);
            }
        }
    }

	void Update () 
    {
        if (firstsetting == false)
        {
            if (PlayerInput.Instance.CamHead)
            {
                moveRigidbody = PlayerInput.Instance.CamRig.gameObject.AddComponent<Rigidbody>();
                moveRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                GameObject col = new GameObject("MoveCollider");
                moveCollider = col.transform;
                moveCollider.parent = PlayerInput.Instance.CamRig;
                moveCollider.localPosition = Vector3.zero;
                moveCollider.localRotation = Quaternion.identity;
                moveCollider.gameObject.layer = LayerMask.NameToLayer("Player");
                GameObject colsub = new GameObject("MoveCollider_sub");
                colsub.transform.parent = moveCollider;
                colsub.transform.localPosition = Vector3.zero;
                colsub.transform.localRotation = Quaternion.identity;
                colsub.gameObject.layer = LayerMask.NameToLayer("Player");
                CapsuleCollider cc = col.AddComponent<CapsuleCollider>();
                if (cc)
                {
                    moveColHead = cc;
                    cc.center = Define.WCVector3(0f, 1f, 0f);
                    cc.radius = 0.3f;
                    cc.height = 1.2f;
                }
                CapsuleCollider ccsub = colsub.AddComponent<CapsuleCollider>();
                if (ccsub)
                {
                    ccsub.center = Define.WCVector3(0f, 0f, 0f);
                    ccsub.radius = 0.2f;
                    ccsub.height = 1f;
                }
                firstsetting = true;
            }
        }
        //if (moveCollider && PlayerInput.Instance.CamHead)
        //    moveCollider.localPosition = Define.WCVector3(PlayerInput.Instance.CamHead.localPosition.x, 0f, PlayerInput.Instance.CamHead.localPosition.z);
        
        EyeFocusRay();
        if (playerAvatar && PlayerInput.Instance.CamHead)
        {
            playerAvatar.headPos = PlayerInput.Instance.CamHead.position;
            playerAvatar.headDir = PlayerInput.Instance.CamHead.forward;
            playerAvatar.lHandPos = PlayerInput.Instance.TrackedObjL.position - PlayerInput.Instance.CamHead.position;
            playerAvatar.rHandPos = PlayerInput.Instance.TrackedObjR.position - PlayerInput.Instance.CamHead.position;
        }
	}

    void EyeFocusRay()
    {
        if (vehicle)
        {
            vehicle.Focus(false);
            vehicle = null;
        }

        RaycastHit hit;
        if(Physics.Raycast(PlayerInput.Instance.CamHead.position, PlayerInput.Instance.CamHead.forward, out hit))
        {
            vehicle = hit.transform.GetComponent<Vehicle>();
            if(vehicle)
            {
                vehicle.Focus(true);
            }
        }
    }

    public void VehicleBoard(bool value)
    {
        // 탑승체에..
        if(value)
        {// 탑승..
            moveRigidbody.isKinematic = true;
        }
        else
        {// 내림..
            moveRigidbody.isKinematic = false;
        }
    }
}
