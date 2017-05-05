using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyManager : NetworkLobbyManager
{
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		//we need to transfer all user made settings from lobbyPlayer object to gamePlayer here
		Color colorFromLobbyPlayersGun = lobbyPlayer.GetComponent<LobbyPlayerConfiguration>().lobbyPlayerRaceColor;

		if (colorFromLobbyPlayersGun != Color.black)
		{
			gamePlayer.GetComponent<PlyerScript>().myColor = colorFromLobbyPlayersGun;
		}

		//disable LobbyPlayer object as we don't need it anymore on the scene
		lobbyPlayer.SetActive(false);
		return true;
	}
	
}
