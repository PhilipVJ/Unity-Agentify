using System.Collections.Generic;

public class ArbitrationRule
{
    private readonly List<AgentBehaviour> toExecute;
    private readonly bool ignoreHighestIndexed;

    public ArbitrationRule(List<AgentBehaviour> toExecute, bool ignoreHighestIndexed)
    {
        this.toExecute = toExecute;
        this.ignoreHighestIndexed = ignoreHighestIndexed;
    }

    public List<AgentBehaviour> GetBehaviours()
    {
        return toExecute;
    }

    public bool IgnoreHighestIndexed()
    {
        return ignoreHighestIndexed;
    }

}
