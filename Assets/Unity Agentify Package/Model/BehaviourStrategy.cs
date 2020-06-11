using System;
using System.Collections.Generic;

namespace Assets.Unity_Agentify_Package
{
    public class BehaviourStrategy
    {
        private readonly List<AgentBehaviour> behaviours;
        private Action<object> switchAction;
        private readonly Func<object, bool> activationCondition;
        private readonly List<Func<List<AgentBehaviour>, SwitchBehaviourRule>> switchRules;

        public BehaviourStrategy(List<AgentBehaviour> behaviours, Func<object, bool> activationCondition)
        {
            this.behaviours = behaviours;
            this.activationCondition = activationCondition;
            switchRules = new List<Func<List<AgentBehaviour>, SwitchBehaviourRule>>();
        }
        public List<AgentBehaviour> GetBehaviours()
        {
            return behaviours;
        }

        public void AddSwitchAction(Action<object> switchAction)
        {
            this.switchAction = switchAction;
        }

        public Action<object> GetSwitchAction()
        {
            return switchAction;
        }

        public Func<object, bool> GetActivationCondition()
        {
            return activationCondition;
        }

        public void AddBehaviorAsHighestIndexed(AgentBehaviour behaviour)
        {
            if (behaviours.Contains(behaviour))
            {
                throw new DuplicateBehaviourException("The behaviour already exists");
            }
            behaviours.Add(behaviour);
        }

        public void AddBehaviourAfterSpecificBehaviour(AgentBehaviour toInsert, AgentBehaviour afterThis)
        {
            if (!behaviours.Contains(afterThis))
            {
                throw new NoBehaviorException("The behaviour doesn't exist in the default strategy");
            }
            if (behaviours.Contains(toInsert))
            {
                throw new DuplicateBehaviourException("The behaviour already exists");
            }
            int index = behaviours.IndexOf(afterThis);
            behaviours.Insert(index + 1, toInsert);
        }

        public void AddBehaviourBeforeSpecificBehaviour(AgentBehaviour toInsert, AgentBehaviour beforeThis)
        {
            if (!behaviours.Contains(beforeThis))
            {
                throw new NoBehaviorException("The behaviour doesn't exist in the default strategy");
            }
            if (behaviours.Contains(toInsert))
            {
                throw new DuplicateBehaviourException("The behaviour already exists");
            }
            int index = behaviours.IndexOf(beforeThis);
            behaviours.Insert(index, toInsert);
        }

        public void RemoveBehavior(AgentBehaviour behaviour)
        {
            if (!behaviours.Contains(behaviour))
            {
                throw new NoBehaviorException("The behaviour doesn't exist in the default strategy");
            }
            behaviours.Remove(behaviour);
        }

        #region SwitchRule related

        public void AddSwitchRule(Func<List<AgentBehaviour>, SwitchBehaviourRule> switchRule)
        {
            switchRules.Add(switchRule);
        }
        public void DeleteSwitchRule(Func<List<AgentBehaviour>, SwitchBehaviourRule> switchRule)
        {
            if (!switchRules.Contains(switchRule))
                throw new NoSwitchRuleException("The given switch rule to delete doesn't exist in the list");
            else
                switchRules.Remove(switchRule);
        }

        public void ExecuteBehaviourSwitching()
        {
            List<SwitchBehaviourRule> rulesList = new List<SwitchBehaviourRule>();
            List<int> rulesToDelete = new List<int>();
            for (int i = 0; i < switchRules.Count; i++)
            {
                SwitchBehaviourRule switchRule = switchRules[i].Invoke(behaviours);
                if (switchRule != null)
                {
                    rulesList.Add(switchRule);
                    if (switchRule.DeleteAtExecute())
                        rulesToDelete.Add(i);
                    break;
                }
            }

            if (rulesList.Count > 0)
            {
                foreach (var rule in rulesList)
                {
                    SwitchBehaviourOrder(rule.GetBehaviourOne(), rule.GetBehaviourTwo());
                }
                // If any rules demands to be deleted after execution it will be done here
                foreach (var index in rulesToDelete)
                {
                    switchRules.RemoveAt(index);
                }
            }

        }

        private void SwitchBehaviourOrder(AgentBehaviour behaviourOne, AgentBehaviour behaviourTwo)
        {
            if (!behaviours.Contains(behaviourOne) || !behaviours.Contains(behaviourTwo))
                throw new NoBehaviorException("A behaviour is missing from the collection");
            // Getting the index
            int indexOfFirst = behaviours.IndexOf(behaviourOne);
            int indexOfSecond = behaviours.IndexOf(behaviourTwo);
            // Switching order
            behaviours[indexOfFirst] = behaviourTwo;
            behaviours[indexOfSecond] = behaviourOne;
        }

        #endregion

    }
}
