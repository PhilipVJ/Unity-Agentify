using Assets.Scenes.PackageExample.Behaviours;
using Assets.Unity_Agentify_Package;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BirdScript : MonoBehaviour
{
    public ControlCenter ControlCenter { get; private set; }
    [SerializeField]
    private GameObject tree;
    [SerializeField]
    private GameObject tree2;
    [SerializeField]
    private GameObject tree3;
    [SerializeField]
    private GameObject tree4;
    [SerializeField]
    private GameObject palm;

    private AgentBehaviour flyAround;
    private AgentBehaviour migrate;

    // Start is called before the first frame update
    void Start()
    {
        // Creating the default strategy behaviours
        flyAround = new FlyAroundBehaviour(GetComponent<NavMeshAgent>(), tree, tree2, tree3, tree4);
        migrate = new MigrateBehaviour(GetComponent<NavMeshAgent>(), palm);

        List<AgentBehaviour> behaviours = new List<AgentBehaviour>
        {
            flyAround,
            migrate
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
