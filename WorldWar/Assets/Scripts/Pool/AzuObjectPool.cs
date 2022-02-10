//AzuObjectPool.cs by Azuline Studios© All Rights Reserved
//Creates object pools and contains functions for instantiating and recycling pooled objects
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MultiDimensionalPool{
	[Tooltip("The gameobject used to create an object pool.")]
	public GameObject prefab;
	[Tooltip("Number of this type of object to store in object pool.")] 
	public int poolSize;
    [Tooltip("스테이지별 개수제한을 각각세팅하기위함. (poolSize 값이 0일때 적용)")]
    public int[] stagePoolSize = new int[6];
	[HideInInspector]
	public int nextActive = 0;//index of next pooled object to spawn
	[Tooltip("True if spawned pooled object should ignore collision with player.")]
	public bool ignorePlayerCollision;
    [Tooltip("True일경우 Spawn 호출시점에 생성. False일경우 미리생성됨.")]
    public bool callCreate;
	[Tooltip("List of pooled game objects (should never have any missing list entries at runtime).")]
	public List<GameObject> pooledObjs = new List<GameObject>();
}

[System.Serializable]
public class AzuObjectPool : MonoBehaviour {

	//private Collider FPSWalkerCapsule;
	public static AzuObjectPool instance;
	private Transform myTransform;

	[Tooltip("List of object pools that are active in the scene. Index numbers of pools in this list are used by other scripts to identify which pooled objects to spawn.")]
	public List<MultiDimensionalPool> objRegistryModel = new List<MultiDimensionalPool>();
    public List<MultiDimensionalPool> objRegistryObject = new List<MultiDimensionalPool>();
    public List<MultiDimensionalPool> objRegistryEffect = new List<MultiDimensionalPool>();
    public List<MultiDimensionalPool> objRegistryEtc = new List<MultiDimensionalPool>();

	void Awake(){
		instance = this;

        //FPSWalkerCapsule = FPSPlayer.Instance.FPSWalkerComponent.GetComponent<Collider>();
        myTransform = transform;

        //create the object pools
        CreatePools(objRegistryModel);
        CreatePools(objRegistryObject);
        CreatePools(objRegistryEffect);
        CreatePools(objRegistryEtc);

        //GameEvents.PlayerDeadEvent += OnPlayerDead;
	}

    void OnDestroy()
    {
        //GameEvents.PlayerDeadEvent -= OnPlayerDead;
    }

    //void OnPlayerDead()
    //{
    //    if(objRegistryEffect != null)
    //    {
    //        for (int i = 0; i < objRegistryEffect.Count; i++)
    //        {
    //            if (objRegistryEffect[i].pooledObjs != null)
    //            {
    //                for (int j = 0; j < objRegistryEffect[i].pooledObjs.Count; j++)
    //                {
    //                    objRegistryEffect[i].pooledObjs[j].gameObject.SetActive(false);
    //                    if (objRegistryEffect[i].pooledObjs[j].transform.parent != transform)
    //                        objRegistryEffect[i].pooledObjs[j].transform.parent = transform;
    //                }
    //            }
    //        }
    //    }
    //}

	void CreatePools(List<MultiDimensionalPool> list)
    {
        int stageidx = 0;
        for (int i = 0; i < list.Count; i++)
        {
            int size = list[i].poolSize;
            if (size <= 0)
            {
                if (stageidx == -1)
                {// ArcadeMode 에서는 모두다 생성..
                    for (int n = 0; n < list[i].stagePoolSize.Length; n++)
                    {
                        if (list[i].stagePoolSize[n] > 0)
                        {
                            size = list[i].stagePoolSize[n];
                            break;
                        }
                    }
                }
                else
                    size = list[i].stagePoolSize[stageidx];
            }
            for (int n = 0; n < size; n++)
            {
                if (list[i].prefab && list[i].callCreate == false)
                {
                    GameObject obj;
                    obj = Instantiate(list[i].prefab, myTransform.position, myTransform.rotation) as GameObject;//instantiate a pooled object
                    if (list[i].ignorePlayerCollision)
                    {
                        //ignore collision with player if ignorePlayerCollision is true
                        ;//// Physics.IgnoreCollision(obj.GetComponent<Collider>(), FPSWalkerCapsule, true);
                    }
                    obj.SetActive(false);
                    list[i].pooledObjs.Add(obj);//add object to pool
                    obj.transform.parent = myTransform;
                    //obj.AddComponent<PoolObject>();
                }
            }
        }
    }

	//spawn an object from a pool
	public GameObject SpawnPooledObj(int objRegIndex, Vector3 spawnPosition, Quaternion spawnRotation )
    {
        if (objRegIndex < 100)
        {
            return SpawnPooledObj(objRegIndex, spawnPosition, spawnRotation, objRegistryModel);
        }
        else if (objRegIndex < 200)
        {
            objRegIndex -= 100;
            return SpawnPooledObj(objRegIndex, spawnPosition, spawnRotation, objRegistryObject);
        }
        else if (objRegIndex < 300)
        {
            objRegIndex -= 200;
            return SpawnPooledObj(objRegIndex, spawnPosition, spawnRotation, objRegistryEtc);
        }
        else
        {
            objRegIndex -= 300;
            return SpawnPooledObj(objRegIndex, spawnPosition, spawnRotation, objRegistryEffect);
        }
	}

