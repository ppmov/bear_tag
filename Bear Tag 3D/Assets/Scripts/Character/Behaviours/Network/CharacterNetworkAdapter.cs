using Mirror;
using UnityEngine;

public class CharacterNetworkAdapter : NetworkBehaviour
{
    [SerializeField]
    private GameObject cameraPrefab;
    [SerializeField]
    private Hitbox hitbox;

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
    private void CmdJoin(string name, uint id) => Game.Party.Join(name, id);

    public void ReportTag(Transform tagger)
    {
        if (!hasAuthority)
            return;

        CmdTag(tagger.GetComponent<NetworkIdentity>().netId);
    }

    [Command]
    private void CmdTag(uint tagger)
    {
        Game.Party.Score(tagger);
        RpcTag();
    }

    [ClientRpc(includeOwner = true)]
    private void RpcTag() => hitbox.Tag();

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
