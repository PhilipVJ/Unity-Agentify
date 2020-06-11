using Assets.Unity_Agentify_Package.Behaviour;
using Assets.Unity_Agentify_Package.Managers;
using Assets.Unity_Agentify_Package.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scenes.PackageExample.Behaviours
{
    public class DanceWithAgentBehaviour : BroadcastedBehaviour
    {
        private readonly GameObject otherAgent;
        private bool initDancePosition = false;
        public bool reachedDancePosition = false;

        public DanceWithAgentBehaviour(GameObject otherAgent)
        {
            this.otherAgent = otherAgent;
        }



        public override bool TakeControl()
        {
            return true;
        }

        public override void Update()
        {
            NavMeshAgent agent = Recipient.GetComponent<NavMeshAgent>();
            // Go 1 unit in front of the other agent
            if (!initDancePosition)
            {
                initDancePosition = true;
                Vector3 position = otherAgent.transform.position + otherAgent.transform.forward * 2;
                agent.SetDestination(position);
            }
            if (agent.remainingDistance < 0.1f && !agent.pathPending && !reachedDancePosition)
            {
                agent.ResetPath();
                reachedDancePosition = true;
            }
            if (reachedDancePosition)
            {
                Recipient.transform.Rotate(new Vector3 { x = 0, y = -2, z = 0 }, Space.World);
            }
        }

    }
}
