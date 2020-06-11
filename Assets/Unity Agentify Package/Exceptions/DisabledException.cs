using System;
using System.Runtime.Serialization;

[Serializable]
internal class DisabledException : Exception
{
    public DisabledException(string message) : base(message)
    {
    }

}