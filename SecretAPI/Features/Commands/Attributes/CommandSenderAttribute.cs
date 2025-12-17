namespace SecretAPI.Features.Commands.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter)]
    public class CommandSenderAttribute : Attribute
    {
    }
}