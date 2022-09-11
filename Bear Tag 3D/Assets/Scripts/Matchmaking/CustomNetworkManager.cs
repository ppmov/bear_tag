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
    [SerializeField]
    [Range(0, 60)]
    private int maxRestartWaitingTime;
    [SerializeField]
    [Range(1, 10)]
    private int maxReconnectAttemptsCount;

    private string lastNetworkAddress;

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

        // wait until everyone disconnects
        for (float time = 0f; time < maxRestartWaitingTime; time += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();

            if (NetworkServer.connections.Count == 1)
                break;
        }

        // shutdown
        StopHost();

        // start new server and connect
        StartHost();
        Discovery.AdvertiseServer();
    }

    [Client]
    private IEnumerator WaitAndReconnect()
    {
        IsReconnecting = true;

        // disconnect
        StopClient();

        // try reconnect to server
        for (int i = 0; i < maxReconnectAttemptsCount; i++)
        {
            networkAddress = lastNetworkAddress;
            StartClient();

            yield return new WaitWhile(() => NetworkClient.isConnecting);

            if (NetworkClient.isConnected)
                break;
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        lastNetworkAddress = networkAddress;
        IsReconnecting = false;
        OnConnect?.Invoke();
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        OnDisconnect?.Invoke();
    }

    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        if (Game.HasWinner)
            conn.Disconnect();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Party.RemoveClient(conn);
        base.OnServerDisconnect(conn);
    }
}
