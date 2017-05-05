using UnityEngine;
using UnityEngine.Networking;

public class MyLobbyManager : NetworkLobbyManager
{
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
	{
		//we need to transfer all user made settings from lobbyPlayer object to gamePlayer here
		Color colorFromLobbyPlayers = lobbyPlayer.GetComponent<LobbyPlayerConfiguration>().lobbyPlayerRaceColor;

		if (colorFromLobbyPlayers != Color.black)
		{
			gamePlayer.GetComponent<PlyerScript>().myColor = colorFromLobbyPlayers;
		}

		//disable LobbyPlayer object as we don't need it anymore on the scene
		lobbyPlayer.SetActive(false);
		return true;
	}
	
}
