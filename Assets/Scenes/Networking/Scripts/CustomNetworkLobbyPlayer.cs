using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkLobbyPlayer : NetworkLobbyPlayer
{
	public override void OnClientExitLobby()
	{
		gameObject.SetActive(false);
	}
}
