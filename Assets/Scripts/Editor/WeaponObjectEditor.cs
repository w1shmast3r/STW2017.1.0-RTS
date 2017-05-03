using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponObject))]
public class WeaponObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {

        WeaponObject myTarget = (WeaponObject)target;
        base.OnInspectorGUI();

        if (myTarget.hasProjectile)
        {
            myTarget.projectile = (GameObject)EditorGUILayout.ObjectField(myTarget.projectile, typeof(GameObject), false);
            myTarget.projectileType = (EffectRecycler.EffectType)EditorGUILayout.EnumPopup(myTarget.projectileType);
        }


    }


}
