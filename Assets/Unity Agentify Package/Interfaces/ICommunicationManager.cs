using Assets.Unity_Agentify_Package.Model;
using System.Collections.Generic;

namespace Assets.Unity_Agentify_Package.Managers
{
    public interface ICommunicationManager
    {
        BroadcastSettings BroadcastSettings { get; }
        bool EnableBroadcasting { get; set; }

        bool Broadcast(BroadcastMessage message);
        void CheckPendingMessage();
        AgentBehaviour GetBehaviourToDo(List<AgentBehaviour> arbitratedBehaviours);
        void ReceiveMessage(BroadcastMessage message);
        void SetBroadcastSettings(BroadcastSettings settings);
    }
}