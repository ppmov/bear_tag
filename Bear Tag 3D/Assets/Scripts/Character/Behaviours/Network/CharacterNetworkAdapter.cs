using Mirror;
using UnityEngine;

public class CharacterNetworkAdapter : NetworkBehaviour
{
    [SerializeField]
    private GameObject cameraPrefab;

    public override void OnStartLocalPlayer()
    {
        if (!hasAuthority)
            return;

        if (isServer)
            Game.Party.Join(Game.Nickname, netId);
        else
            CmdJoin(Game.Nickname, netId);
    }

    [Command]
    public void CmdJoin(string name, uint id) => Game.Party.Join(name, id);

    public void ScoreMyself()
    {
        if (isServer)
            Game.Party.Score(netId);
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            // only local game object has camera and character script
            Instantiate(cameraPrefab, transform);
            gameObject.AddComponent<Character>();
        }
    }
}
