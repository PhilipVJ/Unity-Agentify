
namespace Assets.Unity_Agentify_Package
{
    public class SwitchBehaviourRule
    {
        private readonly AgentBehaviour behaviourOne;
        private readonly AgentBehaviour behaviourTwo;
        private readonly bool deleteAtExecute;

        public SwitchBehaviourRule(AgentBehaviour behaviourOne, AgentBehaviour behaviourTwo, bool deleteAtExecute)
        {
            this.behaviourOne = behaviourOne;
            this.behaviourTwo = behaviourTwo;
            this.deleteAtExecute = deleteAtExecute;
        }


        public AgentBehaviour GetBehaviourOne()
        {
            return behaviourOne;
        }

        public bool DeleteAtExecute()
        {
            return deleteAtExecute;
        }

        public AgentBehaviour GetBehaviourTwo()
        {
            return behaviourTwo;
        }

    }
}
