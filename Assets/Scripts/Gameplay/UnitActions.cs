using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class UnitActions : MonoBehaviour
{
    [SerializeField]
    bool hasAnimation;

    Animator animator;
    NavMeshAgent agent;

    void Start()
    {
        if (hasAnimation)
            animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 destination)
    {
        agent.SetDestination(destination);
        GameObject moveFX = EffectRecycler.effectRecycler.GetEffect(EffectRecycler.EffectType.MoveEffect);
        moveFX.transform.position = destination;
        moveFX.SetActive(true);

        if (hasAnimation)
        {
            StartCoroutine(UnitMoving());
        }
    }

    IEnumerator UnitMoving()
    {
        animator.SetInteger("state", 7);
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (GetComponent<Unit>().IsDead)
                break;
            if (agent.remainingDistance < 0.1f)
            {
                animator.SetInteger("state", 0);
                break;
            }
        }
    }
}
