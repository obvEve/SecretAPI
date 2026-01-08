namespace SecretAPI.CodeGeneration.Utils;

internal static class TypeUtils
{
    internal static PredefinedTypeSyntax GetPredefinedTypeSyntax(SyntaxKind kind)
        => PredefinedType(Token(kind));
}