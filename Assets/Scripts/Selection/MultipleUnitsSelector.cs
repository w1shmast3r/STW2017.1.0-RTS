using UnityEngine;
using UnityEngine.EventSystems;

public class MultipleUnitsSelector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// This class to be assigned to terrain to handle start/end of multiple unit selection
    /// </summary>

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            Camera.main.GetComponent<UnitSelectionComponent>().EndMultipleSelection();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Camera.main.GetComponent<UnitSelectionComponent>().StartMultipleSelection();
            //
        }
        else
            Camera.main.GetComponent<UnitSelectionComponent>().Action();
        LevelController.HideMenu();
    }

}