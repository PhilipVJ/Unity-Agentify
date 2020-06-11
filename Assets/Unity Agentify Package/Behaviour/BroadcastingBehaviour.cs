using Assets.Unity_Agentify_Package.Managers;
using Assets.Unity_Agentify_Package.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Unity_Agentify_Package.Behaviour
{
    public abstract class BroadcastingBehaviour : AgentBehaviour
    {
        private BroadcastMessage message;
        private readonly ICommunicationManager communicationManager;

        public BroadcastingBehaviour(ICommunicationManager communicationManager)
        {
            this.communicationManager = communicationManager;
        }

        public BroadcastMessage GetMessage()
        {
            return message;
        }

        public abstract BroadcastMessage MessageToBroadcast();

        public override void Suppress()
        {
            if (message != null)
            {
                message.Cancel();
                message = null;
            }
        }

        private void Broadcast()
        {
            message = MessageToBroadcast();
            if (!communicationManager.Broadcast(message))
            {
                message = null; // Try again next frame
            }
        }

        public override void Update()
        {
            if (message == null)
            {
                Broadcast();
                return;
            }

            if (message.IsAccepted())
            {
                OnAcceptedUpdate();
            }
            else if (message.AllHaveResponded() && !message.IsAccepted() || message.IsCancelled())
            {
                // Not all accepted the broadcast - or it has been cancelled - trying again
                message = null;
            }
        }

        public override void FixedUpdate()
        {
            if (message != null && message.IsAccepted())
            {
                OnAcceptedFixedUpdate();
            }
        }

        public override void LateUpdate()
        {
            if (message != null && message.IsAccepted())
            {
                OnAcceptedLateUpdate();
            }
        }

        public virtual void OnAcceptedUpdate() { }
        public virtual void OnAcceptedFixedUpdate() { }
        public virtual void OnAcceptedLateUpdate() { }


    }
}
