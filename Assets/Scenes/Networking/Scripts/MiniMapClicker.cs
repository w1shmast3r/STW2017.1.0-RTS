using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapClicker : MonoBehaviour, IPointerClickHandler
{
    public GameObject miniMap;
    public Camera miniCamera;
    public ISRTSCamera mainCamera;

	void Start () 
    {
        miniCamera = miniMap.GetComponent<Camera>();

        mainCamera = Camera.main.GetComponent<ISRTSCamera> ();
		
	}
	
    public void Update ()
    {        
        if (mainCamera.followingTarget == null)
            return;
                   
        //Debug.Log (Input.GetMouseButtonDown(1));
        if (Input.GetMouseButtonDown (1) && miniCamera.pixelRect.Contains (Input.mousePosition)) 
        {
            //Debug.Log (Input.mousePosition);
            var point = miniCamera.ScreenToWorldPoint (Input.mousePosition);
            var follow = mainCamera.followingTarget;
            follow.position = point;
            mainCamera.Follow (follow);
        }
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        //throw new NotImplementedException ();
        Debug.Log ("I was clicked");
    }
}
