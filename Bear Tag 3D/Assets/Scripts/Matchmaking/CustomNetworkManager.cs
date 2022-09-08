using Mirror;
using Mirror.Discovery;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CustomNetworkManager : NetworkManager
{
    [Header("Extended")]
    [SerializeField]
    private Party party;
    [SerializeField]
    private NetworkDiscovery discovery;

    public UnityEvent OnConnect;
    public UnityEvent OnDisconnect;

    public Party Party => party;
    public NetworkDiscovery Discovery => discovery;

    public bool IsReconnecting { get; private set; } = false;

    public void Restart()
    {
        // restart host if host
        if (NetworkServer.active && NetworkClient.isConnected)
            StartCoroutine(WaitAndRestartServer());
        // reconnect to server if client
        else if (NetworkClient.isConnected)
            StartCoroutine(WaitAndReconnect());
    }

    [Server]
    private IEnumerator WaitAndRestartServer()
    {
        IsReconnecting = true;

        // wait until all players leave
        while (NetworkServer.connections.Count > 1)
            yield return new WaitForFixedUpdate();

        StopHost();
        Discovery.StopDiscovery();
        StartHost();
        Discovery.AdvertiseServer();
        IsReconnecting = false;
    }

    [Client]
    private IEnumerator WaitAndReconnect()
    {
        IsReconnecting = true;
        string lastAddress = networkAddress;
        StopClient();

        yield return new WaitForSecondsRealtime(1f);
        
        Discovery.StopDiscovery();
        networkAddress = lastAddress;
        StartClient();
        IsReconnecting = false;
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        OnConnect?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnDisconnect?.Invoke();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Party.RemoveClient(conn);
        base.OnServerDisconnect(conn);
    }
}
