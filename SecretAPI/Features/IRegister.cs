namespace SecretAPI.Features;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/// <summary>
/// Interface used to define a type that should auto register.
/// </summary>
/// <remarks>This supports <see cref="IPriority"/>, will default to 0 if not implemented.</remarks>
// TODO: Source generate this
public interface IRegister
{
    /// <summary>
    /// A list of all registered items for an assembly.
    /// </summary>
    private static Dictionary<Assembly, List<IRegister>> registerables = new();

    /// <summary>
    /// Attempts to register the object.
    /// </summary>
    public void TryRegister();

    /// <summary>
    /// Attempts to unregister the object.
    /// </summary>
    public void TryUnregister()
    {
        // default to empty
    }

    /// <summary>
    /// Registers all <see cref="IRegister"/>.
    /// </summary>
    /// <param name="assembly">The assembly to register from.</param>
    public static void RegisterAll(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        registerables.TryAdd(assembly, new());

        List<IRegister> registers = new();
        foreach (Type type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
                continue;

            if (!typeof(IRegister).IsAssignableFrom(type))
                continue;

            if (Activator.CreateInstance(type) is not IRegister register)
                continue;

            registers.Add(register);
        }

        foreach (IRegister register in registers.OrderBy(x => x is IPriority priority ? priority.Priority : 0))
        {
            registerables[assembly].Add(register);
            register.TryRegister();
        }
    }

    /// <summary>
    /// Unregisters all <see cref="IRegister"/> from an <see cref="Assembly"/>.
    /// </summary>
    /// <param name="assembly">The assembly to unregister from.</param>
    public static void UnRegisterAll(Assembly? assembly = null)
    {
        assembly ??= Assembly.GetCallingAssembly();

        foreach (IRegister register in registerables[assembly])
            register.TryUnregister();

        registerables.Remove(assembly);
    }
}