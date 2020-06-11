using Assets.Unity_Agentify_Package.Behaviour;
using Assets.Unity_Agentify_Package.Managers;
using Assets.Unity_Agentify_Package.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scenes.PackageExample.Behaviours
{
    public class DanceBehaviour : BroadcastingBehaviour
    {
        private readonly GameObject self;
        private readonly AgentState state;
        private readonly NavMeshAgent agent;
      


        public DanceBehaviour(ICommunicationManager communicationManager, NavMeshAgent agent, GameObject self, AgentState state) : base(communicationManager)
        {
            this.self = self;
            this.state = state;
            this.agent = agent;
        }

        public override BroadcastMessage MessageToBroadcast()
        {
            List<BroadcastedBehaviour> behavioursToDo = new List<BroadcastedBehaviour>
                {
                    new DanceWithAgentBehaviour(self)
                };
            return new BroadcastMessage(behavioursToDo, 1);
        }

        public override void OnAcceptedUpdate()
        {
            var agent = GetMessage().GetAcceptedAgents()[0];
            double distance = Vector3.Distance(self.transform.position, agent.transform.position);
            if (distance < 3)
            {
                self.transform.Rotate(new Vector3 { x = 0, y = 2, z = 0 }, Space.World);
            }
        }


        public override bool TakeControl()
        {
            if (state.isAtDisco && agent.velocity == Vector3.zero)
            {
                return true;
            }
            return false;
        }

    }
}
