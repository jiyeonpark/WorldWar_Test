using UnityEngine;
using System.Collections;

public class PlayerInput : AutoSingleton<PlayerInput> 
{
    private Transform camHead = null;
    public Transform CamHead { get { return camHead; } }
    private Transform camEars = null;
    public Transform CamEars { get { return camEars; } }
    private SteamVR_TrackedObject trackedObjL_Vive;
    public Transform TrackedObjL {
        get
        {
            if (StoreManager.sInstance.device == DeviceType.Steam)
            {
                if (trackedObjL_Vive == null) return null;
                return trackedObjL_Vive.transform;
            }
            else
            {
                return trackedObjL_Ovr;
            }
        }
    }
    private SteamVR_TrackedObject trackedObjR_Vive;
    public Transform TrackedObjR {
        get
        {
            if (StoreManager.sInstance.device == DeviceType.Steam)
            {
                if (trackedObjR_Vive == null) return null;
                return trackedObjR_Vive.transform;
            }
            else
            {
                return trackedObjR_Ovr;
            }
        }
    }
    private SteamVR_Controller.Device deviceL_Vive = null;
    private SteamVR_Controller.Device deviceR_Vive = null;
    private GameObject camRig = null;
    public Transform CamRig {
        get
        {
            if (camRig == null) return null;
            return camRig.transform;
        }
    }
    private GameObject controllerModelL = null;
    private GameObject controllerModelR = null;

    private Transform trackedObjL_Ovr;
    private Transform trackedObjR_Ovr;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Setup()
    {
        if (StoreManager.sInstance.device == DeviceType.Normal)
        {
            camRig = GameObject.Find("oculusParent");
            camHead = CamChoice.Instance._mainCam.transform;
            //camHead = Camera.allCameras[0].transform;
            //return;
        }
        else
        {
            camRig = GameObject.Find("[CameraRig]");
            Setting();
        }
        if (camHead && camHead.GetComponentInChildren<AudioListener>())
            camEars = camHead.GetComponentInChildren<AudioListener>().transform;
        GameManager.Instance.CamClippingSetting();
    }

    void Setting()
    {
        if (camRig)
        {
            if (StoreManager.sInstance.device == DeviceType.Steam)
            {
                if (trackedObjL_Vive == null)
                {
                    trackedObjL_Vive = camRig.transform.Find("Controller (left)").GetComponent<SteamVR_TrackedObject>();
                    controllerModelL = trackedObjL_Vive.transform.GetChild(0).gameObject;
                }
                if (trackedObjR_Vive == null)
                {
                    trackedObjR_Vive = camRig.transform.Find("Controller (right)").GetComponent<SteamVR_TrackedObject>();
                    controllerModelR = trackedObjR_Vive.transform.GetChild(0).gameObject;
                }
                if (camHead == null)
                    camHead = camRig.transform.Find("Camera (eye)");
            }
            else
            {
                trackedObjL_Ovr = camRig.transform.GetChild(0).Find("LeftHandAnchor");
                controllerModelL = trackedObjL_Ovr.transform.GetChild(0).gameObject;
                trackedObjR_Ovr = camRig.transform.GetChild(0).Find("RightHandAnchor");
                controllerModelR = trackedObjR_Ovr.transform.GetChild(0).gameObject;
                if (camHead == null)
                    camHead = camRig.transform.GetChild(0).Find("CenterEyeAnchor");
            }
#if UNITY_EDITOR
            if (camHead == null)
                camHead = Camera.allCameras[0].transform;
#endif
        }
    }



