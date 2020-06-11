using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GetWaterBehaviour : AgentBehaviour
{
    private readonly NavMeshAgent agent;
    private readonly Transform transform;
    private GameObject[] water;
    private GameObject waterObject;
    private readonly AgentState state;

    public GetWaterBehaviour(NavMeshAgent agent, Transform transform, AgentState state)
    {
        this.agent = agent;
        this.state = state;
        this.transform = transform;

    }

    public override void Suppress()
    {
       
    }

    public override bool TakeControl()
    {
        if (state.hydrationLevel > 25)
        {
            return false;
        }

        bool takeControl = false;
        water = GameObject.FindGameObjectsWithTag("Water");
        foreach (var gameObject in water)
        {
            double distance = Vector3.Distance(transform.position, gameObject.transform.position);
            if (distance < 20)
            {
                takeControl = true;
                waterObject = gameObject;
                break;
            }
        }
        return takeControl;
    }

    public override void Update()
    {
        if (waterObject != null  && waterObject.transform.position != agent.destination)
        {
            agent.SetDestination(waterObject.transform.position);
        }
    }
}

