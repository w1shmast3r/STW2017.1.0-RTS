using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayerConfiguration : NetworkBehaviour {

	[SyncVar(hook = "OnChangeLobbyPlayerRaceColor")]
	public Color lobbyPlayerRaceColor = Color.black;

	//we need to show DropDown menu only on LocalPlayer's obects
	public override void OnStartLocalPlayer()
	{
		GetComponentInChildren<Dropdown>(true).gameObject.SetActive(true);
	}

	//this method will be called automatically when user changes dropdown value
	public void DropdownValueChanged(int value)
	{
		Color selectedColor;

		if (value == 0)
			selectedColor = Color.blue;
		else
			selectedColor = Color.red;

		//use Command to change color on the Server's copy of the LobbyPlyer object
		CmdSetRaceColor(selectedColor);
	}

	[Command]
	public void CmdSetRaceColor(Color c)
	{
		// changing value of this variable causes call of hook-function "OnChangeLobbyPlayerGunColor" on copies of the LobbyPlayer object on all Clients
		lobbyPlayerRaceColor = c;
		ChangeColor(c);
	}
	//will be called on all Clients
	public void OnChangeLobbyPlayerRaceColor(Color newColor)
	{
		lobbyPlayerRaceColor = newColor;
		ChangeColor(newColor);
	}

	public void ChangeColor(Color c)
	{
		//TODO Change color of the tank 
/*
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		foreach (var mr in renderers)
		{
			if (mr.name == "Gun")
				mr.material.color = c;
		}
 */
	}
}
