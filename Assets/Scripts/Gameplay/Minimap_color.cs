using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_color : MonoBehaviour {

    public GameObject Cube;
	// Use this for initialization
	void Start () {
        GameObject mapArea = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mapArea.name = "MapArea";
        mapArea.layer = 12;
        mapArea.tag = "MiniMapBlob";
        Destroy(mapArea.GetComponent<Collider>());
        mapArea.transform.parent = this.transform;
        mapArea.transform.parent = transform;
        mapArea.transform.localScale = new Vector3(20, 20, 20);
        mapArea.transform.localPosition = new Vector3(0, 0, 0);
        //mapArea.GetComponent<Renderer>().material = GetComponent<Renderer>().material; // This might be needed after the tag coloring is fixed
       
        Material mat = mapArea.GetComponent<Renderer>().material;
        Base.Team team = GetComponent<Base>().team;

        switch (team)
        {
            case Base.Team.Neutral:
                gameObject.tag = "Neutral";
                mat.SetColor("_Color", TeamHandler.teamHandler.colorNeutral);
                break;

            case Base.Team.My:
                gameObject.tag = "Player1";
                mat.SetColor("_Color", TeamHandler.teamHandler.colorPlayer1);
                break;

            case Base.Team.Enemy:
                gameObject.tag = "Player2";
                mat.SetColor("_Color", TeamHandler.teamHandler.colorPlayer2);
                break;

            default:
                break;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
