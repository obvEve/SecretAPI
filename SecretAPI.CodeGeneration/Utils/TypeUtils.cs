namespace SecretAPI.CodeGeneration.Utils;

internal static class TypeUtils
{
    internal static PredefinedTypeSyntax GetPredefinedTypeSyntax(SyntaxKind kind)
        => PredefinedType(Token(kind));

    internal static TypeSyntax GetTypeSyntax(string typeIdentifier)
        => IdentifierName(typeIdentifier);
}