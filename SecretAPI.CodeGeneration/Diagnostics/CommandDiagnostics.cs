namespace SecretAPI.CodeGeneration.Diagnostics;

internal static class CommandDiagnostics
{
    internal static readonly DiagnosticDescriptor InvalidExecuteMethod = new(
        "SecretGen0",
        "Invalid ExecuteCommand method",
        "Method '{0}' marked with [ExecuteCommand] is invalid: {1}",
        "Usage",
        DiagnosticSeverity.Error,
        true);
}