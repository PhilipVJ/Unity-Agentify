using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EatNearbyFoodBehaviour : AgentBehaviour
{
    private readonly NavMeshAgent agent;
    private readonly AgentState state;
    private Transform transform;
    private GameObject[] food;
    private GameObject foundFood;

    public EatNearbyFoodBehaviour(NavMeshAgent agent, Transform transform, AgentState state)
    {
        this.agent = agent;
        this.state = state;
        this.transform = transform; 
    }

    public override bool TakeControl()
    {
        if (state.foodSupply > 2)
        {
            return false;
        }

        bool takeControl = false;
        food = GameObject.FindGameObjectsWithTag("Food");
        List<GameObject> foodsNearby = new List<GameObject>();
        foreach (var gameObject in food)
        {
            double distance = Vector3.Distance(transform.position, gameObject.transform.position);
            if (distance < 30)
            {
                foodsNearby.Add(gameObject);
            }
        }
        if (foodsNearby.Count > 0)
        {
            takeControl = true;
            if (foundFood == null)
            {
                int randomNumber = UnityEngine.Random.Range(0, foodsNearby.Count);
                foundFood = foodsNearby[randomNumber];
            }
        }
        return takeControl;
    }

    public override void Update()
    {
        if (foundFood.gameObject != null && agent.destination != foundFood.gameObject.transform.position)
        {
            agent.SetDestination(foundFood.transform.position);
        }
    }

    public override void Suppress()
    {
        foundFood = null;
    }

}
