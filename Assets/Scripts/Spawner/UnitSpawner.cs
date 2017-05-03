using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.Networking;

public class UnitSpawner : MonoBehaviour, IPointerClickHandler
{
    public Transform spawnPosition;
    public Transform meetPoint;
    public List<GameObject> units;
    public GameObject buildingMenu;
   // public NavMeshAgent agent;


    public void OnPointerClick(PointerEventData eventData)
    {
        ShowBuildingMenu();
    }

    private void ShowBuildingMenu() {
        LevelController.buildingMenu.SetActive(true);
    }
}
