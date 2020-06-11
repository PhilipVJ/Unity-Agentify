using System;
using System.Runtime.Serialization;

[Serializable]
internal class NothingWantsControlException : Exception
{
    public NothingWantsControlException(string message) : base(message)
    {
    }

}