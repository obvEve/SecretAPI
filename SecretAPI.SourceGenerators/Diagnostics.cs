namespace SecretAPI.SourceGenerators;

internal static class Diagnostics
{
    internal static readonly DiagnosticDescriptor MustBePartialPluginClass = new(
        "SecretGen0",
        "Plugin class must be partial",
        "Plugin class '{0}' is missing partial modifier",
        "Usage",
        DiagnosticSeverity.Error,
        true);

    internal static readonly DiagnosticDescriptor MustBeAccessibleMethod = new(
        "SecretGen1",
        "Method must be accessible",
        "Method '{0}' has accessibility '{1}', which is not supported for generated calls",
        "Usage",
        DiagnosticSeverity.Error,
        true);

    internal static readonly DiagnosticDescriptor MustBeStaticMethod = new(
        "SecretGen2",
        "Method must be static",
        "Method '{0}' is not marked as static",
        "Usage",
        DiagnosticSeverity.Error,
        true);
}