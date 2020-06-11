using UnityEngine;
using UnityEngine.AI;

public class WalkAroundBehaviour : AgentBehaviour
{
    private readonly NavMeshAgent agent;
    private Vector3 randomPosition;

    public WalkAroundBehaviour(NavMeshAgent agent)
    {
        this.agent = agent;
    }


    public override void Suppress()
    {
        randomPosition = Vector3.zero;
    }

    public override bool TakeControl()
    {
        return true;
    }

    public override void Update()
    {
        if (randomPosition == Vector3.zero)
        {
            randomPosition = GetRandomPoint();
            agent.SetDestination(randomPosition);
        }

        if (agent.remainingDistance<1)
        {
            randomPosition = Vector3.zero;
        }

    }


    private Vector3 GetRandomPoint()
    {
        Vector3 randomPos = Random.insideUnitSphere * 25 + Vector3.zero;
        randomPos.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, 25, NavMesh.AllAreas);
        return hit.position;
    }
}
