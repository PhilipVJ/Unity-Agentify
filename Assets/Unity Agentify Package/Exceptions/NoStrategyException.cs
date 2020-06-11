using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoStrategyException : Exception
{
    public NoStrategyException(string message) : base(message)
    {
    }

}