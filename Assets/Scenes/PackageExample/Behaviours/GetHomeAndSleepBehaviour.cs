using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scenes.PackageExample.Behaviours
{
    public class GetHomeAndSleepBehaviour : AgentBehaviour
    {
        private readonly NavMeshAgent agent;
        private readonly AgentState state;
        private readonly GameObject restingPlace;
        private bool startedWalkHome = false;
        private bool reachedHome = false;


        public GetHomeAndSleepBehaviour(NavMeshAgent agent, GameObject restingPlace, AgentState state)
        {
            this.agent = agent;
            this.restingPlace = restingPlace;
            this.state = state;
        }

        public override bool TakeControl()
        {
            if (state.hydrationLevel < 20 || state.foodSupply < 5)
            {
                return true;
            }
            return false;
        }
        public override void Update()
        {
            if (!startedWalkHome)
            {
                agent.SetDestination(restingPlace.transform.position);
                startedWalkHome = true;
            }

            if (agent.remainingDistance < 0.1f && !agent.pathPending && !reachedHome)
            {
                reachedHome = true;
                agent.ResetPath();
            }
        }

        public override void Suppress()
        {

            reachedHome = false;
            startedWalkHome = false;

        }
    }
}