    GameObject SpawnPooledObj(int objRegIndex, Vector3 spawnPosition, Quaternion spawnRotation, List<MultiDimensionalPool> list)
    {
        if(list.Count <= objRegIndex)
        {
            Debug.LogError("SpawnPooledObj objRegIndex : " + objRegIndex.ToString() + " / list.Count : " + list.Count.ToString());
        }
        int size = list[objRegIndex].poolSize;
        //if (size <= 0)
        //{
        //    if (GameManager.Instance._modeState == ModeState.StoryMode)
        //    {
        //        switch (GameManager.Instance.stageIdx)
        //        {
        //            case Define.StageIdx.stage_oh1: size = list[objRegIndex].stagePoolSize[0]; break;
        //            case Define.StageIdx.stage_oh2: size = list[objRegIndex].stagePoolSize[1]; break;
        //            case Define.StageIdx.stage_oh3: size = list[objRegIndex].stagePoolSize[2]; break;
        //            case Define.StageIdx.stage_we1: size = list[objRegIndex].stagePoolSize[3]; break;
        //            case Define.StageIdx.stage_we2: size = list[objRegIndex].stagePoolSize[4]; break;
        //            case Define.StageIdx.stage_we3: size = list[objRegIndex].stagePoolSize[5]; break;
        //        }
        //    }
        //}
        if (list[objRegIndex].callCreate == true && list[objRegIndex].pooledObjs.Count < size)
        {
            GameObject obj;
            obj = Instantiate(list[objRegIndex].prefab, myTransform.position, myTransform.rotation) as GameObject;//instantiate a pooled object
            obj.SetActive(false);
            list[objRegIndex].pooledObjs.Add(obj);//add object to pool
            obj.transform.parent = myTransform;
        }
        // 혹시모를 예외처리..
        if (list[objRegIndex].callCreate == true)
        {
            if (list[objRegIndex].nextActive >= size)
                list[objRegIndex].nextActive = 0;//start from beginning if this is the last object in the pool
        }
        else
        {
            if (list[objRegIndex].nextActive >= list[objRegIndex].pooledObjs.Count)
                list[objRegIndex].nextActive = 0;//start from beginning if this is the last object in the pool
        }
        GameObject spawnObj = list[objRegIndex].pooledObjs[list[objRegIndex].nextActive];//identify which pool and object to spawn from with objRegIndex
        if (spawnObj == null)
            Debug.LogError("SpawnPooledObj spawnObj null : " + objRegIndex.ToString() + " / pooledObjs.Count : " + list[objRegIndex].pooledObjs.Count.ToString() + " / nextActive : " + list[objRegIndex].nextActive.ToString());
        else
        {
            spawnObj.SetActive(true);
            //set spawned object's position and rotation
            spawnObj.transform.position = spawnPosition;
            spawnObj.transform.rotation = spawnRotation;
            //find index of object in pool to spawn after this one
            if (list[objRegIndex].callCreate == true)
            {
                if (list[objRegIndex].nextActive >= size - 1)
                {
                    list[objRegIndex].nextActive = 0;//start from beginning if this is the last object in the pool
                }
                else
                {
                    list[objRegIndex].nextActive++;
                }
            }
            else
            {
                if (list[objRegIndex].nextActive >= list[objRegIndex].pooledObjs.Count - 1)
                {
                    list[objRegIndex].nextActive = 0;//start from beginning if this is the last object in the pool
                }
                else
                {
                    list[objRegIndex].nextActive++;
                }
            }
            //if (spawnObj.GetComponent<Explosion>())
            //{
            //    spawnObj.GetComponent<Explosion>().ExplosionEffect(spawnPosition);
            //}
            return spawnObj;
        }
        return null;
    }

	//recycle spawned pool object (visibly remove from scene and return to its object pool)
	public GameObject RecyclePooledObj(int objRegIndex, GameObject obj)
    {
        if (obj == null)
            return null;
        if (objRegIndex < 100)
        {
            return RecyclePooledObj(objRegIndex, obj, objRegistryModel);
        }
        else if (objRegIndex < 200)
        {
            objRegIndex -= 100;
            return RecyclePooledObj(objRegIndex, obj, objRegistryObject);
        }
        else if (objRegIndex < 300)
        {
            objRegIndex -= 200;
            return RecyclePooledObj(objRegIndex, obj, objRegistryEtc);
        }
        else
        {
            objRegIndex -= 300;
            return RecyclePooledObj(objRegIndex, obj, objRegistryEffect);
        }
		
	}

    GameObject RecyclePooledObj(int objRegIndex, GameObject obj, List<MultiDimensionalPool> list)
    {
        List<ParticleSystem> particles = new List<ParticleSystem>();
        obj.GetComponentsInChildren<ParticleSystem>(particles);
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
        int idx = list[objRegIndex].pooledObjs.IndexOf(obj);
        if (idx == -1) return null;
        GameObject recycleObj = list[objRegIndex].pooledObjs[idx];//identify which pool and object to recycle with objRegIndex
        recycleObj.transform.parent = myTransform;
        recycleObj.SetActive(false);

        return recycleObj;
    }

}
