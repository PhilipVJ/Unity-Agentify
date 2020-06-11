using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoStateException : Exception
{
    public NoStateException(string message) : base(message)
    {
    }

}