    void Update()
    {
        if (GameManager.IsCreated() == false || StoreManager.sInstance.device == DeviceType.Normal)
            return;  

        Setting();
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (trackedObjL_Vive != null && trackedObjL_Vive.index != SteamVR_TrackedObject.EIndex.None)
                deviceL_Vive = SteamVR_Controller.Input((int)trackedObjL_Vive.index);
            if (trackedObjR_Vive != null && trackedObjR_Vive.index != SteamVR_TrackedObject.EIndex.None)
                deviceR_Vive = SteamVR_Controller.Input((int)trackedObjR_Vive.index);
        }
#if UNITY_EDITOR
        //else if (StoreManager.sInstance.device == DeviceType.Oculus)
        //{
        //    if (OVRManager.hasVrFocus)
        //    {
        //        if (UIManager.IsCreated() && !UIManager.Instance.isPauseNow)
        //        {
        //            if (Time.timeScale != 1f)
        //            {
        //                Debug.Log("Time scale = 1");
        //                Time.timeScale = 1f;
        //                AudioListener.volume = 1f;
        //            }
        //        }
        //        else if (!UIManager.IsCreated())
        //        {
        //            if (Time.timeScale != 1f)
        //            {
        //                Debug.Log("Time scale = 1 (2)");
        //                Time.timeScale = 1f;
        //                AudioListener.volume = 1f;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Time.timeScale != 0f)
        //        {
        //            AudioListener.volume = 0f;
        //            Debug.Log("Time scale = 0");
        //            Time.timeScale = 0f;
        //        }
        //    }
        //}
#else
        //else if (StoreManager.sInstance.device == DeviceType.Oculus)
        //{
        //    if (OVRManager.hasVrFocus)
        //    {
        //        if (UIManager.IsCreated() && !UIManager.Instance.isPauseNow)
        //        {
        //            if (Time.timeScale != 1f)
        //            {
        //                Debug.Log("Time scale = 1");
        //                Time.timeScale = 1f;
        //                AudioListener.volume = 1f;
        //            }
        //        }
        //        else if (!UIManager.IsCreated())
        //        {
        //            if (Time.timeScale != 1f)
        //            {
        //                Debug.Log("Time scale = 1 (2)");
        //                Time.timeScale = 1f;
        //                AudioListener.volume = 1f;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Time.timeScale != 0f)
        //        {
        //            AudioListener.volume = 0f;
        //            Debug.Log("Time scale = 0");
        //            Time.timeScale = 0f;
        //        }
        //    }
        //}
#endif
    }

    // Model
    public void SetControllerModelActive(bool active)
    {
        if (controllerModelL)
            controllerModelL.SetActive(active);
        if (controllerModelR)
            controllerModelR.SetActive(active);
    }

    // Trigger
    public void TriggerHapticPulseR(ushort durationMicroSec = 500, bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive != null)
                    deviceR_Vive.TriggerHapticPulse(durationMicroSec);
            }
            else
                TriggerHapticPulseL(durationMicroSec, true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                float ff = ((durationMicroSec / 2f) - 550) / 1000f;
                //OVRInput.SetControllerVibration(0.1f, ff, OVRInput.Controller.RTouch);
                StartCoroutine(OvrVibrationR(0.1f, 0.35f));
            }
            else
                TriggerHapticPulseL(durationMicroSec, true);
        }
    }
    public void TriggerHapticPulseL(ushort durationMicroSec = 500, bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive != null)
                    deviceL_Vive.TriggerHapticPulse(durationMicroSec);
            }
            else
                TriggerHapticPulseR(durationMicroSec, true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                float ff = ((durationMicroSec / 2f) - 550) / 1000f;
                StartCoroutine(OvrVibrationL(0.1f, 0.35f));
            }
            else
                TriggerHapticPulseR(durationMicroSec, true);
        }
    }

    IEnumerator OvrVibrationR(float time, float power)
    {
        OVRInput.SetControllerVibration(0.1f, power, OVRInput.Controller.RTouch);
        yield return new WaitForSeconds(0.1f);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        yield break;
    }

    IEnumerator OvrVibrationL(float time, float power)
    {
        OVRInput.SetControllerVibration(0.1f, power, OVRInput.Controller.LTouch);
        yield return new WaitForSeconds(0.1f);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
        yield break;
    }

    public bool GetTouchDownTriggerR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger);
            }
            else
                return GetTouchDownTriggerL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            }
            else
                return GetTouchDownTriggerL(true);
        }
    }
    public bool GetTouchDownTriggerL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger);
            }
            else
                return GetTouchDownTriggerR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            }
            else
                return GetTouchDownTriggerR(true);
        }
    }
    public bool GetTouchTriggerR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetTouch(SteamVR_Controller.ButtonMask.Trigger);
            }
            else
                return GetTouchTriggerL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            }
            else
                return GetTouchTriggerL(true);
        }
    }
    public bool GetTouchTriggerL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetTouch(SteamVR_Controller.ButtonMask.Trigger);
            }
            else
                return GetTouchTriggerR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            }
            else
                return GetTouchTriggerR(true);
        }
    }
    public bool GetTouchUpTriggerR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger);
            }
            else
                return GetTouchUpTriggerL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
            }
            else
                return GetTouchUpTriggerL(true);
        }
    }
    public bool GetTouchUpTriggerL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger);
            }
            else
                return GetTouchUpTriggerR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
            }
            else
                return GetTouchUpTriggerR(true);
        }
    }

    // Touchpad
    public bool GetPressDownTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetPressDownTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                if (ovrAxis.magnitude > 0.2f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return GetPressDownTouchpadL(true);
        }
    }
    public bool GetPressDownTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetPressDownTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
                if (ovrAxis.magnitude > 0.2f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return GetPressDownTouchpadR(true);
        }
    }
    public bool GetPressTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetPressTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
            }
            else
                return GetPressTouchpadL(true);
        }
    }
    public bool GetPressTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetPressTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);
            }
            else
                return GetPressTouchpadR(true);
        }
    }
    public bool GetPressUpTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetPressUpTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
            }
            else
                return GetPressTouchpadL(true);
        }
    }
    public bool GetPressUpTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetPressUpTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);
            }
            else
                return GetPressUpTouchpadR(true);
        }
    }
    public bool GetTouchDownTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetTouchDownTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                if (ovrAxis.magnitude > 0.2f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return GetPressUpTouchpadL(true);
        }
    }
    public bool GetTouchDownTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetTouchDownTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
                if (ovrAxis.magnitude > 0.2f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return GetPressUpTouchpadR(true);
        }
    }
    public bool GetTouchTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetTouch(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetTouchTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                if (ovrAxis.magnitude > 0.2f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return GetTouchTouchpadL(true);
        }
    }
    public bool GetTouchTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetTouch(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetTouchTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
                if (ovrAxis.magnitude > 0.2f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return GetTouchTouchpadR(true);
        }
    }
    public bool GetTouchUpTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetTouchUpTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
            }
            else
                return GetTouchUpTouchpadL(true);
        }
    }
    public bool GetTouchUpTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad);
            }
            else
                return GetTouchUpTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);
            }
            else
                return GetTouchUpTouchpadR(true);
        }
    }
    public Vector2 GetAxisTouchpadR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return Vector2.one;
                return deviceR_Vive.GetAxis();
            }
            else
                return GetAxisTouchpadL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.RTouch))
                //    ovrAxis = new Vector2(0f, 1f);
                //else if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.RTouch))
                //    ovrAxis = new Vector2(0f, -1f);
                //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch))
                //    ovrAxis = new Vector2(-1f, 0f);
                //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch))
                //    ovrAxis = new Vector2(1f, 0f);
                return ovrAxis;
            }
            else
                return GetAxisTouchpadL(true);
        }
    }
    public Vector2 GetAxisTouchpadL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return Vector2.one;
                return deviceL_Vive.GetAxis();
            }
            else
                return GetAxisTouchpadR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                Vector2 ovrAxis = Vector2.zero;
                ovrAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
                //if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp, OVRInput.Controller.LTouch))
                //    ovrAxis = new Vector2(0f, 1f);
                //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown, OVRInput.Controller.LTouch))
                //    ovrAxis = new Vector2(0f, -1f);
                //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.LTouch))
                //    ovrAxis = new Vector2(-1f, 0f);
                //else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.LTouch))
                //    ovrAxis = new Vector2(1f, 0f);
                return ovrAxis;
            }
            else
                return GetAxisTouchpadR(true);
        }
    }

    // Grip
    public bool GetPressDownGripR(bool direct = false)
    {
        if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
        {
            if (deviceR_Vive == null) return false;
            return deviceR_Vive.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
        }
        else
            return GetPressDownGripL(true);
    }
    public bool GetPressDownGripL(bool direct = false)
    {
        if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
        {
            if (deviceL_Vive == null) return false;
            return deviceL_Vive.GetPressDown(SteamVR_Controller.ButtonMask.Grip);
        }
        else
            return GetPressDownGripR(true);
    }
    public bool GetPressGripR(bool direct = false)
    {
        if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
        {
            if (deviceR_Vive == null) return false;
            return deviceR_Vive.GetPress(SteamVR_Controller.ButtonMask.Grip);
        }
        else
            return GetPressGripL(true);
    }
    public bool GetPressGripL(bool direct = false)
    {
        if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
        {
            if (deviceL_Vive == null) return false;
            return deviceL_Vive.GetPress(SteamVR_Controller.ButtonMask.Grip);
        }
        else
            return GetPressGripR(true);
    }
    public bool GetPressUpGripR(bool direct = false)
    {
        if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
        {
            if (deviceR_Vive == null) return false;
            return deviceR_Vive.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
        }
        else
            return GetPressUpGripL(true);
    }
    public bool GetPressUpGripL(bool direct = false)
    {
        if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
        {
            if (deviceL_Vive == null) return false;
            return deviceL_Vive.GetPressUp(SteamVR_Controller.ButtonMask.Grip);
        }
        else
            return GetPressUpGripR(true);
    }

    // ApplicationMenu
    public bool GetPressDownMenuR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
            }
            else
                return GetPressDownMenuL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch);
            }
            else
                return GetPressDownMenuL(true);
        }
    }
    public bool GetPressDownMenuL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu);
            }
            else
                return GetPressDownMenuR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.LTouch);
            }
            else
                return GetPressDownMenuR(true);
        }
    }
    public bool GetPressMenuR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
            }
            else
                return GetPressMenuL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch);
            }
            else
                return GetPressMenuL(true);
        }
    }
    public bool GetPressMenuL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
            }
            else
                return GetPressMenuR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch);
            }
            else
                return GetPressMenuR(true);
        }
    }
    public bool GetPressUpMenuR(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceR_Vive == null) return false;
                return deviceR_Vive.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
            }
            else
                return GetPressUpMenuL(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch);
            }
            else
                return GetPressMenuL(true);
        }
    }
    public bool GetPressUpMenuL(bool direct = false)
    {
        if (StoreManager.sInstance.device == DeviceType.Steam)
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                if (deviceL_Vive == null) return false;
                return deviceL_Vive.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu);
            }
            else
                return GetPressUpMenuR(true);
        }
        else
        {
            if (GameManager.Instance.controllerIdx == Define.ControllerIdx.Right || direct == true)
            {
                return OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch);
            }
            else
                return GetPressMenuR(true);
        }
    }
}
