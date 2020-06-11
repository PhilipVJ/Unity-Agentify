using System;
using System.Runtime.Serialization;

[Serializable]
internal class InvalidSettingsException : Exception
{
    public InvalidSettingsException(string message) : base(message)
    {
    }

}