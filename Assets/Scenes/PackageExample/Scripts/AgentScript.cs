using Assets.Scenes.PackageExample.Behaviours;
using Assets.Unity_Agentify_Package;
using Assets.Unity_Agentify_Package.Managers;
using Assets.Unity_Agentify_Package.Model;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class AgentScript : MonoBehaviour, ICommunicationLine
{
    public ControlCenter ControlCenter { get; private set; }
    private AgentState state;
    private AgentBehaviour walkAround;
    private AgentBehaviour eatFood;
    private AgentBehaviour homeAndRest;
    private AgentBehaviour findWater;
    private AgentBehaviour awayFromMonster;
    private BehaviourStrategy defaultStrategy;

    [SerializeField]
    private GameObject restingPlace;
    [SerializeField]
    private GameObject disco;

    private Stopwatch lifeTime;
    private Stopwatch takeHealthTimer;

    private GameObject lastTrigger = null;
    private bool homeBehaviourAdded;


    void Start()
    {
        // Timers
        lifeTime = new Stopwatch();
        takeHealthTimer = new Stopwatch();
        lifeTime.Start();
        takeHealthTimer.Start();

        // Setting up the startup state
        state = new AgentState
        {
            hydrationLevel = 35,
            foodSupply = 50,
            happy = false,
            isAtDisco = false
        };

        // Creating the default strategy behaviours
        walkAround = new WalkAroundBehaviour(GetComponent<NavMeshAgent>());
        eatFood = new EatNearbyFoodBehaviour(GetComponent<NavMeshAgent>(), transform, state);
        findWater = new GetWaterBehaviour(GetComponent<NavMeshAgent>(), transform, state);
        awayFromMonster = new RunAwayFromMonster(GetComponent<NavMeshAgent>(), transform);
        List<AgentBehaviour> behaviours = new List<AgentBehaviour>
        {
            walkAround,
            eatFood,
            findWater,
            awayFromMonster
        };
        homeAndRest = new GetHomeAndRestBehaviour(GetComponent<NavMeshAgent>(), restingPlace, state); // Added later to the default strategy

        defaultStrategy = new BehaviourStrategy(behaviours, (state) => { return true; });

        ControlCenter = new ControlCenter(gameObject, defaultStrategy);
        ControlCenter.SetStateObject(state);

        // Making additional strategies
        IStrategyManager strategyManager = ControlCenter.StrategyManager;
        strategyManager.EnableMultipleStrategies = true;

        AgentBehaviour goToDisco = new GoToDiscoBehaviour(GetComponent<NavMeshAgent>(), disco);
        AgentBehaviour dance = new DanceBehaviour(ControlCenter.CommunicationManager, GetComponent<NavMeshAgent>(), gameObject, state);
        AgentBehaviour goHomeAndSleep = new GetHomeAndSleepBehaviour(GetComponent<NavMeshAgent>(), restingPlace, state);
        
        List<AgentBehaviour> nightTimeStrategy = new List<AgentBehaviour>
        {
            goToDisco,
            dance,
            goHomeAndSleep
        };
        BehaviourStrategy strategy = new BehaviourStrategy(nightTimeStrategy, (state) =>
        {
            if (GameManager.isNight)
                return true;
            else
                return false;
        });

        strategyManager.AddAdditionalStrategy(strategy);

        // Setting up the communication manager
        ICommunicationManager communicationManager = ControlCenter.CommunicationManager;
        communicationManager.EnableBroadcasting = true;
        BroadcastSettings settings = new BroadcastSettings
        {
            AgentTag = "Agent",
            BroadcastRadius = 20
        };
        settings.SetSuppressibleBehaviours(new List<AgentBehaviour> { dance, goToDisco });
        communicationManager.SetBroadcastSettings(settings);

        // Add arbitration rules
        ControlCenter.ArbitrationManager.EnableArbitrationRules = true;
        ControlCenter.ArbitrationManager.AddArbitrationRule((inputBehaviors) =>
        {
            if (inputBehaviors.Contains(findWater) && inputBehaviors.Contains(awayFromMonster))
            {
                List<AgentBehaviour> toExecute = new List<AgentBehaviour>
                {
                    findWater
                };
                return new ArbitrationRule(toExecute, true);
            }
            return default;
        });
        // Add switch rule
        ControlCenter.StrategyManager.EnableBehaviourSwitcing = true;
        defaultStrategy.AddSwitchRule((inputBehaviors) =>
        {
            if (!inputBehaviors.Contains(eatFood) || !inputBehaviors.Contains(awayFromMonster))
            {
                return default; // If the list no longer contains these behaviours - the rule should return default;
            }

            if (state.isBrave)
            {
                return new SwitchBehaviourRule(eatFood, awayFromMonster, true); // Switch order and delete the SwitchRule afterwards (indicated by the boolean)
            }

            return default;
        });

    }

    void Update()
    {
        if (!state.isBrave)
        {
            var monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (var monster in monsters)
            {
                double distance = Vector3.Distance(transform.position, monster.transform.position);
                if (distance < 4)
                {
                    state.isBrave = true;
                    break;
                }
            }
        }

        if (takeHealthTimer.ElapsedMilliseconds >= (15 * 1000)) // each fifteen seconds
        {
            takeHealthTimer.Restart();
            if (state.foodSupply > 0)
                state.foodSupply--;
            if (state.hydrationLevel > 0)
                state.hydrationLevel -= 5;
        }
        if (lifeTime.ElapsedMilliseconds > 30 * 1000 && !homeBehaviourAdded)
        {
            homeBehaviourAdded = true;
            // When the agent has lived 30 seconds it will get a new behaviour - the ability to go home and rest
            defaultStrategy.AddBehaviourAfterSpecificBehaviour(homeAndRest, walkAround);
        }

        ControlCenter.Update();
    }

    private void FixedUpdate()
    {
        ControlCenter.FixedUpdate();
    }

    private void LateUpdate()
    {
        ControlCenter.LateUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (lastTrigger != null && lastTrigger == other.gameObject)
            return;

        lastTrigger = other.gameObject;
        if (other.gameObject.tag == "Food")
        {
            state.foodSupply++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Water")
        {
            state.hydrationLevel = 100;
            Destroy(other.gameObject);
        }


    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Disco")
        {
            state.isAtDisco = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Disco")
        {
            state.isAtDisco = false;
        }
    }

    public ICommunicationManager GetCommunicationManager()
    {
        return ControlCenter.CommunicationManager;
    }
}
