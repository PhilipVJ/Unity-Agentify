using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Unity_Agentify_Package.Behaviour
{
    public abstract class BroadcastedBehaviour : AgentBehaviour
    {
        public GameObject Recipient { get; set; }
    }
}
