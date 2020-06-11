using System;
using System.Collections.Generic;

public interface IArbitrationManager
{
    bool EnableArbitrationRules { get; set; }

    void AddArbitrationRule(Func<List<AgentBehaviour>, ArbitrationRule> ruleFunc);
    List<AgentBehaviour> Arbitrate(List<AgentBehaviour> strategyBehaviours);
    void DeleteArbitrationRule(Func<List<AgentBehaviour>, ArbitrationRule> ruleFunc);
    void ResetVariableReferences();
}