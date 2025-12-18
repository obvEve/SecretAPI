namespace SecretAPI.Features.Commands.Attributes
{
    using System;
    using CommandSystem;
    using LabApi.Features.Wrappers;

    /// <summary>
    /// Defines a parameter as accepting the command sender.
    /// </summary>
    /// <remarks>this must be <see cref="ICommandSender"/>, <see cref="ReferenceHub"/> or <see cref="Player"/>.</remarks>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class CommandSenderAttribute : Attribute
    {
    }
}