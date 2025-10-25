namespace SecretAPI.Attribute
{
    using System;
    using System.Reflection;
    using SecretAPI.Features;

    /// <summary>
    /// Defines the attribute for methods to call on unload.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CallOnUnloadAttribute : Attribute, IPriority
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallOnUnloadAttribute"/> class.
        /// </summary>
        /// <param name="priority">The priority of the load.</param>
        public CallOnUnloadAttribute(int priority = 0)
        {
            Priority = priority;
        }

        /// <summary>
        /// Gets the priority of the loading.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Unloads and calls all <see cref="CallOnUnloadAttribute"/>.
        /// </summary>
        /// <param name="assembly">The assembly to begin this on. Null will attempt to get calling, but may fail.</param>
        public static void Unload(Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            CallOnLoadAttribute.CallAttributeMethodPriority<CallOnUnloadAttribute>(assembly);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj == this;

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();
    }
}