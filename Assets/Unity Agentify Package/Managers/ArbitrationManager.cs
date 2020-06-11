using System;
using System.Collections.Generic;

public class ArbitrationManager : IArbitrationManager
{
    private AgentBehaviour highestIndexedBehaviourToExecute;
    private List<AgentBehaviour> wantsToTakeControl;
    private List<AgentBehaviour> toExecute;
    private List<Func<List<AgentBehaviour>, ArbitrationRule>> arbitrationRules;
    public bool EnableArbitrationRules { get; set; }

    public ArbitrationManager()
    {
        arbitrationRules = new List<Func<List<AgentBehaviour>, ArbitrationRule>>();
    }

    public void ResetVariableReferences()
    {
        toExecute = new List<AgentBehaviour>(); // Creates an empty list
        wantsToTakeControl = new List<AgentBehaviour>(); // Make a new Array holding all the behaviors which wants to take control
        highestIndexedBehaviourToExecute = null;
    }
    public List<AgentBehaviour> Arbitrate(List<AgentBehaviour> strategyBehaviours)
    {
        if (strategyBehaviours.Count == 0)
        {
            throw new NoBehaviorException("No behaviours in the current strategy");
        }

        ResetVariableReferences();

        foreach (var behaviour in strategyBehaviours)
        {
            if (behaviour.TakeControl())
            {
                wantsToTakeControl.Add(behaviour);
                highestIndexedBehaviourToExecute = behaviour;
            }
        }
        if (highestIndexedBehaviourToExecute == null)
        {
            throw new NothingWantsControlException("No behaviours wants to take control");
        }
        // If ArbitrationRules is enabled we check if a rule returns a ArbitrationRule. If yes - that rule determines the arbitration

        if (EnableArbitrationRules && arbitrationRules.Count > 0)
        {
            ExecuteArbitrationRulesCheck();
        }

        if (toExecute.Count > 0)
        {
            return toExecute;
        }
        else
        {
            List<AgentBehaviour> highestIndexedOnly = new List<AgentBehaviour>();
            highestIndexedOnly.Add(highestIndexedBehaviourToExecute);
            return highestIndexedOnly;
        }
    }

    private void ExecuteArbitrationRulesCheck()
    {
        ArbitrationRule activatedRule = null;
        foreach (var rule in arbitrationRules)
        {
            activatedRule = rule.Invoke(wantsToTakeControl);
            if (activatedRule != null) // Found a valid rule
            {
                break;
            }
        }
        if (activatedRule != null)
        {
            toExecute = activatedRule.GetBehaviours();
            if (!activatedRule.IgnoreHighestIndexed())
            {
                // If the rule says we should also run the highest indexed behavior, we apply that to the list - if it isn't already there
                if (!toExecute.Contains(highestIndexedBehaviourToExecute))
                {
                    toExecute.Add(highestIndexedBehaviourToExecute);
                }
            }
        }
    }

    public void AddArbitrationRule(Func<List<AgentBehaviour>, ArbitrationRule> ruleFunc)
    {
        arbitrationRules.Add(ruleFunc);
    }

    public void DeleteArbitrationRule(Func<List<AgentBehaviour>, ArbitrationRule> ruleFunc)
    {
        if (!arbitrationRules.Contains(ruleFunc))
            throw new NoArbitrationRuleException("The given arbitration rule doesn't exist in the collection");
        else
            arbitrationRules.Remove(ruleFunc);
    }
}
