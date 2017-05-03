using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.Networking;

public class UnitSpawner : NetworkBehaviour, IPointerClickHandler
{
    public Transform spawnPosition;
    public Transform meetPoint;
    public List<GameObject> units;
    public GameObject buildingMenu;
   // public NavMeshAgent agent;


    public void Start()
    {
        var netID = GetComponent<NetworkIdentity>().netId.Value;
        if (TeamHandler_NW.teamHandler.GetMyTeam(netID) == LevelController.myTeam)
        {
            LevelController.spawnPoint = spawnPosition.position;
            LevelController.meetPoint = meetPoint.position;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        ShowBuildingMenu();
    }

    private void ShowBuildingMenu() {
        LevelController.buildingMenu.SetActive(true);
    }
}
