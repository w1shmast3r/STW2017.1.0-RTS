using UnityEngine;
using UnityEngine.Networking;

public class EnvironmentSpawner : NetworkBehaviour
{
    public GameObject TeamStart;

    public override void OnStartServer()
    {
        CmdSapwnStartBuiling();
    }

    [Command]
    private void CmdSapwnStartBuiling()
    {
        var start = Instantiate(TeamStart);
        NetworkServer.Spawn(start);
    }
}
