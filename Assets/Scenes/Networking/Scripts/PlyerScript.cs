using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class PlyerScript : NetworkBehaviour
{
    public GameObject[] tanks;
    public Base_NW.Team myTeam;
    public TeamHandler_NW teamHandler;
    public GameObject myBase;
    private Vector3 spawnPosition;
	
	[SyncVar]
	public Color myColor = Color.black;
	
    void Start()
    {

		if (myColor != Color.black)
		{
			if(myColor == Color.blue)
				myTeam = Base_NW.Team.Player1;
			else
				myTeam = Base_NW.Team.Player2;
			Debug.Log("serverRaceColor = " + myColor);


			//ChangeTankColor(serverRaceColor); - here we can implement change of the tank color
		}

        if (isLocalPlayer)
        {
            StartCoroutine(InitializeBase());
        }
    }

    IEnumerator InitializeBase()
    {
        yield return new WaitForSeconds(1);

        LevelController.myTeam = myTeam;
        LevelController.player = this;
        var strTeam = myTeam == Base_NW.Team.Player1 ? "Player1" : "Player2";

        var baseTransform = GameObject.FindGameObjectWithTag("SpawnPosition" + strTeam).transform;
        var basePosition = baseTransform.position;
        spawnPosition = basePosition + Vector3.forward * 3;

        Camera.main.GetComponent<ISRTSCamera>().Follow(baseTransform);

        LevelController.buildingMenu = GameObject.Find("BaseOptions");
        LevelController.buildingMenu.SetActive(false);
        CmdSpawnBase(basePosition, myTeam, myColor);
    }

    [Command]
    public void CmdSpawnBase(Vector3 position, Base_NW.Team thisTeam, Color thisColor)
    {
        var go = (GameObject)Instantiate(myBase, position, Quaternion.identity);
        if (thisTeam == Base_NW.Team.Player2)
            go.transform.Rotate(Vector3.up, 180);
        if (thisTeam == Base_NW.Team.Player1)
            go.transform.Rotate(Vector3.up, -50);
		go.GetComponent<Base_NW>().team = thisTeam;
		go.GetComponent<Base_NW>().teamColor = thisColor;
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }


    [Command]
    void CmdSpawn(int i, Vector3 position, Base_NW.Team thisTeam, Color thisColor)
    {
        var go = Instantiate(tanks[i], position, Quaternion.identity);
		go.GetComponent<Base_NW>().team = thisTeam;
		go.GetComponent<Base_NW>().teamColor = thisColor;
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
    }

    public void SpownTank(int i)
    {
        CmdSpawn(i, LevelController.spawnPoint, myTeam, myColor);
    }
}
