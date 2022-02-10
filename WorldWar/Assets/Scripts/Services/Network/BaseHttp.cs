using BestHTTP;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseHttp
{
    HTTPRequest request;

    public void Get(Uri url, OnRequestFinishedDelegate callback)
    {
        request = new HTTPRequest(url, HTTPMethods.Get, (req, resp) =>
        {
            callback(req, resp);
        });

        request.Send();
    }

    public void PostField(Uri url, object field, OnRequestFinishedDelegate callback)
    {
        request = new HTTPRequest(url, HTTPMethods.Post, (req, resp) =>
        {
            HTTPManager.Logger.Information("Post", string.Format("State : {0}", req.State));

            switch (req.State)
            {
                case HTTPRequestStates.Finished: callback(req, resp); break;
                case HTTPRequestStates.Error: callback(req, resp); break;
                case HTTPRequestStates.TimedOut: callback(req, resp); break;
            }
        });

        request.AddField("args", Newtonsoft.Json.JsonConvert.SerializeObject(field));
        request.Send();
    }

    public void PostField(Uri url, Dictionary<string, string> fields, OnRequestFinishedDelegate callback)
    {
        request = new HTTPRequest(url, HTTPMethods.Post, (req, resp) =>
        {
            HTTPManager.Logger.Information("Post", string.Format("State : {0}", req.State));

            switch (req.State)
            {
                case HTTPRequestStates.Finished: callback(req, resp); break;
                case HTTPRequestStates.Error: callback(req, resp); break;
                case HTTPRequestStates.TimedOut: callback(req, resp); break;
            }
        });

        foreach (KeyValuePair<string, string> field in fields)
            request.AddField(field.Key, field.Value);

        request.Send();
    }

    public void Post(Uri url, object field, OnRequestFinishedDelegate callback)
    {
        request = new HTTPRequest(url, HTTPMethods.Post, (req, resp) =>
        {
            callback(req, resp);
        });

        request.AddHeader("Content-Type", "application/json");
        request.RawData = new System.Text.UTF8Encoding().GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(field));
        request.Send();
    }

    public void Put(Uri url, object field, OnRequestFinishedDelegate callback)
    {
        request = new HTTPRequest(url, HTTPMethods.Put, (req, resp) =>
        {
            callback(req, resp);
        });

        request.AddHeader("Content-Type", "application/json");
        request.RawData = new System.Text.UTF8Encoding().GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(field));
        request.Send();
    }

    public void Delete(Uri url, OnRequestFinishedDelegate callback)
    {
        request = new HTTPRequest(url, HTTPMethods.Delete, (req, resp) =>
        {
            callback(req, resp);
        });

        request.Send();
    }
}


//using BestHTTP;
//using System;

//public class BaseHttp
//{
//    HTTPRequest request;

//    public void Get(Uri url, OnRequestFinishedDelegate callback)
//    {
//        request = new HTTPRequest(url, HTTPMethods.Get, (req, resp) =>
//        {
//            HTTPManager.Logger.Information("Get", string.Format("State : {0}", req.State));

//            switch (req.State)
//            {
//                case HTTPRequestStates.Finished: callback(req, resp); break;
//                case HTTPRequestStates.Error: callback(req, resp); break;
//                case HTTPRequestStates.TimedOut: callback(req, resp); break;
//            }
//        });

//        request.Send();
//    }

//    public void Post(Uri url, object field, OnRequestFinishedDelegate callback)
//    {
//        request = new HTTPRequest(url, HTTPMethods.Post, (req, resp) =>
//        {
//            HTTPManager.Logger.Information("Post", string.Format("State : {0}", req.State));

//            switch (req.State)
//            {
//                case HTTPRequestStates.Finished: callback(req, resp); break;
//                case HTTPRequestStates.Error: callback(req, resp); break;
//                case HTTPRequestStates.TimedOut: callback(req, resp); break;
//            }
//        });

//        request.AddField("args", Newtonsoft.Json.JsonConvert.SerializeObject(field));
//        request.Send();
//    }
//}
