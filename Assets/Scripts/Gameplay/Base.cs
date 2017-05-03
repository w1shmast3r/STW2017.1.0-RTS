using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Base : MonoBehaviour
{
    ///<summary>
    ///Base class for all units and structures.
    ///</summary>
    ///
    
    public enum Team
    {
        My,
        Enemy,
        Neutral
    }

    [Header("General")]
    public Team team = Team.Enemy;
    public string className = "Unnamed";
    public string description = "Purpose of this unit/structure.";

    [Header("Attributes")]
    public float maxHealth = 1f;
    public float armor = 0f;
    private float currentHealth;

    [Header("VFX")]
    public EffectRecycler.EffectType explosionType = EffectRecycler.EffectType.ExplosionSmall;

    public bool IsDead { get; set; }
    public Transform MyTransform { get; set; }

    public virtual void Start()
    {
        currentHealth = maxHealth;
        MyTransform = transform;

        var renderer = GetComponent<Renderer>();
        if (renderer == null)
            renderer = GetComponentInChildren<Renderer>();

        var meterial = renderer.material;
        Renderer[] visuals = transform.GetComponentsInChildren<Renderer>();
        Color minimapColor = Color.clear;
        
        switch (team)
        {
            case Team.Neutral:
                gameObject.tag = "Neutral";
                meterial.SetColor("_Color", TeamHandler.teamHandler.colorNeutral);
                minimapColor = TeamHandler.teamHandler.colorNeutral;
                break;
            case Team.My:
                gameObject.tag = "My";
                meterial.SetColor("_Color", TeamHandler.teamHandler.colorPlayer1);
                minimapColor = TeamHandler.teamHandler.colorPlayer1;
                break;
            case Team.Enemy:
                gameObject.tag = "Enemy";
                meterial.SetColor("_Color", TeamHandler.teamHandler.colorPlayer2);
                minimapColor = TeamHandler.teamHandler.colorPlayer2;
                break;
        }

        foreach (Renderer rend in visuals)
        {
            if (!rend.CompareTag("MiniMapBlob"))
                rend.material = meterial;
            else
                rend.material.SetColor("_Color", minimapColor);
        }

    }

    void OnEnable()
    {
        IsDead = false;
    }

    public bool TakeDamage(float damage)
    {
        bool isDestroyed = false;

        //subtract damage from armor value
        if (damage > armor)
            damage -= armor;
        //if damage is lower than armor, ensure at least 1 damage is done
        else
            damage = 1f;

        //take damage
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isDestroyed = true;
            IsDead = isDestroyed;
            Destroy();
        }
            
        return isDestroyed;
    }

    //kill off unit
    private void Destroy()
    {
        GameObject explosion = EffectRecycler.effectRecycler.GetEffect(explosionType);
        explosion.transform.position = transform.position + Vector3.up * 0.5f;
        explosion.SetActive(true);
        Destroy(gameObject);
    }
}
