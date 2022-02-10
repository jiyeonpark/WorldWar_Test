using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAxeObject : MonoBehaviour 
{
    public bool isMine = false;
    public bool isBlocked = false;
    public GameObject rotationAxis;
    [HideInInspector]
    public Rigidbody myRigidbody;
    public MeshRenderer myMeshRenderer;
    [HideInInspector]
    public bool hit;
    [HideInInspector]
    public bool falling;
    [Tooltip("Base damage of arrow without damage add amount.")]
    public float damage = 50f;
    [Tooltip("Force to apply to rigidbody that is hit with arrow.")]
    public float force = 3f;
    [Tooltip("Time that arrow object stays in scene after hitting object.")]
    public float waitDuration = 30f;
    private Collider hitCol;
    [HideInInspector]
    public BoxCollider myBoxCol;
    [Tooltip("Scale of the arrow object.")]
    public Vector3 scale = Vector3.zero;
    [Tooltip("Initial size of the arrow collider (increased after hit to make pick up easier).")]
    public Vector3 initialColSize;
    [Tooltip("True if helper gizmos for arrow object should be shown to assist setting script values.")]
    public bool drawHelperGizmos;
    public RaycastHit arrowRayHit;
    //[Tooltip("Distance in front of arrow to check for hits (scaled up at higher velocities).")]
    //public float hitCheckDist = 0.4f;
    [Tooltip("Layers that the arrow will collide with.")]
    public LayerMask rayMask_Mine;
    public LayerMask rayMask_Enemy;
    //set from WeaponBehavior.cs FireOneShot() function to scale forward hit detection raycast based on arrow release velocity
    //to large of a raycast distance at low velocities makes arrow "jump" forward to impact point
    //[HideInInspector]
    //public float velFactor;
    public Vector3 prevVelocity;

    private float hitTime;
    private float startTime;
    private Vector3 prevPos = Vector3.zero;
    private Vector3 prevDir = Vector3.zero;
    private GameObject emptyObject;

    //public XftWeapon.XWeaponTrail[] _listTrail = null;
    private TrailRenderer trail = null;

    void Start()
    {
        //FPSPlayerComponent = FPSPlayer.Instance.gameObject.transform.GetComponent<FPSPlayer>();
        //_listTrail = transform.GetComponentsInChildren<XftWeapon.XWeaponTrail>();
        //if (GameManager.Instance.graphicIdx == Define.GraphicIdx.Low)
        //{
        //    for (int i = 0; i < _listTrail.Length; i++)
        //        _listTrail[i].gameObject.SetActive(false);
        //}
    }

    public void InitializeProjectile(bool isCharterArrow)
    {
        hit = false;
        falling = false;
        isMine = isCharterArrow;
        isBlocked = false;
        myRigidbody = GetComponent<Rigidbody>();
        myBoxCol = GetComponent<BoxCollider>();
        myMeshRenderer.enabled = false;
        //reset collider size to original, smaller arrow sized value for more realistic, narrower collisions
        myBoxCol.size = initialColSize;
        startTime = Time.time;
        myRigidbody.isKinematic = false;
        myBoxCol.isTrigger = true;
        transform.gameObject.tag = "Untagged";//don't allow arrow to be grabbed in flight
        if (isMine)
            gameObject.layer = LayerMask.NameToLayer("PlayerWeapon");
        else
            gameObject.layer = LayerMask.NameToLayer("EnemyWeapon");
        transform.parent = AzuObjectPool.instance.transform;
        transform.localScale = scale;
        prevPos = transform.position;
        prevDir = transform.forward;
        trail = GetComponent<TrailRenderer>();
        ////GetComponent<TrailRenderer>().enabled = true;
        if (trail) trail.enabled = false;
        StartCoroutine(ReserveReturnArrow(waitDuration));
        SoundManager.Instance.PlayHandAxe(transform.position);
    }

    IEnumerator ReserveReturnArrow(float dur)
    {
        yield return new WaitForSeconds(dur);
        transform.SetParent(AzuObjectPool.instance.transform);
        gameObject.SetActive(false);
    }

    IEnumerator HandAxeSound()
    {
        SoundManager.Instance.PlayHandAxe(transform.position);
        float vel = myRigidbody.velocity.magnitude;
        Debug.Log(vel);
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            SoundManager.Instance.PlayHandAxe(transform.position);
            yield return new WaitForSeconds(0.2f);
        }
        yield return null;
    }

    void OnDisable()
    {
        GetComponent<Rigidbody>().Sleep();
    }

    void OnDrawGizmos()
    {
        if (drawHelperGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.04f);
            Gizmos.color = Color.green;
            //Gizmos.DrawSphere(transform.position + transform.forward * (hitCheckDist + ((hitCheckDist * 1f) * velFactor)), 0.04f);
            Gizmos.DrawSphere(transform.position + transform.forward, 0.04f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(arrowRayHit.point, 0.06f);
        }
    }

    void Update()
    {
        if (Time.timeScale == 0 || falling == true) return;
        if (startTime < Time.time)
        {
            myMeshRenderer.enabled = true;//enable mesh renderer after delay to prevent arrow from blocking player view if near clip plane is small
        }
        if (!hit && !isBlocked)
        {
            //Check for arrow hit
            LayerMask lm = new LayerMask();
            if (isMine)
                lm = rayMask_Mine;
            else
                lm = rayMask_Enemy;
            //if (Physics.Raycast(transform.position, transform.forward, out arrowRayHit, hitCheckDist + ((hitCheckDist * 1f) * velFactor), lm, QueryTriggerInteraction.Ignore))
            float dis = Vector3.Distance(prevPos, transform.position);
            if (Physics.Raycast(prevPos, prevDir, out arrowRayHit, dis, lm, QueryTriggerInteraction.Ignore))
            {
                HitTarget(arrowRayHit);
                SoundManager.Instance.PlayArrowHitSound(arrowRayHit.point);
            }
            prevDir = (transform.position - prevPos).normalized;
            prevPos = transform.position;
            if (!falling && myRigidbody.velocity.magnitude > 0.01f)
            {
                LoopRotation(0, 900f);
            }
        }
        else
        {
            if (hitTime + waitDuration < Time.time)
            {//wait duration has elapsed, recycle arrow object
                transform.parent = AzuObjectPool.instance.transform;
                gameObject.SetActive(false);
                DestroyImmediate(emptyObject);
            }
            if (hitCol && !hitCol.enabled)
            {//make arrow fall to ground if hit collider is removed or disabled
                falling = true;
                this.After(5f,
                    () =>
                    {
                        myRigidbody.isKinematic = false;
                        myBoxCol.isTrigger = false;
                    });
            }
        }
    }

    void LoopRotation(float rotPerSecX, float rotPerSecY)
    {
        float frameTime = Time.time;



        float currRotY = WMath.Inst.GetRotY(transform) * Mathf.Rad2Deg;
        float rotNowY = rotPerSecY * frameTime;  //현재 돌아야 할 상대각도
        float newRotY = currRotY + rotNowY;
        if (newRotY > 360)
            newRotY -= 360;
        else if (newRotY < -360)
            newRotY += 360;

        float currRotX = WMath.Inst.GetRotX(transform) * Mathf.Rad2Deg;
        float rotNowX = rotPerSecX * frameTime;  //현재 돌아야 할 상대각도
        float newRotX = currRotX + rotNowX;
        if (newRotX > 360)
            newRotX -= 360;
        else if (newRotX < -360)
            newRotX += 360;
        //transform.RotateAround(transform.position, transform.right, newRotX);

        rotationAxis.transform.Rotate(rotPerSecY * Time.deltaTime, 0, rotPerSecX * Time.deltaTime, Space.Self);

    }

    public void HitTarget(RaycastHit rayHit)
    {
        arrowRayHit = rayHit;
        if (!hit
        && arrowRayHit.collider
        && !arrowRayHit.collider.gameObject.GetComponent<HandAxeObject>())
        {
            hitCol = arrowRayHit.collider;
            if (hitCol.gameObject.layer == gameObject.layer)
                return;
            myRigidbody.isKinematic = true;
            //transform.gameObject.tag = "Usable";//allow arroe to be picked up
            //increase collider size to make is easier to retrieve later
            myBoxCol.size = Define.WCVector3(initialColSize.x, initialColSize.y, initialColSize.z);
            ////AmmoPickupComponent.enabled = true;
            transform.position = arrowRayHit.point;

            if (hitCol.GetComponent<Rigidbody>() || (hitCol.transform.parent != null && hitCol.transform.parent.GetComponent<Rigidbody>()))
            {//or other moving objects?
                if (hitCol.GetComponent<Rigidbody>())
                {
                    hitCol.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
                }
                else if (hitCol.transform.parent != null && hitCol.transform.parent.GetComponent<Rigidbody>())
                {//do additional check for rigidbody on parent object if one not found on hit collider
                    hitCol.transform.parent.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
                }
                //Create empty parent object for arrow object to prevent arrow from inheriting scale of hit collider if it is skewed or uneven
                emptyObject = new GameObject();
                emptyObject.transform.position = arrowRayHit.point;
                emptyObject.transform.rotation = hitCol.transform.rotation;
                emptyObject.transform.parent = hitCol.transform;
                transform.parent = emptyObject.transform;
                //empty obj will be destroyed after waitDuration, but obj pool might reuse the prefab and leave the parent obj stranded
                //Destroy(emptyObject.gameObject, waitDuration + 1f);
                if (trail) trail.enabled = false;
            }

            if (hitCol.gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                // 일반 오브젝트..
                SoundManager.Instance.PlayArrowHitSound(transform.position);
            }
            else if (isMine && hitCol.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // 내 화살에 상대방이 맞을경우..
                DamageCheck dc = hitCol.gameObject.GetComponent<DamageCheck>();
                if (dc)
                {
                    if (dc.ai)
                        dc.ai.Damage(damage);
                }
                SoundManager.Instance.PlayArrowHitSound(transform.position);
                EffectManager.Instance.DecalEffect(arrowRayHit.point, arrowRayHit.normal, arrowRayHit.collider.tag);
            }
            else if (!isMine && hitCol.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // 상대방화살에 내가 맞았을경우..
                GameEvents.EffectDamage();
                SoundManager.Instance.PlayArrowHitSound(transform.position);
            }
            else if (isMine && hitCol.gameObject.layer == LayerMask.NameToLayer("Explosive"))
            {
                // 폭발물체..
            }

            hitTime = Time.time;
            hit = true;

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("PlayerWeapon"))
            && !hit && !isBlocked)
        {
            //Mace mace = other.GetComponent<Mace>();
            //mace.ArrowBlocked();
            ////isBlocked
            GetComponent<Rigidbody>().Sleep();
            //Vector3 swingVec = mace.GetMaceSwingList()[mace.GetMaceSwingList().Count - 1] - mace.GetMaceSwingList()[0];
            //GetComponent<Rigidbody>().AddForce(swingVec * 300f);
            isBlocked = true;
            myBoxCol.isTrigger = false;
            //myBoxCol.size = Define.WCVector3(0.1f, 0.3f, 0.5f);
            //if (trail) trail.enabled = false;
            //GameObject particle = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.effect_AttackWeaponBurst, transform.position, Quaternion.identity) as GameObject;
            //this.After(1f,
            //    () =>
            //    {
            //        particle.gameObject.SetActive(false);
            //    }
            //    );
            //SoundManager.Instance.PlayMetalSound(transform.position);
            hitTime = Time.time;
            StopCoroutine(ReserveReturnArrow(waitDuration));
        }
    }
}
