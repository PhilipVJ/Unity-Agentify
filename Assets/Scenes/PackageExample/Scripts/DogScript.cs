using Assets.Unity_Agentify_Package;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogScript : MonoBehaviour
{
    public ControlCenter ControlCenter { get; private set; }
    [SerializeField]
    private GameObject home;
    private AgentBehaviour walkNearHome;

    // Start is called before the first frame update
    void Start()
    {
        // Creating the default strategy behaviours
        walkNearHome = new WalkNearHomeBehaviour(GetComponent<NavMeshAgent>(), home);
        List<AgentBehaviour> behaviours = new List<AgentBehaviour>
        {
            walkNearHome
        };

        BehaviourStrategy defaultStrategy = new BehaviourStrategy(behaviours, (state) => { return true; });
        ControlCenter = new ControlCenter(gameObject, defaultStrategy);

    }

    // Update is called once per frame
    void Update()
    {
        ControlCenter.Update();
    }
}
