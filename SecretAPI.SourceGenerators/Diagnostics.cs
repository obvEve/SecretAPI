namespace SecretAPI.SourceGenerators;

internal static class Diagnostics
{
    internal static readonly DiagnosticDescriptor MustBeAccessibleMethod = new(
        "SG002",
        "Method must be accessible",
        "Method '{0}' has accessibility '{1}', which is not supported for generated calls",
        "Usage",
        DiagnosticSeverity.Error,
        true);

    internal static readonly DiagnosticDescriptor MustBeStaticMethod = new(
        "SG003",
        "Method must be static",
        "Method '{0}' is not marked as static",
        "Usage",
        DiagnosticSeverity.Error,
        true);
}