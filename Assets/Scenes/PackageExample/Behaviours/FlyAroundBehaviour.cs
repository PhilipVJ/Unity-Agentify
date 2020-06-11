using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scenes.PackageExample.Behaviours
{
    public class FlyAroundBehaviour : AgentBehaviour
    {
        private readonly NavMeshAgent agent;
        private readonly GameObject tree;
        private readonly GameObject tree2;
        private readonly GameObject tree3;
        private readonly GameObject tree4;
        private bool flying;
        private GameObject goingTo;
        private readonly Stopwatch timer;
        private int lastTree;

        public FlyAroundBehaviour(NavMeshAgent agent, GameObject tree, GameObject tree2, GameObject tree3, GameObject tree4)
        {
            this.agent = agent;
            this.tree = tree;
            this.tree2 = tree2;
            this.tree3 = tree3;
            this.tree4 = tree4;

            timer = new Stopwatch();
            timer.Start();
        }

        public override bool TakeControl()
        {
            return true;
        }
        public override void Update()
        {

            if (!flying && timer.ElapsedMilliseconds > 5 * 1000)
            {
                flying = true;
                bool foundValidTree = false;
                int treeNumber = -1; // Placeholder
                while (!foundValidTree)
                {
                    treeNumber = Random.Range(0, 4);
                    if (treeNumber != lastTree)
                    {
                        foundValidTree = true;
                    }
                }
                switch (treeNumber)
                {
                    case 0:
                        goingTo = tree;
                        break;
                    case 1:
                        goingTo = tree2;
                        break;
                    case 2:
                        goingTo = tree3;
                        break;
                    case 3:
                        goingTo = tree4;
                        break;
                    default:
                        break;
                }
                lastTree = treeNumber;
                agent.SetDestination(goingTo.transform.position);
            }

            if (agent.remainingDistance < 0.3f && !agent.pathPending && flying)
            {
                agent.ResetPath();
                timer.Restart();
                flying = false;
            }


        }
    }
}
