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
   
    public void Start()
    {
        if (GetComponent<Base_NW>().team == LevelController.myTeam)
        {
            LevelController.spawnPoint = spawnPosition.position;
            LevelController.meetPoint = meetPoint.position;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GetComponent<Base_NW>().team == LevelController.myTeam)
            ShowBuildingMenu();
    }

    private void ShowBuildingMenu() {
        LevelController.buildingMenu.SetActive(true);
    }
}
