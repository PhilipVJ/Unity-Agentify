using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scenes.PackageExample.Behaviours
{
    public class GoToDiscoBehaviour : AgentBehaviour
    {
        private readonly NavMeshAgent agent;
        private readonly GameObject disco;
        private bool startedWalking = false;
        private bool reachedDisco = false;

        public GoToDiscoBehaviour(NavMeshAgent agent, GameObject disco)
        {
            this.agent = agent;
            this.disco = disco;
        }

        public override bool TakeControl()
        {
            return true;
        }
        public override void Update()
        {
            if (!startedWalking)
            {
                Vector3 randomPointInDisco = disco.transform.position;
                int randomX = UnityEngine.Random.Range(-8, 9);
                int randomZ = UnityEngine.Random.Range(-8, 9);
                randomPointInDisco.x += randomX;
                randomPointInDisco.z += randomZ; 
                agent.SetDestination(randomPointInDisco);
                startedWalking = true;
            }

            if (agent.remainingDistance < 0.1f && !agent.pathPending && !reachedDisco)
            {
                reachedDisco = true;
                agent.ResetPath();
            }
        }

        public override void Suppress()
        {
            reachedDisco = false;
            startedWalking = false;
        }
    }
}
