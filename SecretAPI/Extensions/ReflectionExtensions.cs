namespace SecretAPI.Extensions;

using System;
using System.Linq;
using System.Reflection;

/// <summary>
/// Extensions for reflection.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Gets the long name of a function.
    /// </summary>
    /// <param name="type">The type containing the method.</param>
    /// <param name="methodName">The method name.</param>
    /// <returns>The long function name.</returns>
    /// <exception cref="InvalidOperationException">When the method could not be found.</exception>
    public static string GetLongFuncName(Type type, string methodName)
    {
        const BindingFlags methodFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        MethodInfo method = type.GetMethod(methodName, methodFlags) ?? throw new InvalidOperationException($"[ReflectionExtensions.GetLongFuncName] {type.FullName}.{methodName} could not be found.");
        return GetLongFuncName(type, method);
    }

    /// <summary>
    /// Gets the long name of a function.
    /// </summary>
    /// <param name="type">The type containing the method.</param>
    /// <param name="method">The method to use.</param>
    /// <returns>The long function name.</returns>
    public static string GetLongFuncName(Type type, MethodInfo method)
    {
        return $"{method.ReturnType.FullName} {type.FullName}::{method.Name}({string.Join(",", method.GetParameters().Select(x => x.ParameterType.FullName))})";
    }

    /// <summary>
    /// Copies the properties.
    /// </summary>
    /// <param name="source">The source of the properties to copy.</param>
    /// <param name="destination">Where to copy to.</param>
    public static void CopyProperties(this object source, object destination)
    {
        Type destinationType = destination.GetType();
        foreach (PropertyInfo property in source.GetType().GetProperties())
        {
            destinationType.GetProperty(property.Name)?.SetValue(destination, property.GetValue(source));
        }
    }
}