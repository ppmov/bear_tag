using System.Collections.Generic;
using Mirror;
using System;

public class Party : NetworkBehaviour
{
    private readonly SyncList<Player> syncList = new SyncList<Player>();
    private readonly List<Player> list = new List<Player>();

    public event Action Callback;
    public event Action OnReset;

    public int Count => list.Count;
    public Player this[int index] => list[index];

    [Server]
    public void Score(uint tagger)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i].netId == tagger)
                syncList[i] = syncList[i].Score();
    }

    [Server]
    public void Join(string name, uint id)
    {
        syncList.Add(new Player(name, id));
    }

    [Server]
    public void RemoveClient(NetworkConnectionToClient client)
    {
        if (client == null || client.identity == null)
            return;

        Player leaver = null;

        foreach (Player player in syncList)
            if (player.netId == client.identity.netId)
                leaver = player;

        if (leaver != null)
            syncList.Remove(leaver);
    }

    public override void OnStartClient() => Prepare();

    public override void OnStopClient() => Reset();

    private void Prepare()
    {
        syncList.Callback += OnPlayerUpdates;

        foreach (Player player in syncList)
            list.Add(player);
    }

    private void Reset()
    {
        syncList.Callback -= OnPlayerUpdates;

        syncList.Reset();
        list.Clear();
        OnReset?.Invoke();
    }

    private void OnPlayerUpdates(SyncList<Player>.Operation operation, int itemIndex, Player oldItem, Player newItem)
    {
        switch (operation)
        {
            case SyncList<Player>.Operation.OP_ADD:
                list.Add(newItem);
                break;
            case SyncList<Player>.Operation.OP_CLEAR:
                break;
            case SyncList<Player>.Operation.OP_INSERT:
                break;
            case SyncList<Player>.Operation.OP_REMOVEAT:
                list.Remove(oldItem);
                break;
            case SyncList<Player>.Operation.OP_SET:
                list[itemIndex] = newItem;
                break;
        }

        Callback?.Invoke();
    }
}
