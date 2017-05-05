using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap_color_NW : MonoBehaviour
{
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

        Material mat = mapArea.GetComponent<Renderer>().material;
        mat.SetColor("_Color", GetComponent<Base_NW>().teamColor);
    }
}
