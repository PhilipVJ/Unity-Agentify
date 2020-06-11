using Assets.Unity_Agentify_Package;
using Assets.Unity_Agentify_Package.Managers;
using System.Collections.Generic;
using UnityEngine;

public class ControlCenter
{
    // Internal variables
    private int lastArbitratedFrame;
    public object stateObject;
    public List<AgentBehaviour> toExecute;
    private BehaviourStrategy strategyToUse;

    // Managers
    public IStrategyManager StrategyManager { get; private set; }
    public IArbitrationManager ArbitrationManager { get; private set; }

    public ICommunicationManager CommunicationManager { get; private set; }


    public ControlCenter(GameObject self, BehaviourStrategy defaultStrategy)
    {
        InitializeControlCenter(self, defaultStrategy);
    }

    public void SetStateObject(object stateObject)
    {
        this.stateObject = stateObject;
        StrategyManager.StateObject = stateObject;
    }


    private void InitializeControlCenter(GameObject self, BehaviourStrategy defaultStrategy)
    {
        StrategyManager = new StrategyManager(defaultStrategy)
        {
            EnableBehaviourSwitcing = false,
            EnableMultipleStrategies = false
        };

        ArbitrationManager = new ArbitrationManager
        {
            EnableArbitrationRules = false,
        };

        CommunicationManager = new CommunicationManager(self)
        {
            EnableBroadcasting = false
        };
    }

    /* This method will be called before Update, FixedUpdate and LateUpdate
     * This makes sure that the first method which gets called each frame (which can vary)
     * forces the controlcenter to calculate which behaviour(s) to execute exactly one time
     * each frame
     */
    public void PreExecuteCalculations()
    {
        if (lastArbitratedFrame != Time.frameCount)
        {
            strategyToUse = StrategyManager.GetStrategy(); // Get the strategy to use
            toExecute = ArbitrationManager.Arbitrate(strategyToUse.GetBehaviours()); // Get the behaviour(s) to execute
            if (CommunicationManager.EnableBroadcasting)
            {
                AgentBehaviour broadcastedBehaviourToDo = CommunicationManager.GetBehaviourToDo(toExecute);
                if(broadcastedBehaviourToDo != null)
                {
                    toExecute.Clear();
                    toExecute.Add(broadcastedBehaviourToDo);
                }
            }
            SuppressBehaviours();
            lastArbitratedFrame = Time.frameCount;
        }
    }


    /* This method calls the suppress method on all behaviours which isn't going to be executed. 
     * This can be used to stop existing coroutines
     */
    public void SuppressBehaviours()
    {
        foreach (var behaviour in strategyToUse.GetBehaviours())
        {
            if (!toExecute.Contains(behaviour))
            {
                behaviour.Suppress();
            }
        }
    }

    #region Bindings to MonoBehaviour
    public void Update()
    {
        PreExecuteCalculations();
        if (toExecute.Count > 0)
        {
            foreach (var behaviour in toExecute)
            {
                behaviour.Update();
            }
        }
    }

    public void FixedUpdate()
    {
        PreExecuteCalculations();
        if (toExecute.Count > 0)
        {
            foreach (var behaviour in toExecute)
            {
                behaviour.FixedUpdate();
            }
        }
    }

    public void LateUpdate()
    {
        PreExecuteCalculations();
        if (toExecute.Count > 0)
        {
            foreach (var behaviour in toExecute)
            {
                behaviour.LateUpdate();
            }
        }
    }

    #endregion


}
