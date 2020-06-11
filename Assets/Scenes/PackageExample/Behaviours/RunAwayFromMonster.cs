using UnityEngine;
using UnityEngine.AI;

public class RunAwayFromMonster : AgentBehaviour
{
    private readonly NavMeshAgent agent;
    private Vector3 randomPosition;
    private readonly Transform transform;
    private GameObject monster;

    public RunAwayFromMonster(NavMeshAgent agent, Transform transform)
    {
        this.agent = agent;
        this.transform = transform;
    }


    public override void Suppress()
    {
        randomPosition = Vector3.zero;
        monster = null;
    }

    public override bool TakeControl()
    {
        bool takeControl = false;
        var monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var monster in monsters)
        {
            double distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < 5)
            {
                this.monster = monster;
                takeControl = true;
                break;
            }
        }
        return takeControl;
    }

    public override void Update()
    {
        if (randomPosition == Vector3.zero)
        {
            randomPosition = GetRandomPositionAwayFromMonster(monster.GetComponent<Transform>().position, monster.transform.forward);
            agent.SetDestination(randomPosition);
        }
    }


    private Vector3 GetRandomPositionAwayFromMonster(Vector3 monsterPosition, Vector3 monsterForward)
    {
        Vector3 point = monsterPosition + monsterForward * 6;
        point.y = 0;
        NavMeshHit hit;
        NavMesh.SamplePosition(point, out hit, 5, NavMesh.AllAreas);
        return hit.position;
    }
}
