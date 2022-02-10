using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISoldier : MonoBehaviour 
{
    public Transform rootbone = null;
    public Transform gunbone = null;
    public Transform[] path = null;
    public float resetTime = 3f;
    public float lookDis = 10f;
    public float lookAngle = 50f;
    public float life = 30f;

    private NavMeshAgent agent = null;
    private Animator ani = null;
    private Collider[] collist = null;
    private Rigidbody[] rigidlist = null;
    private CapsuleCollider col = null;
    private bool die = false;
    private bool look = false;
    private float resetTimeTemp = 0f;
    private Vector3 orgPosition = Vector3.zero;
    private Vector3 targetpoint = Vector3.zero;
    private int pathcount = 0;
    private float lifeTemp = 0f;

	void Start () 
    {
        collist = rootbone.GetComponentsInChildren<Collider>();
        rigidlist = rootbone.GetComponentsInChildren<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        orgPosition = transform.position;
        lifeTemp = life;
        SetRagdoll(false);
        SetBone();
        MoveStart();
	}
	
	void Update () 
    {
        if (die)
        {
            resetTimeTemp += Time.deltaTime;
            if (resetTimeTemp > resetTime)
            {
                life = lifeTemp;
                resetTimeTemp = 0f;
                MoveStart();
            }
        }
        else if (look == false)
        {
            float dis = Vector3.Distance(transform.position, PlayerInput.Instance.CamHead.position);
            float angle = Vector3.Angle(transform.forward, (PlayerInput.Instance.CamRig.position - transform.position).normalized);
            if(dis < lookDis && angle < lookAngle)
            {
                // 시야안에 포착!
                ShowTarget();
            }
            else
                MoveNext();
        }
        else if (look == true)
        {
            transform.LookAt(Define.WCVector3(PlayerInput.Instance.CamHead.position.x, transform.position.y, PlayerInput.Instance.CamHead.position.z) + (transform.right * 1f));
            float dis = Vector3.Distance(transform.position, PlayerInput.Instance.CamHead.position);
            if (dis > lookDis + 20f)
                look = false;
        }
	}

    void SetRagdoll(bool value)
    {
        if (collist == null || rigidlist == null) return;
        if (value)
        {
            for (int i = 0; i < collist.Length; i++)
                collist[i].enabled = true;
            for (int i = 0; i < rigidlist.Length; i++)
                rigidlist[i].isKinematic = false;
            if (ani) ani.enabled = false;
            if (col) col.enabled = false;
        }
        else
        {
            for (int i = 0; i < collist.Length; i++)
                collist[i].enabled = false;
            for (int i = 0; i < rigidlist.Length; i++)
                rigidlist[i].isKinematic = true;
            if (ani) ani.enabled = true;
            if (col) col.enabled = true;
        }
    }

    void SetBone()
    {
        Transform[] list = GetComponentsInChildren<Transform>();
        for (int i = 0; i < list.Length; i++)
        {
            bool add = false;
            switch (list[i].name)
            {
                case "Bip001 Spine":
                    {
                        CapsuleCollider col = list[i].gameObject.AddComponent<CapsuleCollider>();
                        col.center = Vector3.zero;
                        //col.radius = 0.26f;
                        col.radius = 0.3f;
                        col.height = 1.04f;
                        col.direction = 0;
                        list[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
                        add = true;

                        // CheckPoint랑 충돌처리하기위해 하나 더 붙여줌..
                        BoxCollider col2 = list[i].gameObject.AddComponent<BoxCollider>();
                        col2.size = Define.WCVector3(1f, 0.5f, 0.5f);
                        col2.isTrigger = true;
                    }
                    break;
                case "Bip001 Neck":
                    {
                        CapsuleCollider col = list[i].gameObject.AddComponent<CapsuleCollider>();
                        col.center = Define.WCVector3(-0.19f, 0f, 0f);
                        //col.radius = 0.16f;
                        col.radius = 0.2f;
                        col.height = 0.3f;
                        col.direction = 0;
                        list[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
                        add = true;
                    }
                    break;
            }
            if (add)
            {
                list[i].tag = "Player";
                DamageCheck damagecheck = list[i].gameObject.AddComponent<DamageCheck>();
                damagecheck.ai = this;
            }
        }
    }

    void MoveStart()
    {
        if (agent == null || path == null) return;
        pathcount = 0;
        if (path.Length > pathcount)
        {
            agent.SetDestination(path[pathcount].position);
            if (ani) ani.SetTrigger("run");
        }
        SetRagdoll(false);
        transform.position = orgPosition;
        die = false;
        look = false;
    }

    void MoveNext()
    {
        if (agent == null || path == null || path.Length <= 0) return;
        //Debug.Log("remainingDistance = " + agent.remainingDistance);
        if (agent.pathPending == false && agent.hasPath == false && agent.remainingDistance <= agent.stoppingDistance)
        {
            pathcount++;
            if (path.Length <= pathcount) pathcount = 0;
            if (path.Length > pathcount && path.Length > 1)
            {
                agent.SetDestination(path[pathcount].position);
                //Debug.Log(pathcount.ToString() + " pathPending : " + agent.pathPending.ToString() + " / hasPath : " + agent.hasPath.ToString() + " / status : " + agent.path.status.ToString());
            }
            if (ani) ani.SetTrigger("run");
        }
    }

    void ShowTarget()
    {
        look = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        transform.LookAt(Define.WCVector3(PlayerInput.Instance.CamHead.position.x, transform.position.y, PlayerInput.Instance.CamHead.position.z));
        if (ani)
        {
            ani.ResetTrigger("run");
            ani.SetTrigger("shot");
        }
    }

    public void Damage(float damage)
    {
        life -= damage;
        if (life <= 0f)
        {
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            SetRagdoll(true);
            die = true;
            look = false;
        }
    }

    public void Shot()
    {
        //SoundManager.Instance.PlayAKMShotSound(transform.position, 1f);
        for (int i = 0; i < Random.Range(1, 6); i++)
        {// 사운드연출을 위해서 여러번 사운드 플레이..
            this.After(Random.Range(0.1f, 0.3f) * i,
                () =>
                {
                    SoundManager.Instance.PlayAKMShotSound(transform.position, 1f);
                });
        }
        GameObject bullet = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.obj_Bullet, gunbone.position, gunbone.rotation);
        GameObject gunfireeffect = AzuObjectPool.instance.SpawnPooledObj((int)Define.PoolIdx.effect_GunFire, gunbone.position + gunbone.forward * 0.1f, gunbone.rotation);
        gunfireeffect.transform.parent = gunbone;
        this.After(0.3f,
            () =>
            {
                gunfireeffect.SetActive(false);
                gunfireeffect.transform.parent = AzuObjectPool.instance.transform;
            });
        RaycastHit hit;
        if (Physics.Raycast(gunbone.position, gunbone.forward, out hit))
        {
            targetpoint = hit.point;
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                GameEvents.EffectDamage();
        }
        StartCoroutine("ShotMove", bullet.transform);
    }

    IEnumerator ShotMove(Transform obj)
    {
        yield return null;

        //float pulsetime = 0f;
        Vector3 point = targetpoint;
        float dis = Vector3.Distance(point, obj.position);
        if (point == Vector3.zero) dis = 10f;
        float time = dis / 600f;            // AKM speed = 600~715m / 1sec
        float alldis = 0f;
        while (alldis < dis)
        {
            float value = dis * Time.deltaTime / time;
            alldis += value;
            if (value > dis)
                value = dis;
            obj.position += obj.forward * value;
            yield return null;
        }
        obj.gameObject.SetActive(false);

        //Debug.Log("shot gun ! = dis : " + dis.ToString() + " , time : " + time.ToString());
    }
}
