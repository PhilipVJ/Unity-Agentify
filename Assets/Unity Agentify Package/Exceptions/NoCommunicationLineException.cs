using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoCommunicationLineException : Exception
{
    public NoCommunicationLineException(string message) : base(message)
    {
    }

}