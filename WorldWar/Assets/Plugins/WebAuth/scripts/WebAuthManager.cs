using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using HttpAuth;
using SimpleJSON;

[System.Serializable]
public class AuthResultEvent : UnityEvent<bool, int, string> { }

public class WebAuthManager : MonoBehaviour
{
    public GameAuthInfo AuthInfo;

    public AuthResultEvent OnSuccessEvent;

    public delegate void OnResult();

    void Awake()
    {
        Check_Auth();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    bool Check_Auth()
    {
        GameAuth testAuth = new GameAuth(AuthInfo);
        string strEncrypt = testAuth.GetURLParamString();

        //Debug.Log(strEncrypt);

        Dictionary<string, string> postString = new Dictionary<string, string>();
        postString.Add("GameCode", strEncrypt);
        
        
        if (testAuth.GetConnectURL().Length == 0)
        {
            Debug.LogError("WebAuth Connect Error!!");

            return false;
        }
        else
        {
            Send_PostRequest(testAuth.GetConnectURL(), postString);
        }

        return true;
    }

    public WWW Send_PostRequest(string addrURL, Dictionary<string, string> post)
    {
        Debug.Log(addrURL);

        WWWForm form = new WWWForm();

        foreach(KeyValuePair<string, string> post_arg in post)
        {
            form.AddField(post_arg.Key, post_arg.Value);
        }

        WWW www = new WWW(addrURL, form);

        StartCoroutine(WaitforRequest(www));

        return www;
    }

    private IEnumerator WaitforRequest(WWW www)
    {
        yield return www;

        // check for errors
        if(www.error == null)
        {
            OnHttpRequest(true, www.text);
            //Debug.Log("WWW OK!: " + www.text);
        }
        else
        {
            OnHttpRequest(false, www.error);
            //Debug.Log("WWW Error: " + www.error);
        }
    }

    public void OnHttpRequest(bool bSuccess, string strMessage)
    {
        if (bSuccess)
        {
            SimpleJSON.JObject obj = JSONDecoder.Decode(strMessage);

            int iResult = obj["result"].IntValue;
            string strSuccess = obj["success_yn"].StringValue;
            string strResult = obj["result_msg"].StringValue;
            bool bResult = false;
            
            if (strSuccess == "N")
            {
                bResult = false;
                //Application.Quit();
            }
            else
            {
                bResult = true;
            }

            Debug.Log("result : " + obj["result"].IntValue);
            Debug.Log("result_msg : " + obj["result_msg"].StringValue);
            Debug.Log("success_yn : " + obj["success_yn"].StringValue);

            OnSuccessEvent.Invoke(bResult, iResult, strResult);
        }
    }
}
