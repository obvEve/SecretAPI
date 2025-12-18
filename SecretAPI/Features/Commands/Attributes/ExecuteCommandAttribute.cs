namespace SecretAPI.Features.Commands.Attributes
{
    using System;

    /// <summary>
    /// Attribute used to identify a method as a possible execution result.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteCommandAttribute : Attribute
    {
    }
}