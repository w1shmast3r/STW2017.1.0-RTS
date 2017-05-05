using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VehicleConstructionMenuItem : MonoBehaviour {
    public LevelController levelController;
    public float timeToBuildVehicle;
    bool inConstruction;
    Text textGO;
    int unitIndex;

    public void ConstructVehicle (int unitIndex)
    {
        if (inConstruction) {
            return;
        }
        inConstruction = true;
        textGO = GetComponentInChildren<Text> ();
        this.unitIndex = unitIndex;
        StartCoroutine (StartUnitSpawn());
    }

    IEnumerator StartUnitSpawn () {
        int progress = 0;
        textGO.text = progress.ToString ();
        float tickTime = timeToBuildVehicle / 100;
        float elapsedTime = 0;
        while (progress < 100) {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= tickTime) {
                progress++;
                textGO.text = progress.ToString ();
                elapsedTime = 0;
            }
            yield return new WaitForEndOfFrame ();
        }
        StartCoroutine (SpawnUnit ());
        textGO.text = "";
        inConstruction = false;
    }

    IEnumerator SpawnUnit () {
        levelController.SpawUnit (unitIndex);
        yield return null;
    }
}
