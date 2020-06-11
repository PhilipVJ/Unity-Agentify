using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoBehaviorException : Exception
{
    public NoBehaviorException(string message) : base(message)
    {
    }

}