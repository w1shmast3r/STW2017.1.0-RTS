using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class UnitMover : NetworkBehaviour
{
    private NavMeshAgent agent;

    public override void OnStartServer()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void SetDestination(Vector3 target)
    {
        if (agent == null)
            return;

        agent.SetDestination(target);
    }
}
