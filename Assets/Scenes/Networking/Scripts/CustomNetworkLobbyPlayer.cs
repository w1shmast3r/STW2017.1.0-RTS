using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkLobbyPlayer : NetworkLobbyPlayer
{
	public void Start()
	{
		DontDestroyOnLoad(gameObject);
		if (!isLocalPlayer)
			gameObject.SetActive(false);
	}
	public override void OnClientExitLobby()
	{
		gameObject.SetActive(false);
	}
}
