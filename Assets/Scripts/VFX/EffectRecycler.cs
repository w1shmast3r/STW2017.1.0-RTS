using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class VFXs
{
    /// <summary>
    /// Effect prefabs and how much to allocate.
    /// </summary>

    public string effectName;
    public EffectRecycler.EffectType effectType = EffectRecycler.EffectType.None;
    public GameObject prefab;
    public int count;

    [HideInInspector]
    public List<GameObject> effectPool = new List<GameObject>();
}

public class EffectRecycler : NetworkBehaviour
{

    /// <summary>
    /// Recycler for reusing VFX and creating new ones on demand
    /// </summary>

    //static reference of recycler for each access from other classes
    public static EffectRecycler effectRecycler;

    public List<VFXs> effects = new List<VFXs>();

    public enum EffectType
    {
        None,
        ExplosionSmall,
        ExplosionMedium,
        ExplosionLarge,
        MachineGunMuzzeFlash,
        MachineGunHit,
        CannonMuzzleFlash,
        CannonHit,
        RocketProjectile,
        Bug203Projectile,
        MoveEffect,
        Bug203ProjectileHit,
    }
        
    void Awake()
    {
        effectRecycler = this;
    }

    void Start()
    {
        AllocateEffects();
    }

    void AllocateEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null)
            {
                effects[i].effectPool.Clear();
                for (int j = 0; j < effects[i].count; j++)
                {
                    GameObject go = Instantiate(effects[i].prefab);
                    go.SetActive(false);
                    effects[i].effectPool.Add(go);
                }
            }
        }
    }

    public GameObject GetEffect(EffectType type)
    {
        GameObject returnedEffect = null;

        //look for effect not being used. If none are available, create new one.
        for (int i = 0; i < effects.Count; i++)
        {
            if (effects[i] != null)
            {
                if (effects[i].effectType == type)
                {
                    returnedEffect = SearchRecycledEffects(effects[i].effectPool);
                    if (returnedEffect == null)
                        returnedEffect = CreateNewEffect(i);
                }
            }
            else
                effects.RemoveAt(i);
        }
        return returnedEffect;
    }

    GameObject SearchRecycledEffects(List<GameObject> list)
    {
        GameObject result = null;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
                continue;

            if (!list[i].activeInHierarchy)
            {
                result = list[i];
                break;
            }
        }
        return result;
    }

    GameObject CreateNewEffect(int index)
    {
        GameObject go = Instantiate<GameObject>(effects[index].prefab);
        go.SetActive(false);
        effects[index].effectPool.Add(go);
        return go;
    }
}
