using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowObject : MonoBehaviour
{
    public GameObject arrowFireObj;
    public bool isMine = false;
    public bool isBlocked = false;
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

    void Start()
    {
        //FPSPlayerComponent = FPSPlayer.Instance.gameObject.transform.GetComponent<FPSPlayer>();
    }

    public void InitializeProjectile(bool isCharterArrow)
    {
        arrowFireObj.transform.localPosition = new Vector3(0, 0, 0.05f);
        arrowFireObj.gameObject.SetActive(false);
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
        transform.SetParent(AzuObjectPool.instance.transform);
        transform.localScale = scale;
        prevPos = transform.position;
        prevDir = transform.forward;
        GetComponent<TrailRenderer>().enabled = true;
        StartCoroutine(ReserveReturnArrow(waitDuration));
    }

    IEnumerator ReserveReturnArrow(float dur)
    {
        yield return new WaitForSeconds(dur);
        transform.SetParent(AzuObjectPool.instance.transform);
        gameObject.SetActive(false);
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
        if (myRigidbody != null && myRigidbody.velocity != Vector3.zero)
            prevVelocity = myRigidbody.velocity;
        if (Time.timeScale == 0) return;
        if (transform.position.y < -20f)
        {
            gameObject.SetActive(false);
        }
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
            //float dis = hitCheckDist + ((hitCheckDist * 1f) * velFactor);
            //if (Physics.Raycast(transform.position, transform.forward, out arrowRayHit, dis, lm, QueryTriggerInteraction.Ignore))
            float dis = Vector3.Distance(prevPos, transform.position);
            if (Physics.Raycast(prevPos, prevDir, out arrowRayHit, dis, lm, QueryTriggerInteraction.Ignore))
            {
                HitTarget(arrowRayHit);
            }
            prevPos = transform.position;
            prevDir = transform.forward;
            if (!falling && myRigidbody.velocity.magnitude > 0.01f)
            {
                transform.rotation = Quaternion.LookRotation(myRigidbody.velocity);//make arrow always point in direction of movement
            }
        }
        else
        {
            if (hitTime + waitDuration < Time.time)
            {//wait duration has elapsed, recycle arrow object
                transform.SetParent(AzuObjectPool.instance.transform);
                gameObject.SetActive(false);
            }
            if (hitCol && !hitCol.enabled)
            {//make arrow fall to ground if hit collider is removed or disabled
                //myRigidbody.isKinematic = false;  // 죽은직후 물리가 먹어서 화살이 어디론가 사라짐...(그래서 코드 주석처리)
                myBoxCol.isTrigger = false;
                falling = true;
            }
        }
    }

    public void HitTarget(RaycastHit rayHit)
    {
        arrowRayHit = rayHit;
        if (!hit
        && arrowRayHit.collider
        && !arrowRayHit.collider.gameObject.GetComponent<ArrowObject>())
        {
            hitCol = arrowRayHit.collider;
            if (hitCol.gameObject.layer == gameObject.layer)
                return;

            myRigidbody.isKinematic = true;
            //increase collider size to make is easier to retrieve later
            myBoxCol.size = new Vector3(initialColSize.x * 2f, initialColSize.y * 2f, initialColSize.z);
            //AmmoPickupComponent.enabled = true;
            //transform.LookAt(arrowRayHit.point + ((arrowRayHit.point - transform.position).normalized * 10f));
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

                transform.SetParent(hitCol.transform);
                this.After(waitDuration + 1f,
                () =>
                {
                    hitCol.transform.SetParent(AzuObjectPool.instance.transform);
                    hitCol.gameObject.SetActive(false);
                }
                );
                GetComponent<TrailRenderer>().enabled = false;
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
            //Vector3 swingVec = Vector3.zero;
            //Mace mace = other.GetComponent<Mace>();
            //if (mace)
            //{
            //    mace.ArrowBlocked();
            //    //isBlocked
            //    swingVec = mace.GetMaceSwingList()[mace.GetMaceSwingList().Count - 1] - mace.GetMaceSwingList()[0];
            //}
            GetComponent<Rigidbody>().Sleep();
            //GetComponent<Rigidbody>().AddForce(swingVec * 300f);
            isBlocked = true;
            myBoxCol.isTrigger = false;
            //GetComponent<TrailRenderer>().enabled = false;
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

