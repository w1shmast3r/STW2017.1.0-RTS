using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Unit_NW : Base_NW
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

    private LayerMask attackMask;

    // Use this for initialization
    public override void Start()
    {
        Initialize();
        
    }

    public void Initialize()
    {
        if ((TeamHandler_NW.teamHandler.unitPlayer1.Contains(GetComponent<NetworkIdentity>().netId.Value))
            || (TeamHandler_NW.teamHandler.structurePlayer1.Contains(GetComponent<NetworkIdentity>().netId.Value)))
            team = Team.Player1;
        else if ((TeamHandler_NW.teamHandler.unitPlayer2.Contains(GetComponent<NetworkIdentity>().netId.Value))
            || (TeamHandler_NW.teamHandler.structurePlayer2.Contains(GetComponent<NetworkIdentity>().netId.Value)))
            team = Team.Player2;

        attackMask = 1 << LayerMask.NameToLayer("Active");

        if (team == LevelController.myTeam)
        {
            if (GetComponent<NavMeshObstacle>() != null)
            {
                GetComponent<NavMeshObstacle>().enabled = false;
                Agent = GetComponent<NavMeshAgent>();
                Agent.enabled = true;
                LevelController.RegisterUnit(gameObject);
                Agent.SetDestination(transform.position + Vector3.forward*10);
            }
            else
            {
                LevelController.HideMenu();
            }
        }
        //if (!GetComponent<UnitSpawner>())

            StartCoroutine(FindTarget());
        InitBase();
    }

    void Update()
    {
        if (team != LevelController.myTeam)
            return;

        if (target != null && turret != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - turret.position);
            turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, Time.deltaTime * weapon.turretRotationSpeed);

            if (target.CompareTag(gameObject.tag))
                target = null;
        }
    }

    //find target in radius
    IEnumerator FindTarget()
    {
        while (target == null)
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

            yield return new WaitForSeconds(0.5f); //check every half a second for an enemy
        }

        if (weapon != null)
            StartCoroutine(TrackTarget());
    }



    IEnumerator TrackTarget()
    {
        Base_NW enemy = null;
        while (target != null && target.gameObject.activeSelf && muzzle.Length > 0 && !IsDead)
        {
            for (int j = 0; j < muzzle.Length; j++)
            {

                Vector3 forward = Vector3.Normalize(muzzle[j].TransformDirection(Vector3.forward));
                Vector3 targetDir = Vector3.zero;
                    
                if (target)
                    targetDir = Vector3.Normalize(target.position - muzzle[j].position);

                float facing = Vector3.Dot(forward, targetDir);

                //if turret is not facing towards target, don't fire
                if (facing < 0.95f)
                    break;


                // turret.LookAt(target.transform);
                Vector3 offset = Vector3.up * 0.5f;
                Vector3 direction = Vector3.Normalize((target.position + offset) - muzzle[j].transform.position);
                RaycastHit[] impacts = Physics.RaycastAll(muzzle[j].position, direction, weapon.range, attackMask);


                //scrap target if targeting own team
                if (target.CompareTag(transform.tag))
                    break;

                if (impacts.Length > 0)
                {
                    for (int i = 0; i < impacts.Length; i++)
                    {
                        if (impacts[i].transform.root != transform.root)
                        {
                            GameObject muzzleFlash = EffectRecycler.effectRecycler.GetEffect(weapon.muzzleFlash);
                            muzzleFlash.transform.parent = muzzle[j];
                            muzzleFlash.transform.localPosition = Vector3.zero;
                            muzzleFlash.transform.localRotation = Quaternion.identity;
                            muzzleFlash.transform.localScale = Vector3.one;
                            muzzleFlash.SetActive(true);


                            if (weapon.hasProjectile)
                            {
                                GameObject projectile = EffectRecycler.effectRecycler.GetEffect(weapon.projectileType);
                                ProjectileWeapon projectileScript = projectile.GetComponent<ProjectileWeapon>();
                                projectileScript.StartPosition = muzzle[j].position;
                                projectileScript.EndPosition = impacts[i].point;
                                projectileScript.MyWeapon = weapon;
                                projectileScript.Target = impacts[i].transform.root.gameObject;
                                projectile.tag = transform.tag;
                                projectile.SetActive(true);
                            }
                            else
                            {
                                //cache enemy base class
                                if (enemy == null)
                                    enemy = impacts[i].transform.root.GetComponent<Base_NW>();

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

                yield return new WaitForSeconds(0.1f);
                
            }
            yield return new WaitForSeconds(1 / weapon.rateOfFire);

//            yield return null;
        }

        yield return new WaitForEndOfFrame(); //wait for a frame to avoid 
        StartCoroutine(FindTarget());
    }

}
