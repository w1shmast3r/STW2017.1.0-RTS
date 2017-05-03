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
        Debug.Log("In OnPointerClick");
        if (units.Count < 1)        {
            Debug.LogError("You need to add units before spawning.");
            return;
        }

        ShowBuildingMenu();
    }


    public void SpawnUnit()
    {
        var unit = Instantiate(units[0], spawnPosition.position, Quaternion.identity) as GameObject;
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        agent.SetDestination(meetPoint.position);

       // var mover = unit.GetComponent<UnitMover>();
       // NetworkServer.Spawn(unit);
        //mover.SetDestination(meetPoint1.position);
    }


    public void SpawnUnit1()
    {
        var unit = Instantiate(units[1], spawnPosition.position, Quaternion.identity) as GameObject;
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        agent.SetDestination(meetPoint.position);

        // var mover = unit.GetComponent<UnitMover>();
        // NetworkServer.Spawn(unit);
        //mover.SetDestination(meetPoint1.position);
    }

    public void SpawnUnit2()
    {
        var unit = Instantiate(units[2], spawnPosition.position, Quaternion.identity) as GameObject;
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        agent.SetDestination(meetPoint.position);

        // var mover = unit.GetComponent<UnitMover>();
        // NetworkServer.Spawn(unit);
        //mover.SetDestination(meetPoint1.position);
    }

    public void SpawnUnit3()
    {
        var unit = Instantiate(units[3], spawnPosition.position, Quaternion.identity) as GameObject;
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        agent.SetDestination(meetPoint.position);

        // var mover = unit.GetComponent<UnitMover>();
        // NetworkServer.Spawn(unit);
        //mover.SetDestination(meetPoint1.position);
    }

    public void SpawnUnit4()
    {
        var unit = Instantiate(units[4], spawnPosition.position, Quaternion.identity) as GameObject;
        NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
        agent.SetDestination(meetPoint.position);

        // var mover = unit.GetComponent<UnitMover>();
        // NetworkServer.Spawn(unit);
        //mover.SetDestination(meetPoint1.position);
    }
    //    [Command]
    //    private void CmdSpawnUnit()
    //    {
    //        var unit = Instantiate(GetRandomUnit(), spawnPosition.position, Quaternion.identity) as GameObject;
    //        var mover = unit.GetComponent<UnitMover>();
    //        NetworkServer.Spawn(unit);
    //        mover.SetDestination(meetPoint.position);
    //    }

    //    [Command]
    //    public void CmdSpawnUnit(int index) {
    //        var unit = Instantiate(units[index], spawnPosition.position, Quaternion.identity) as GameObject;
    //        var mover = unit.GetComponent<UnitMover>();
    //        NetworkServer.Spawn(unit);
    //        mover.SetDestination(meetPoint.position);
    //    }
    //
    private void ShowBuildingMenu() {
        //buildingMenu.SetActive(true);
        LevelController.buildingMenu.SetActive(true);

    }

    private GameObject GetRandomUnit()
    {
        return units[Random.Range(0, units.Count)];
    }
}
