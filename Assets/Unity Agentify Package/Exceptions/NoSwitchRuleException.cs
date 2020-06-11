using System;
using System.Runtime.Serialization;

[Serializable]
internal class NoSwitchRuleException : Exception
{
    public NoSwitchRuleException(string message) : base(message)
    {
    }

}