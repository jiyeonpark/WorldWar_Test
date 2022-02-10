using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class CamChoice : AutoSingleton<CamChoice> 
{
    public float eyeHeight = 1.4f;
    public GameObject steamVRCamera = null;
    public GameObject ovrVRCamera = null;
    [HideInInspector]
    public GameObject _mainCam;
    [HideInInspector]
    public GameObject _oculusParent;

    [Space]
    public bool effect = false;
    public Texture2D tex1 = null;
    public Texture2D tex2 = null;
    public Shader shader = null;
    public FrostEffect frostEffect = null;

    private float keyboardSpeed = 0.1f;
    private float mouseSpeed = 0.5f;
    private float mouseX = 0f;

    protected override void OnAwake()
    {
        DontDestroyOnLoad(gameObject);

        base.OnAwake();
    }

    protected override void OnStart()
    {
        if (OVRManager.isHmdPresent)
        {
            StoreManager.sInstance.device = DeviceType.Oculus;
        }
        else if (SteamVR.usingNativeSupport)
        {
            StoreManager.sInstance.device = DeviceType.Steam;
        }
        else
        {
            StoreManager.sInstance.device = DeviceType.Normal;
        }

        //CamChoice 생성
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            //스팀이라면
            Object obj = Instantiate(steamVRCamera, transform.position, transform.rotation);
            obj.name = "[CameraRig]";
            _mainCam = obj as GameObject;
            DontDestroyOnLoad(_mainCam);
        }
        else if (StoreManager.sInstance.device == DeviceType.Oculus)
        {
            //오큘이라면
            Object obj = Instantiate(ovrVRCamera, transform.position, transform.rotation);
            obj.name = "[CameraRig]";
            _mainCam = obj as GameObject;
            DontDestroyOnLoad(_mainCam);
        }
        else
        {
            _oculusParent = new GameObject("oculusParent");
            _oculusParent.transform.position = transform.position + new Vector3(0f, eyeHeight, 0f);
            _oculusParent.transform.rotation = transform.rotation;
            GameObject obj = new GameObject("vrcamera");
            obj.transform.parent = _oculusParent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            obj.AddComponent<Camera>();
            _oculusParent.AddComponent<AudioListener>();
            _mainCam = obj as GameObject;
            DontDestroyOnLoad(_oculusParent);
        }

        PlayerInput.Instance.Setup();
        // fog setting
        //GlobalFog_Custom fog = GetComponent<GlobalFog_Custom>();
        //if (fog)
        //{
        //    GlobalFog_Custom setfog = PlayerInput.Instance.CamHead.gameObject.AddComponent<GlobalFog_Custom>();
        //    setfog.distanceFog = fog.distanceFog;
        //    setfog.excludeFarPixels = fog.excludeFarPixels;
        //    setfog.useRadialDistance = fog.useRadialDistance;
        //    setfog.heightFog = fog.heightFog;
        //    setfog.height = fog.height;
        //    setfog.heightDensity = fog.heightDensity;
        //    setfog.startDistance = fog.startDistance;
        //    setfog.color = fog.color;
        //    fog.enabled = false;
        //    GetComponent<Camera>().enabled = false;
        //}

        if (effect == true && PlayerInput.Instance.CamHead)
        {
            frostEffect = PlayerInput.Instance.CamHead.gameObject.AddComponent<FrostEffect>();
            frostEffect.Frost = tex1;
            frostEffect.FrostNormals = tex2;
            frostEffect.Shader = shader;
            frostEffect.FrostAmount = 0f;
            frostEffect.EdgeSharpness = 5f;
            frostEffect.minFrost = 0f;
            frostEffect.maxFrost = 1f;
            frostEffect.seethroughness = 0.2f;
            frostEffect.distortion = 0.1f;
        }

        base.OnStart();
    }

    void Update()
    {
        if (GameManager.IsCreated() == false || GameManager.Instance.isInitialized == false)
            return;
        if (StoreManager.sInstance.device == DeviceType.Normal)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.localPosition += (_oculusParent.transform.forward * keyboardSpeed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.localPosition -= (_oculusParent.transform.forward * keyboardSpeed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.localPosition += (Define.WCVector3(_oculusParent.transform.right.x, 0f, _oculusParent.transform.right.z)
                    * keyboardSpeed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.localPosition -= (Define.WCVector3(_oculusParent.transform.right.x, 0f, _oculusParent.transform.right.z)
                    * keyboardSpeed);
            }

            if (Input.GetMouseButton(0))
            {
                if ((mouseX - Input.mousePosition.x) < 0)
                    transform.eulerAngles = Define.WCVector3(transform.eulerAngles.x, transform.eulerAngles.y
                        + mouseSpeed, transform.eulerAngles.z);
                else if ((mouseX - Input.mousePosition.x) > 0)
                    transform.eulerAngles = Define.WCVector3(transform.eulerAngles.x, transform.eulerAngles.y
                        - mouseSpeed, transform.eulerAngles.z);
                mouseX = Input.mousePosition.x;
            }
        }
    }
}
