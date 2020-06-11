using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class WalkNearHomeBehaviour : AgentBehaviour
{
    private readonly NavMeshAgent agent;
    private Vector3 randomPosition;
    private readonly GameObject home;
    private readonly Stopwatch timer;

    public WalkNearHomeBehaviour(NavMeshAgent agent, GameObject home)
    {
        this.agent = agent;
        this.home = home;
        timer = new Stopwatch();
        timer.Start();
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
        if (!GameManager.isNight)
        {
            SetDestinationOnAnimail();
        }
        else // Take breaks
        {
            if (timer.ElapsedMilliseconds > 10 * 1000)
            {
                SetDestinationOnAnimail();
                timer.Restart();
            }
        }
    }

    private void SetDestinationOnAnimail()
    {
        if (randomPosition == Vector3.zero)
        {
            randomPosition = GetRandomPositionNearHome(home.transform.position);
            agent.SetDestination(randomPosition);
        }

        if (agent.remainingDistance < 1)
        {
            randomPosition = Vector3.zero;
        }
    }


    private Vector3 GetRandomPositionNearHome(Vector3 homePosition)
    {
        Vector3 randomPos = Random.insideUnitSphere * 10 + homePosition;
        randomPos.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, 11, NavMesh.AllAreas);
        return hit.position;
    }
}
