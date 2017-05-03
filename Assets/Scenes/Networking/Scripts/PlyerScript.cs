using UnityEngine;
using UnityEngine.Networking;

public class PlyerScript : NetworkBehaviour
{
    public GameObject[] tanks;
    public Base_NW.Team myTeam;
    public TeamHandler_NW teamHandler;
    public GameObject myBase;
    private Vector3 spawnPosition;

    void Start()
    {
        teamHandler = GameObject.Find("ObjectPooler").GetComponent<TeamHandler_NW>();

        if (isServer)
            if(isLocalPlayer)
                myTeam = Base_NW.Team.Player1;
            else
                myTeam = Base_NW.Team.Player2;
        else
            if (!isLocalPlayer)
                myTeam = Base_NW.Team.Player1;
            else
                myTeam = Base_NW.Team.Player2;

        if (isLocalPlayer)
        {
            LevelController.myTeam = myTeam;
            LevelController.player = this;
            var strTeam = myTeam == Base_NW.Team.Player1 ? "Player1" : "Player2";

            var baseTransform = GameObject.FindGameObjectWithTag("SpawnPosition" + strTeam).transform;
            var basePosition = baseTransform.position;
            spawnPosition = basePosition + Vector3.forward*3;

            Camera.main.GetComponent<ISRTSCamera>().Follow(baseTransform);

            LevelController.buildingMenu = GameObject.Find("BaseOptions");
            LevelController.buildingMenu.SetActive(false);
            CmdSpawnBase(basePosition, myTeam);
        }
    }

    [Command]
    public void CmdSpawnBase(Vector3 position, Base_NW.Team playerTeam)
    {
        var go = (GameObject)Instantiate(myBase, position, Quaternion.identity);
        if (playerTeam == Base_NW.Team.Player2)
            go.transform.Rotate(Vector3.up, 180);
        if (playerTeam == Base_NW.Team.Player1)
            go.transform.Rotate(Vector3.up, -50);

        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        if (playerTeam == Base_NW.Team.Player1)
            teamHandler.structurePlayer1.Add(go.GetComponent<NetworkIdentity>().netId.Value);
        if (playerTeam == Base_NW.Team.Player2)
            teamHandler.structurePlayer2.Add(go.GetComponent<NetworkIdentity>().netId.Value);
    }


    [Command]
    void CmdSpawn(int i, Vector3 position, Base_NW.Team playerTeam)
    {
        var go = (GameObject)Instantiate(tanks[i], position, Quaternion.identity);
        var unit = go.GetComponent<Unit_NW>();
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        if (playerTeam == Base_NW.Team.Player1)
            teamHandler.unitPlayer1.Add(go.GetComponent<NetworkIdentity>().netId.Value);
        if (playerTeam == Base_NW.Team.Player2)
            teamHandler.unitPlayer2.Add(go.GetComponent<NetworkIdentity>().netId.Value);
    }

    public void SpownTank(int i)
    {
        CmdSpawn(i, spawnPosition, myTeam);
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;        
    }
}
