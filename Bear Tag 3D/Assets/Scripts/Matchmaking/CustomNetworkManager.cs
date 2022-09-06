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

    public void Restart()
    {
        // restart host if host
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            StopHost();
            Discovery.StopDiscovery();
            StartHost();
            Discovery.AdvertiseServer();
        }
        // reconnect to server if client
        else if (NetworkClient.isConnected)
        {
            Uri uri = Transport.activeTransport.ServerUri();
            StopClient();
            StartCoroutine(WaitAndReconnect(uri));
        }
    }

    private IEnumerator WaitAndReconnect(Uri uri)
    {
        yield return new WaitForSeconds(1f);
        Discovery.StopDiscovery();
        StartClient(uri);
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
