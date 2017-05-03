using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    public float projectileSpeed = 1f;
    public float verticalTrajectoryArc = 0f;
    public float arcStartDistance = 0f;
    public float hideDelay = 1f;//time after impact before the effect is hidden
    public GameObject projectileObject;
    public ParticleSystem projectileParticles;

    public Vector3 StartPosition { get; set; }

    public Vector3 EndPosition { get; set; }

    public WeaponObject MyWeapon { get; set; }

    public GameObject Target { get; set; }

    LayerMask attackMask;

    void OnEnable()
    {
        StopAllCoroutines();
        attackMask = 1 << LayerMask.NameToLayer("Active");
        projectileObject.SetActive(true);
       
        projectileParticles.Play();

        float distanceToTarget = Vector3.Distance(StartPosition, EndPosition);
        float arcModifier = 0f;

        //no arc at arcStartDistance, gradually increase arc with distance up to max verticalArc value
        if (distanceToTarget > arcStartDistance)
            arcModifier = Mathf.Min(Mathf.Lerp(0, distanceToTarget * 0.5f, distanceToTarget * 0.1f), verticalTrajectoryArc);

        float timeModifier = MyWeapon.range / distanceToTarget; //speed up projectile when target is closer

        StartCoroutine(AnimateProjectile(arcModifier, timeModifier));
    }

    IEnumerator AnimateProjectile(float arcModifier, float timeModifier)
    {
        bool hasHit = false;
        float timer = 0.0f;
        while (!hasHit && timer < 1)
        {
            timer += Time.deltaTime * projectileSpeed * timeModifier;
            Vector3 newEndPos = Vector3.Lerp(
                EndPosition, 
                EndPosition + Vector3.up * arcModifier, 
                Mathf.Sin(Mathf.Lerp(0, Mathf.PI, timer)));
            Vector3 curPos = Vector3.Lerp(StartPosition, newEndPos, timer);
            transform.LookAt(curPos);
            transform.position = curPos;

            RaycastHit hit;
            float distance = 3f;                                                //Why 3?
            Vector3 dir = Vector3.Normalize(EndPosition - transform.position);
            Debug.DrawRay(curPos, dir, Color.red);
            if (Physics.Raycast(curPos, dir, out hit, distance, attackMask))
            {
                if (!hit.transform.CompareTag(transform.tag))
                {
                    Base_NW enemy = hit.transform.root.GetComponent<Base_NW>();

                    GameObject impact = EffectRecycler.effectRecycler.GetEffect(MyWeapon.hit);
                    impact.transform.position = hit.point;
                    impact.SetActive(true);

                    if (enemy != null)
                        enemy.TakeDamage(MyWeapon.damage);

                    hasHit = true;

                    if (projectileObject)
                        projectileObject.SetActive(false);

                    StartCoroutine(HideEffect());
                }
            }
            else if (Target == null)
            {
                if (projectileObject)
                    projectileObject.SetActive(false);
                StartCoroutine(HideEffect());
            }
            yield return null;
        }
    }

    IEnumerator HideEffect()
    {
        yield return new WaitForSeconds(hideDelay);

        if (projectileParticles)
            projectileParticles.Stop();

        gameObject.SetActive(false);
    }
}
