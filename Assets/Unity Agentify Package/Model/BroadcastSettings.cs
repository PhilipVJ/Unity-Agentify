using System;
using System.Collections.Generic;

namespace Assets.Unity_Agentify_Package.Model
{
    public class BroadcastSettings
    {
        public float BroadcastRadius { get; set; }
        public String AgentTag { get; set; }

        public List<AgentBehaviour> SuppressibleBehaviours { get; private set; }


        // Set which behaviours that can be suppressed by a received broadcast behaviour
        public void SetSuppressibleBehaviours(List<AgentBehaviour> behaviours)
        {
            SuppressibleBehaviours = behaviours;
        }

    }
}
