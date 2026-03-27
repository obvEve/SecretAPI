namespace SecretAPI.CodeGeneration.Diagnostics;

internal static class CallLoadDiagnostics
{
    internal static readonly DiagnosticDescriptor MustBePartialPluginClass = new(
        "SecretGen1",
        "Plugin class must be partial",
        "Plugin class '{0}' is missing partial modifier",
        "Usage",
        DiagnosticSeverity.Error,
        true);
}