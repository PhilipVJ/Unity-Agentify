using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoArbitrationRuleException : Exception
{
    public NoArbitrationRuleException(string message) : base(message)
    {
    }

}