using UnityEngine;
using UnityEngine.AI;

public class GetHomeAndRestBehaviour : AgentBehaviour
{
    private readonly NavMeshAgent agent;
    private readonly AgentState state;
    private readonly GameObject restingPlace;
    private bool startedWalkHome = false;
    private bool reachedHome = false;


    public GetHomeAndRestBehaviour(NavMeshAgent agent, GameObject restingPlace, AgentState state)
    {
        this.agent = agent;
        this.restingPlace = restingPlace;
        this.state = state;
    }

    public override bool TakeControl()
    {
        if (state.foodSupply >= 5 && state.hydrationLevel > 70)
            return true;
        if (reachedHome && state.foodSupply > 3 && state.hydrationLevel >= 50) // Stay home for some time
            return true;
        else
            return false;
    }
    public override void Update()
    {
        if (!startedWalkHome)
        {
            agent.SetDestination(restingPlace.transform.position);
            startedWalkHome = true;
        }
 
        if (agent.remainingDistance < 0.1f && !agent.pathPending && !reachedHome)
        {
            reachedHome = true;
            agent.ResetPath();
        }
    }

    public override void Suppress()
    {

        reachedHome = false;
        startedWalkHome = false;
    }
}
