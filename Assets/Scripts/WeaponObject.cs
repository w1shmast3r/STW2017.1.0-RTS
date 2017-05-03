using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "RTS/Weapon", order = 1)]
public class WeaponObject : ScriptableObject
{
    public string weaponName = "Weapon name";
    [Header("Stats")]
    public float rateOfFire = 1f; //fire speed in shots per second.
    public float range = 5f; //fire range
    public float damage = 1f; //damage per hit
    public float turretRotationSpeed = 1f; //speed by which turret rotates

    public EffectRecycler.EffectType muzzleFlash = EffectRecycler.EffectType.None;
    public EffectRecycler.EffectType hit = EffectRecycler.EffectType.None;

    public bool hasProjectile = false; //is the weapon hit instant, or is a projectle spawned

    [HideInInspector]
    [SerializeField]
    public GameObject projectile;
    [HideInInspector]
    [SerializeField]
    public EffectRecycler.EffectType projectileType;

}