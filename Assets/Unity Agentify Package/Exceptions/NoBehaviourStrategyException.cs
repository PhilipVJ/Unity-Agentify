using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoBehaviourStrategyException : Exception
{
    public NoBehaviourStrategyException(string message) : base(message)
    {
    }

}