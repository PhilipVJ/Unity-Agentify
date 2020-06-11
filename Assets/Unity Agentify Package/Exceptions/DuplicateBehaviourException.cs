using System;
using System.Runtime.Serialization;

[Serializable]
internal class DuplicateBehaviourException : Exception
{
    public DuplicateBehaviourException(string message) : base(message)
    {
    }

}