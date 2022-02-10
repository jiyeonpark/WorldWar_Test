using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Bow : Weapon
{
    public GameObject bowPivot;
    public GameObject bowArrowPivot;
    public GameObject arrowPivot;

    public bool renderLineVisible = true;

    [HideInInspector]
    public Ray weaponRay;//ray pointed in weapon facing direction
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    public float fireHoldMult;
    [HideInInspector]
    public float fireHoldTimer;
    [HideInInspector]
    public bool fireOnReleaseState;
    [HideInInspector]
    public bool releaseAnimState;
    [HideInInspector]
    public bool pullAnimState;

    private GameObject projectile = null;
    [Tooltip("True if weapon should fire after releasing fire button.")]
    public bool fireOnRelease = false;
    [Tooltip("Amount of projectiles to be fired per shot ( > 1 for shotguns).")]
    public int projectileCount = 1;
    [Tooltip("Force to apply to projectile after firing (shot velocity).")]
    public float projectileForce;
    [Tooltip("True if forward velocity of the projectile should be tied to how long fire button is held.")]
    public bool pullProjectileForce;
    [Tooltip("Vertical rotation to add to fired projectile.")]
    public float projRotUp = 0;
    [Tooltip("Horizontal rotation to add to fired projectile.")]
    public float projRotSide = 0;

    public float minimumProjForce = 1.5f;
    private Rigidbody projBody;

    LineRenderer debugLinePrediction = null;
    public ArrowObject tempArrowObj;
    public float bowScale = 1f;
    public float arrowlength = 1f;

    private Animator ani = null;
    private bool attachArrow = false;
    private Vector3 preArrowPos = Vector3.zero;
    private Transform leftHand = null;   // 왼손..
    private Transform rightHand = null;  // 오른손..

    protected override void OnStart()
    {
        type = WeaponType.weapon_bow;
        if (weaponObj)
            ani = weaponObj.GetComponent<Animator>();

        base.OnStart();
    }

    protected override void OnUpdate()
    {
        if (leftHand == null) leftHand = PlayerInput.Instance.TrackedObjL;
        if (rightHand == null) rightHand = PlayerInput.Instance.TrackedObjR;
        if (PlayerInput.Instance.GetTouchTriggerR())
        {
            BowPull();
        }
        if (PlayerInput.Instance.GetTouchUpTriggerR())
        {
            float dis = Vector3.Distance(leftHand.position, rightHand.position);
            if (dis < 0.3f)
            {
                BowReload();
                ani.Play("archerybefore", 0, 0f);
            }
            else
            {
                BowShot();
                this.After(0.3f,
                    () =>
                    {
                        BowReload();
                    }
                    );
            }

        }

        base.OnUpdate();
    }

    protected override void OnLateUpdate()
    {
        if (PlayerInput.Instance.CamRig == null)
            return;

        arrowPivot.transform.position = PlayerInput.Instance.TrackedObjR.position;
        arrowPivot.transform.rotation = PlayerInput.Instance.TrackedObjR.rotation;

        base.OnLateUpdate();
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    void BowAnim(float value)
    {   // 0~1 사이값..

        if (ani)
        {
            ani.Play("archery", 0, value);
        }
    }

    void BowRot()
    {   // 오른손에 의해서 z축 변경이 되지 않게 하기 위함..
        Vector3 target = weaponObj.position + (weaponObj.position - arrowPivot.transform.position).normalized * 5f;
        weaponObj.LookAt(target);
        weaponObj.localEulerAngles = new Vector3(weaponObj.localEulerAngles.x, weaponObj.localEulerAngles.y, 0f);
    }

    IEnumerator BowReposit()
    {   // 화살대 자연스럽게 원래자리로 ..
        yield return null;

        float rate = 0;
        Vector3 startrot = weaponObj.localEulerAngles;
        if (startrot.x > 180f) startrot.x = startrot.x - 360f;
        if (startrot.y > 180f) startrot.y = startrot.y - 360f;
        if (startrot.z > 180f) startrot.z = startrot.z - 360f;
        float value = (1 / Vector3.Angle(startrot, Vector3.zero)) * 2f;

        while (rate <= 1)
        {
            rate += value;
            weaponObj.localEulerAngles = Vector3.Slerp(startrot, Vector3.zero, rate);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void BowReload()
    {
        if (attachArrow == true)
        {
            attachArrow = false;
            weaponObj.localPosition = Vector3.zero;
            weaponObj.localEulerAngles = Vector3.zero;
            weaponObj.localScale = Vector3.one;
            arrowPivot.SetActive(true);
            bowArrowPivot.SetActive(false);
        }
        preArrowPos = arrowPivot.transform.position;
    }

    void BowPull()
    {
        if (attachArrow == false)
        {
            //float dis = Vector3.Distance(weaponObj.position, arrowPivot.transform.position);
            float dis = Vector3.Distance(leftHand.position, rightHand.position);
            if (dis < (0.3f * bowScale))
            {
                // 활에 붙어있는 화살을 보여줌..
                attachArrow = true;
                arrowPivot.SetActive(false);
                bowArrowPivot.SetActive(true);

                SoundManager.Instance.PlayBowPullStart(weaponObj.position);
            }
            StopCoroutine("BowReposit");
        }
        if (attachArrow == true)
        {
            //float rate = (Vector3.Distance(weaponObj.position, arrowPivot.transform.position) - (0.01f * bowScale)) / arrowlength; // 0~1
            float rate = (Vector3.Distance(leftHand.position, rightHand.position) - (0.01f * bowScale)) / arrowlength; // 0~1
            if (rate < 0f) rate = 0f;
            if (rate > 1f) rate = 1f;
            BowAnim(rate);
            Pull(rate);
            BowRot();
            if (Vector3.Distance(preArrowPos, arrowPivot.transform.position) > 0.01f)
            {
                PlayerInput.Instance.TriggerHapticPulseR();
                PlayerInput.Instance.TriggerHapticPulseL();
                preArrowPos = arrowPivot.transform.position;
                SoundManager.Instance.PlayBowPull(weaponObj.position);
            }
        }
    }

    void BowShot()
    {
        if (ani && attachArrow)
        {
            Shot(false);
            ani.Play("archeryend");
            StartCoroutine("BowReposit");
            StartCoroutine("BowShaking");
        }
    }
    IEnumerator BowShaking()
    {
        yield return null;

        ushort shakingValue = 2000;
        while (shakingValue > 0 && shakingValue <= 2000)
        {
            PlayerInput.Instance.TriggerHapticPulseL(shakingValue);
            shakingValue -= 800;

            yield return null;
        }
    }


    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// <param name="isFire"></param>

    public void Shot(bool isFire = false)
    {
        if (gameObject.activeInHierarchy == false)
            return;

        Vector3 pos = weaponRay.origin;
        Quaternion quat = Quaternion.FromToRotation(weaponRay.origin, weaponRay.origin + weaponRay.direction);
        float projectileForceAmt = projectileForce * fireHoldMult + minimumProjForce;
        direction = SprayDirection();
        Vector3 force = direction * projectileForceAmt * 70;
        if (StoreManager.sInstance.device == DeviceType.Oculus)
            force *= 0.8f;

        StopCoroutine(FireOneShot(true, isFire, pos, quat, force));
        StartCoroutine(FireOneShot(true, isFire, pos, quat, force));
        //gamePlayer.CmdShoot(pos, quat, force);
    }

    IEnumerator FireOneShot(bool isMine, bool isFire, Vector3 pos, Quaternion quat, Vector3 force)
    {
        SoundManager.Instance.PlayBowFireSound(transform.position);
        bowArrowPivot.gameObject.SetActive(false);

        //fire the number of projectiles defined by projectileCount 
        for (float i = 0; i < projectileCount; i++)
        {
            direction = SprayDirection();
            ArrowObject ArrowRef = null;

            if (renderLineVisible && isMine)
                debugLinePrediction.gameObject.SetActive(false);
            //projectile = AzuObjectPool.instance.SpawnPooledObj((int)projectilePoolIndex, mainCamTransform.position + (mainCamTransform.forward * playerDist), mainCamTransform.rotation) as GameObject;
            projectile = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.obj_Arrow, pos, quat) as GameObject;
            projectile.transform.LookAt(pos + force);

            //화살 생성 커맨드
            ////gamePlayer.CmdCreate(projectilePoolIndex, weaponRay.origin, Quaternion.FromToRotation(weaponRay.origin, weaponRay.origin + weaponRay.direction));


            ////Physics.IgnoreCollision(projectile.GetComponent<Collider>(), FPSWalkerComponent.capsule, true);

            if (projectile.transform.GetComponent<ArrowObject>())
            {
                ArrowRef = projectile.transform.GetComponent<ArrowObject>();
                ArrowRef.InitializeProjectile(true);
                if (isFire)
                    ArrowRef.arrowFireObj.gameObject.SetActive(true);
                //ArrowRef.velFactor = (projectileForce * fireHoldMult) / projectileForce;
                //ArrowRef.objectPoolIndex = (int)Define.PoolIdx.obj_Arrow;
            }

            projBody = projectile.GetComponent<Rigidbody>();
            projBody.velocity = Vector3.zero;
            projBody.angularVelocity = Vector3.zero;

            projBody.AddForce(force);

            if (projRotUp > 0.0f || projRotSide > 0.0f)
            {
                projBody.maxAngularVelocity = 10;//spin faster than default
                projBody.AddRelativeTorque(Vector3.up * Random.Range(projRotSide, projRotSide * 1.5f));
                projBody.AddRelativeTorque(Vector3.right * Random.Range(projRotUp, projRotUp * 1.5f));
            }

            fireHoldTimer = 0.0f;
            fireHoldMult = 0.0f;
        }
        //UnityEditor.EditorApplication.isPaused = true;
        yield return null;

    }

    private Vector3 SprayDirection()
    {
        return weaponRay.direction;
    }

    public void Pull_ByOther()
    {
        bowArrowPivot.gameObject.SetActive(true);
        //gamePlayer.arrowPivot.SetActive(false);
    }

    public void Pull(float value)
    {
        bowArrowPivot.gameObject.SetActive(true);
        Vector3 shotOrigin = bowPivot.transform.position;
        weaponRay.origin = shotOrigin;
        //weaponRay.direction = -bowPivot.transform.forward; // -bowPivot.transform.up;
        weaponRay.direction = -bowPivot.transform.up; // -bowPivot.transform.up;
        fireHoldMult = value;
        //Prediction
        if (renderLineVisible)
        {
            int predictionCount = 200;
            float predictionGap = 0.01f;
            Vector3 displacePos = Vector3.zero;
            float force = projectileForce * fireHoldMult + minimumProjForce;
            Vector3 forceVector = weaponRay.direction * force * 70f;

            if (debugLinePrediction == null)
            {
                debugLinePrediction = new GameObject("LinePrediction ").AddComponent<LineRenderer>();
                debugLinePrediction.transform.parent = transform;
                debugLinePrediction.SetWidth(0.03f, 0.03f);
                debugLinePrediction.SetVertexCount(predictionCount);
            }
            debugLinePrediction.gameObject.SetActive(true);

            int pointCount = 0;
            for (float i = predictionGap; i < predictionGap * predictionCount; i += predictionGap)
            {
                BallUtil.TrajectoryCalculate.GetDisplacement(ref displacePos.x, ref displacePos.y, ref displacePos.z,
                    shotOrigin, forceVector.x, forceVector.y, forceVector.z, 0, 0, 0, 0.25f, i);
                debugLinePrediction.SetPosition(pointCount, displacePos);
                pointCount++;
            }
        }
        else
        {
            //if (debugLinePrediction)
            //    debugLinePrediction.gameObject.SetActive(false);
        }
        //~Prediction
        //gamePlayer.CmdPull();
    }
}
