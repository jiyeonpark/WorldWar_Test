using BestHTTP.SignalR;
using BestHTTP.SignalR.Hubs;
using System;

public class BaseSignalr
{
    private Connection connect;
    private Hub hub;

    public event OnConnectedDelegate OnConnected;
    public event OnClosedDelegate OnClosed;
    public event OnErrorDelegate OnError;
    public event OnConnectedDelegate OnReconnecting;
    public event OnConnectedDelegate OnReconnected;
    public event OnMethodCallDelegate OnMethodCall;

    public void Close()
    {
        if (connect != null)
            connect.Close();
    }

    public void Connect(Uri uri, string hubName)
    {
        hub = new Hub(hubName);
        connect = new Connection(uri, hub);

        connect.OnConnected += OnConnected;
        connect.OnClosed += OnClosed;
        connect.OnError += OnError;
        connect.OnReconnecting += OnReconnecting;
        connect.OnReconnected += OnReconnected;

        hub.OnMethodCall += OnMethodCall;

        connect.Open();
    }

    public void Reconnect()
    {
        if (connect != null)
            connect.Reconnect();
    }

    public void Call(string method)
    {
        if(hub != null)
            hub.Call(method);
    }

    public void Call(string method, object args)
    {
        if (hub != null)
            hub.Call(method, Newtonsoft.Json.JsonConvert.SerializeObject(args));
    }
}
