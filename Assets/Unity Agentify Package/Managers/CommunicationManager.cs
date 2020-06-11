using Assets.Unity_Agentify_Package.Behaviour;
using Assets.Unity_Agentify_Package.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Unity_Agentify_Package.Managers
{
    public class CommunicationManager : ICommunicationManager
    {
        private readonly GameObject self;
        private readonly List<BroadcastMessage> messages;
        public BroadcastSettings BroadcastSettings { get; private set; }
        public bool EnableBroadcasting { get; set; }

        private BroadcastMessage acceptedMessage;

        private BroadcastMessage pendingMessage;




        public CommunicationManager(GameObject self)
        {
            this.self = self;
            messages = new List<BroadcastMessage>();
        }

        public void SetBroadcastSettings(BroadcastSettings settings)
        {
            if (settings.BroadcastRadius <= 0)
                throw new InvalidSettingsException("The broadcast radius must be greater than 0");
            if (settings.AgentTag == null || settings.AgentTag.Length == 0)
                throw new InvalidSettingsException("You must supply a proper agent tag");
            BroadcastSettings = settings;
        }

        public void CheckPendingMessage()
        {
            if (pendingMessage != null && pendingMessage.AllHaveResponded() && !pendingMessage.IsAccepted() || pendingMessage != null && pendingMessage.IsCancelled())
            {
                pendingMessage = null;
            }
        }

        public AgentBehaviour GetBehaviourToDo(List<AgentBehaviour> arbitratedBehaviours)
        {
            CheckPendingMessage();
            CheckAcceptedMessage();

            CheckMessages(arbitratedBehaviours);

            if (pendingMessage == null && acceptedMessage != null && acceptedMessage.IsAccepted())
            {
                BroadcastedBehaviour behaviour = acceptedMessage.GetBehaviourToDo(self);
                behaviour.Recipient = self;
                return behaviour;
            }

            return null;
        }

        private void CheckAcceptedMessage()
        {
            if (acceptedMessage != null && acceptedMessage.IsCancelled())
                acceptedMessage = null;
        }

        public bool Broadcast(BroadcastMessage message)
        {
            if (pendingMessage != null)
            {
                return false; // Can't broadcast another message when a message is pending
            }

            if (BroadcastSettings == null || !EnableBroadcasting)
            {
                throw new DisabledException("You must enable Broadcasting");
            }

            GameObject[] agents = GameObject.FindGameObjectsWithTag(BroadcastSettings.AgentTag);
            List<GameObject> qualifiedAgents = new List<GameObject>();
            foreach (var agent in agents)
            {
                if (agent == self)
                    continue;

                double distance = Vector3.Distance(self.transform.position, agent.transform.position);
                MonoBehaviour script = agent.GetComponent<MonoBehaviour>();
                ICommunicationLine connection;
                try
                {
                    connection = (ICommunicationLine)(script);
                }
                catch (InvalidCastException)
                {
                    throw new NoCommunicationLineException("Your main agent script must implement the ICommunicationLine interface");
                }
                if (distance < BroadcastSettings.BroadcastRadius && connection.GetCommunicationManager().EnableBroadcasting)
                {
                    qualifiedAgents.Add(agent);
                }
            }
            bool succesfulBroadcast = false;

            if (qualifiedAgents.Count >= message.GetTotalRecipients())
            {
                succesfulBroadcast = true;
                pendingMessage = message;
                for (int i = 0; i < message.GetTotalRecipients(); i++)
                {
                    // Send message to random agents which are qualified
                    int random = UnityEngine.Random.Range(0, qualifiedAgents.Count);
                    MonoBehaviour script = qualifiedAgents[random].GetComponent<MonoBehaviour>();
                    ICommunicationLine connection;
                    try {
                        connection = (ICommunicationLine)(script);
                    }
                    catch (InvalidCastException)
                    {
                        throw new NoCommunicationLineException("Your main agent script must implement the ICommunicationLine interface");
                    }
                   
                    connection.GetCommunicationManager().ReceiveMessage(message);
                    qualifiedAgents.RemoveAt(random);
                }
            }
            return succesfulBroadcast;
        }
        // This method is called by other agents if the EnableBroadcast property is true
        public void ReceiveMessage(BroadcastMessage message)
        {
            if (BroadcastSettings == null || !EnableBroadcasting)
            {
                throw new DisabledException("You can't supply a message if the agents broadcasting is disabled");
            }
            messages.Add(message);
        }

        private void CheckMessages(List<AgentBehaviour> arbitratedBehaviours)
        {
            if (BroadcastSettings == null || !EnableBroadcasting)
            {
                throw new DisabledException("You can't check messages if the agents broadcasting is disabled");
            }

            // Check if the arbitrated behaviours are suppressible according to the BroadcastSettings
            bool suppressible = true;
            foreach (var behaviour in arbitratedBehaviours)
            {
                if (!BroadcastSettings.SuppressibleBehaviours.Contains(behaviour))
                {
                    suppressible = false;
                }
            }
            // The arbitrated behaviours can't be suppressed by a broadcast. Decline all messages and set the previous accepted one to null and cancel it
            if (!suppressible)
            {
                DeclineAllMessages();
                if (acceptedMessage != null)
                    acceptedMessage.Cancel();
                acceptedMessage = null;
            }
            // If the agent has accepted a message or have one pending - all messages will be declined
            if (acceptedMessage != null || pendingMessage != null)
            {
                DeclineAllMessages();
            }
            else
            {
                if (messages.Count > 0)
                {
                    messages[0].AcceptMessage(self);
                    acceptedMessage = messages[0];
                }
                messages.Clear(); // Clears all the messages 
            }
        }

        private void DeclineAllMessages()
        {
            foreach (var message in messages)
            {
                message.DeclineMessage();
            }
            messages.Clear(); // Clears all the messages 
        }
    }
}
