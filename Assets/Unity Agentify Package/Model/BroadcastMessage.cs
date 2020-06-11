using Assets.Unity_Agentify_Package.Behaviour;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Unity_Agentify_Package.Model
{
    public class BroadcastMessage
    {
        private List<BroadcastedBehaviour> behaviourToDo;
        private int acceptances = 0;
        private int responses = 0;
        private int sendTo;
        private bool isCancelled = false;

        private List<GameObject> acceptedAgents;


        public BroadcastMessage(List<BroadcastedBehaviour> behaviourToDo, int sendTo)
        {
            this.behaviourToDo = behaviourToDo;
            this.sendTo = sendTo;
            acceptedAgents = new List<GameObject>();

        }

        public BroadcastedBehaviour GetBehaviourToDo(GameObject agent)
        {
            return behaviourToDo[acceptedAgents.IndexOf(agent)];
        }

        public void Cancel()
        {
            isCancelled = true;
        }

        public bool IsCancelled()
        {
            return isCancelled;
        }

        public void AcceptMessage(GameObject agentToAccept)
        {
            acceptedAgents.Add(agentToAccept);
            acceptances++;
            responses++;
        }

        public void DeclineMessage()
        {
            responses++;
        }

        public bool AllHaveResponded()
        {
            return responses == sendTo;
        }

        public List<GameObject> GetAcceptedAgents()
        {
            return acceptedAgents;
        }

        public bool IsAccepted()
        {
            if (isCancelled) return false;
            return acceptances == sendTo;
        }

        public int GetTotalRecipients()
        {
            return sendTo;
        }
    }
}
