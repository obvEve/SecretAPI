namespace SecretAPI.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;

/// <summary>
/// Extensions for reflection.
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>
    /// Casts an object into <typeparamref name="T"/>.
    /// This will throw an exception if <paramref name="source"/> is not of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="source">The source object to cast from.</param>
    /// <typeparam name="T">The new type to cast to.</typeparam>
    /// <returns>The source after being cast to T.</returns>
    public static T Cast<T>(this object source)
        where T : class => (T)source;

    /// <summary>
    /// Casts an object of <typeparamref name="T1"/> to <typeparamref name="T2"/>.
    /// This will require <typeparamref name="T2"/> to be derived from <typeparamref name="T1"/>.
    /// </summary>
    /// <param name="source">The source to cast from.</param>
    /// <typeparam name="T1">The original type.</typeparam>
    /// <typeparam name="T2">The type to cast to.</typeparam>
    /// <returns>The source after being cast to <typeparamref name="T2"/>.</returns>
    public static T2 CastTypeSafely<T1, T2>(this T1 source)
        where T1 : class
        where T2 : T1 => (T2)source;

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
    /// Searches through an assembly and returns all <see cref="MethodInfo"/> based on the provided flags.
    /// </summary>
    /// <param name="assembly">The assembly to search through.</param>
    /// <param name="flags">The <see cref="BindingFlags"/> used for the method search within a <see cref="Type"/>.</param>
    /// <returns>A collection of <see cref="MethodInfo"/> based on the provided assembly and flags.</returns>
    public static IEnumerable<MethodInfo> GetMethods(this Assembly assembly, BindingFlags flags)
        => assembly.GetTypes().SelectMany(type => type.GetMethods(flags));

    /// <summary>
    /// Gets a nested <see cref="MethodInfo"/> within a <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> containing the needed method.</param>
    /// <param name="nestedTypeName">The name of the nested type, this does not need to be exact.</param>
    /// <param name="methodName">The name of the method, this does not need to be exact.</param>
    /// <returns>The found method, or null if none matched.</returns>
    public static MethodInfo? GetNestedMethod(this Type type, string nestedTypeName, string methodName)
    {
        return type.GetNestedTypes(AccessTools.all)
            .Where(t => t.Name.Contains(nestedTypeName))
            .SelectMany(t => t.GetMethods(AccessTools.all))
            .FirstOrDefault(m => m.Name.Contains(methodName));
    }

    /// <summary>
    /// Copies the properties from a <see cref="Type"/> onto another instance.
    /// </summary>
    /// <param name="source">The source of the properties to copy.</param>
    /// <param name="destination">Where the source properties should be copied to, this should match the same <see cref="Type"/> as source.</param>
    public static void CopyPropertiesTo(this object source, object destination)
    {
        Type type = source.GetType();

        if (type != destination.GetType())
            throw new InvalidOperationException($"[ReflectionExtensions.CopyPropertiesTo] Source and destination types are mismatched: {type.FullName} | {destination.GetType().FullName}");

        foreach (PropertyInfo property in type.GetProperties())
            property.SetValue(destination, property.GetValue(source));
    }
}