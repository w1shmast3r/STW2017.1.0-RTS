using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Base
{

    ///<summary>
    /// Unit class, including movement and weapons.
    ///</summary>

    [Header("Weapons")]
    public WeaponObject weapon;
    public Transform turret;
    public Transform[] muzzle;
    public Transform target;

    public Transform Target
    {
        get { return target; }
        set { target = value; }
    }

    LayerMask attackMask;
    bool canAttack;

    // Use this for initialization
    public override void Start()
    {
        attackMask = 1 << LayerMask.NameToLayer("Active");

        //if this unit has a weapon, look for target
        if (weapon != null)
        {
            StartCoroutine(EnterWarMode());
        }

        base.Start();
    }

    IEnumerator EnterWarMode()
    {
        // Control behaviours here?
        while (!IsDead)
        {
            if (target == null)
            {
                FindTarget();
                yield return new WaitForSeconds(0.1f);
            }
            else
                yield return StartCoroutine(AttackTarget());
        }
    }

    IEnumerator AttackTarget()
    {
        StartCoroutine(AlignForAttack());
        while (target != null && target.gameObject.activeSelf && muzzle.Length > 0 && !IsDead)
        {
            if (canAttack)
            {
                FireWeapon();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1 / weapon.rateOfFire);
        }
    }

    IEnumerator AlignForAttack()
    {
        while (target != null && !IsDead)
        {
            if (IsAlignedForAttack())
            {
                canAttack = true;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                canAttack = false;
                if (turret)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(target.position - turret.position);
                    turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, Time.deltaTime * weapon.turretRotationSpeed);
                }
                else
                {
                    Vector3 direction = (target.position - transform.position).normalized;
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    bool IsAlignedForAttack()
    {
        Vector3 forward = Vector3.Normalize(muzzle[0].TransformDirection(Vector3.forward));
        Vector3 targetDir = Vector3.zero;

        targetDir = Vector3.Normalize(target.position - muzzle[0].position);

        float facing = Vector3.Dot(forward, targetDir);

        if (facing < 0.95f)
            return false;
        return true;
    }

    void FireWeapon()
    {
        Base enemy = null;

        // turret.LookAt(target.transform);
        Vector3 offset = Vector3.up * 0.5f;
        Vector3 direction = Vector3.Normalize((target.position + offset) - muzzle[0].transform.position);
        RaycastHit[] impacts = Physics.RaycastAll(muzzle[0].position, direction, weapon.range, attackMask);

        if (impacts.Length > 0)
        {
            for (int i = 0; i < impacts.Length; i++)
            {
                if (impacts[i].transform.root != transform.root)
                {
                    GameObject muzzleFlash = EffectRecycler.effectRecycler.GetEffect(weapon.muzzleFlash);
                    muzzleFlash.transform.parent = muzzle[0];
                    muzzleFlash.transform.localPosition = Vector3.zero;
                    muzzleFlash.transform.localRotation = Quaternion.identity;
                    muzzleFlash.transform.localScale = Vector3.one;
                    muzzleFlash.SetActive(true);

                    if (weapon.hasProjectile)
                    {
                        GameObject projectile = EffectRecycler.effectRecycler.GetEffect(weapon.projectileType);
                        ProjectileWeapon projectileScript = projectile.GetComponent<ProjectileWeapon>();
                        projectileScript.StartPosition = muzzle[0].position;
                        projectileScript.EndPosition = impacts[i].point;
                        projectileScript.MyWeapon = weapon;
                        projectileScript.Target = impacts[i].transform.root.gameObject;
                        projectile.tag = transform.tag;
                        projectile.SetActive(true);

                        GetComponent<Animator>().SetInteger("state", 1);
                    }
                    else
                    {
                        //cache enemy base class
                        if (enemy == null)
                            enemy = impacts[i].transform.root.GetComponent<Base>();

                        GameObject hit = EffectRecycler.effectRecycler.GetEffect(weapon.hit);
                        hit.transform.position = impacts[i].point;
                        hit.SetActive(true);

                        if (enemy != null && enemy.TakeDamage(weapon.damage))
                            target = null;

                        break;
                    }
                }
            }
        }
    }
        
    //find target in radius
    void FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, weapon.range, attackMask);

        Transform newTarget = null;
        float distance = Mathf.Infinity;

        for (int i = 0; i < targets.Length; i++)
        {
            //check if potential target belongs to different team
            if (targets[i].transform.root != transform.root && !targets[i].CompareTag(gameObject.tag))
            {
                float newDistance = (targets[i].transform.position - transform.position).sqrMagnitude;
                if (newDistance < distance)
                {
                    distance = newDistance;
                    newTarget = targets[i].transform;
                }
            }
        }

        //set target, if a new one is found
        if (newTarget)
            target = newTarget;
  
    }

    void OnDestroy()
    {
        LevelController.UnregisterUnit(gameObject);
    }
}
