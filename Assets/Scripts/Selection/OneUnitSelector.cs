using UnityEngine;
using UnityEngine.EventSystems;

public class OneUnitSelector : MonoBehaviour, IPointerClickHandler
{
    private UnitSelectionComponent selector;

    void Awake()
    {
        selector = Camera.main.GetComponent<UnitSelectionComponent>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Click on enemy
        LevelController.HideMenu();

        if (!LevelController.playerUnits.Contains(gameObject))
        {
            Enemy(eventData);
            return;
        }

        // Left button
        if (eventData.button == PointerEventData.InputButton.Left)
            selector.SelectUnit(gameObject);

        
    }

    public void Enemy(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
            Debug.Log("I gona atack");
    }

}
