using System;
using System.Collections.Generic;

namespace Assets.Unity_Agentify_Package
{
    /* The strategy manager is used to determine which strategy should be used for arbitration.
     */
    public class StrategyManager : IStrategyManager
    {
        private readonly List<BehaviourStrategy> strategies;

        public object StateObject { get; set; }

        private BehaviourStrategy lastReturnedStrategy;

        // Options
        public bool EnableBehaviourSwitcing { get; set; }
        public bool EnableMultipleStrategies { get; set; }

        public StrategyManager(BehaviourStrategy defaultStrategy)
        {
            // Add default strategy at index 0
            strategies = new List<BehaviourStrategy>
            {
                defaultStrategy
            };
        }


        public void AddAdditionalStrategy(BehaviourStrategy strategy)
        {
            strategies.Add(strategy);
        }

        public void DeleteAdditionStrategy(BehaviourStrategy strategy)
        {
            if (!strategies.Contains(strategy))
            {
                throw new NoStrategyException("The given strategy isn't in the collection");
            }
            else
            {
                strategies.Remove(strategy);
            }
        }

        public List<BehaviourStrategy> GetAllStrategies()
        {
            return strategies;
        }

        public BehaviourStrategy GetStrategy()
        {
            BehaviourStrategy toUse = null;
            if (EnableMultipleStrategies && strategies.Count > 1) // Find the strategy to use
            {
                if (StateObject == null)
                {
                    throw new NoStateException("You need to set a state to use multiple strategies");
                }
                // Check all non-default strategies
                for (int i = 1; i < strategies.Count; i++)
                {
                    if (strategies[i].GetActivationCondition().Invoke(StateObject))
                    {
                        toUse = strategies[i];
                    }
                }
            }

            if (toUse == null)
            {
                toUse = GetDefaultStrategy(); // using the default behaviour strategy
            }

            // If BehaviourSwitching is enabled - we check if something should be switched
            // locally in the current strategy
            if (EnableBehaviourSwitcing)
            {
                toUse.ExecuteBehaviourSwitching();
            }

            if (lastReturnedStrategy != null && lastReturnedStrategy != toUse)
            {
                // Implies a strategy change - call suppress on all former behaviours
                foreach (var behaviour in lastReturnedStrategy.GetBehaviours())
                {
                    behaviour.Suppress(); // Suppressing all previous behaviours in case of running coroutines etc.
                }
                // If an strategy switch action is added - it will be executed here
                Action<object> switchAction = toUse.GetSwitchAction();
                if (switchAction != null)
                {
                    switchAction.Invoke(StateObject);
                }
            }
            lastReturnedStrategy = toUse;
            return toUse;
        }

        public BehaviourStrategy GetDefaultStrategy()
        {
            return strategies[0];
        }
    }
}
