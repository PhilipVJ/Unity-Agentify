using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scenes.PackageExample.Behaviours
{
    public class MigrateBehaviour : AgentBehaviour
    {
        private readonly NavMeshAgent agent;
        private readonly GameObject palm;
        private readonly Stopwatch timer;
        private readonly Stopwatch timeOnIsland;
        private bool flyingToIsland;
        private bool reachedIsland;

        public MigrateBehaviour(NavMeshAgent agent, GameObject palm)
        {
            this.agent = agent;
            this.palm = palm;
            timer = new Stopwatch();
            timeOnIsland = new Stopwatch();
            timer.Start();
        }

        public override void Suppress()
        {
            flyingToIsland = false;
            reachedIsland = false;
            timeOnIsland.Reset();
        }

        public override bool TakeControl()
        {
            if (timer.ElapsedMilliseconds > 60 * 1000)
                return true;
            else
                return false;
        }
        public override void Update()
        {

            if (!flyingToIsland)
            {
                flyingToIsland = true;
                int randomX = Random.Range(-1, 2);
                int randomZ = Random.Range(-1, 2);
                Vector3 position = palm.transform.position;
                position.x += randomX;
                position.z += randomZ;
                agent.SetDestination(position);
            }

            if (agent.remainingDistance < 0.2f && !agent.pathPending && flyingToIsland && !reachedIsland)
            {
                agent.ResetPath();
                reachedIsland = true;
                timeOnIsland.Start();
            }
            if(reachedIsland && timeOnIsland.ElapsedMilliseconds>10 * 1000)
            {
                timeOnIsland.Stop();
                timer.Restart();
            }


        }
    }
}
