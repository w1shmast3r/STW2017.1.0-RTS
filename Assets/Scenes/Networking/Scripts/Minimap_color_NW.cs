using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_color_NW : MonoBehaviour
{

    public GameObject Cube;
    // Use this for initialization
    void Start()
    {
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
        Base_NW.Team team = GetComponent<Base_NW>().team;

        switch (team)
        {
            case Base_NW.Team.Neutral:
                gameObject.tag = "Neutral";
                mat.SetColor("_Color", TeamHandler_NW.teamHandler.colorNeutral);
                break;

            case Base_NW.Team.Player1:
                gameObject.tag = "Player1";
                mat.SetColor("_Color", TeamHandler_NW.teamHandler.colorPlayer1);
                break;

            case Base_NW.Team.Player2:
                gameObject.tag = "Player2";
                mat.SetColor("_Color", TeamHandler_NW.teamHandler.colorPlayer2);
                break;

            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
