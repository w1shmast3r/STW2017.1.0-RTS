using UnityEngine;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

    /// <summary>
    /// This class for general logic within level
    /// </summary>
	
    public static List<GameObject> playerUnits;

    public static Base_NW.Team myTeam;
    public static GameObject myBase;
    public static PlyerScript player;

    public static GameObject buildingMenu;

    public static void HideMenu()
    {
        if(buildingMenu != null)
            buildingMenu.SetActive(false);
    }

    public static int units;

    void Start() {
        playerUnits = new List<GameObject>();
        var objects = FindObjectsOfType<SelectableUnitComponent>();
        foreach (var obj in objects) {
            RegisterUnit(obj.gameObject);
        }
    }

    public void SpawUnit(int i)
    {
        player.SpownTank(i);
    }

    public static void RegisterUnit(GameObject unit)
    {
        playerUnits.Add(unit);
        units = playerUnits.Count;
    }

    public static void Clean()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i] == null)
                playerUnits.RemoveAt(i);
        }
    }

    public static void UnregisterUnit(GameObject unit)
    {
        playerUnits.Remove(unit);
    }
}
