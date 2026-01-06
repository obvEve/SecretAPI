namespace SecretAPI.Features.Commands.Attributes
{
    using System;
    using SecretAPI.Features.Commands.Validators;

    /// <summary>
    /// Defines the attribute needed to auto validate with <see cref="ICommandArgumentValidator{T}"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ValidateArgumentAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateArgumentAttribute"/> class.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the <see cref="ICommandArgumentValidator{T}"/>.</param>
        public ValidateArgumentAttribute(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> of the <see cref="ICommandArgumentValidator{T}"/>.
        /// </summary>
        public Type Type { get; }
    }
}