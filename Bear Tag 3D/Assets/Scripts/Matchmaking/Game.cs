using UnityEngine;
using Mirror;

public class Game
{
    public static Party Party => netManager.Party;
    public static bool IsStarted => NetworkClient.isConnected;
    public static bool IsConnecting => NetworkClient.isConnecting || netManager.IsReconnecting;
    public static int MaxPlayers => netManager.maxConnections;
    public static int WinCondition => 3;
    public static string Nickname { get; set; } = string.Empty;

    private static readonly CustomNetworkManager netManager;

    static Game()
    {
        netManager = (CustomNetworkManager)NetworkManager.singleton;

        if (netManager == null)
            throw new UnityException("Network Manager should be Custom");
    }

    public static Player GetWinner()
    {
        for (int i = 0; i < Party.Count; i++)
            if (Party[i].score >= WinCondition)
                return Party[i];

        return null;
    }

    public static void Restart() => netManager.Restart();
}
