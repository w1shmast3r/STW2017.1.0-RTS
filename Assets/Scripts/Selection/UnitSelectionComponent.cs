using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class UnitSelectionComponent : MonoBehaviour
{
    public bool Networking;

    private bool isSelecting;
    private Vector3 mousePosition1;
    private List<GameObject> selectedUnits = new List<GameObject>();

    public GameObject selectionCirclePrefab;
    public GameObject vehicleMenu;

    public void SelectUnit(GameObject unit)
    {
        ClearPreviousUnitSelection();
        StartCoroutine(SelectingOneUnit(unit));
    }

    public void StartMultipleSelection()
    {
        HideVehicleMenu();
        isSelecting = true;
        mousePosition1 = Input.mousePosition;
        ClearPreviousUnitSelection();
    }

    void ClearPreviousUnitSelection()
    {
        foreach (var selectableObject in LevelController.playerUnits)
        {
            var selectableComponent = selectableObject.GetComponent<SelectableUnitComponent>();
            if (selectableComponent.selectionCircle != null)
            {
                Destroy(selectableComponent.selectionCircle.gameObject);
                selectableComponent.selectionCircle = null;
            }
        }
    }

    public void EndMultipleSelection()
    {
        selectedUnits = new List<GameObject>();
        foreach (var selectableObject in LevelController.playerUnits)
        {
            if (IsWithinSelectionBounds(selectableObject))
            {
                selectedUnits.Add(selectableObject);
            }
        }

        isSelecting = false;

        if (selectedUnits != null && selectedUnits.Count > 0)
            ShowVehicleMenu();
    }

    IEnumerator SelectingOneUnit(GameObject unit)
    {
        yield return new WaitForSeconds(0.1f);

        selectedUnits = new List<GameObject>();

        selectedUnits.Add(unit);
        var selection = unit.GetComponent<SelectableUnitComponent>();
        if (selection.selectionCircle == null)
        {
            InstantiateSelectionCircle(selection);
        }
        ShowVehicleMenu();
    }

    private void InstantiateSelectionCircle(SelectableUnitComponent selection)
    {
        selection.selectionCircle = Instantiate(selectionCirclePrefab);
        selection.selectionCircle.transform.SetParent(selection.transform, false);
        selection.selectionCircle.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void Update()
    {
        LevelController.Clean();

        if (isSelecting)
            HighliteSellected();

    }

    private void HighliteSellected()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            if (selectedUnits[i] == null)
                selectedUnits.RemoveAt(i);
        }
        foreach (var selectableObject in LevelController.playerUnits)
        {
            if (selectableObject == null)
                continue;

            var selectableComponent = selectableObject.GetComponent<SelectableUnitComponent>();
            if (IsWithinSelectionBounds(selectableObject))
            {
                if (selectableComponent.selectionCircle == null)
                    InstantiateSelectionCircle(selectableComponent);
            }
            else
            {
                if (selectableComponent.selectionCircle != null)
                    Destroy(selectableComponent.selectionCircle.gameObject);
            }
        }
    }

    public void Action()
    {
        if (selectedUnits == null || selectedUnits.Count == 0)
            return;
        LevelController.Clean();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
        {
            foreach (var unit in selectedUnits)
            {
                if (unit != null)
                {
                    unit.GetComponent<UnitActions>().Move(hit.point);
                }
                else
                {
                    Debug.LogError("Got null in selectedUnits");
                }
            }
        }
    }

    void ShowVehicleMenu()
    {
        if (Networking)
            return;
        vehicleMenu.SetActive(true);
    }

    void HideVehicleMenu()
    {
        if (Networking)
            return;
        vehicleMenu.SetActive(false);
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}