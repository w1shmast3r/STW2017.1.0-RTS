using UnityEngine;
using UnityEngine.Networking;

public class PlyerScript : NetworkBehaviour
{

    public GameObject[] tanks;
    public Base_NW.Team myTeam;
    public TeamHandler_NW teamHandler;
    public GameObject myBase;
    private Vector3 spawnPosition;

    private Camera m_camera;
    void Start()
    {
        teamHandler = GameObject.Find("ObjectPooler").GetComponent<TeamHandler_NW>();

        m_camera = GameObject.Find("Main Camera").GetComponent<Camera>();
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
            var BasePosition = GameObject.FindGameObjectWithTag("SpawnPosition" + strTeam).transform.position;
            spawnPosition = BasePosition + Vector3.forward*3;
            GameObject.Find("Main Camera").transform.position = BasePosition;
            LevelController.buildingMenu = GameObject.Find("BaseOptions");
            LevelController.buildingMenu.SetActive(false);
            CmdSpawnBase(BasePosition, myTeam);
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

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    RaycastHit hit;
        //    var raycast = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000);
        //    if (raycast)
        //        //var pos = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        //    CmdSpawn(2, hit.point, myTeam);
        //}
    }
}
