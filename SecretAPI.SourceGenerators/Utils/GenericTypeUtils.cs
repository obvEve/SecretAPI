namespace SecretAPI.SourceGenerators.Utils;

internal static class GenericTypeUtils
{
    internal static TypeSyntax GetSingleGenericTypeSyntax(string genericName, SyntaxKind predefinedType)
        => GenericName(genericName)
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList<TypeSyntax>(
                        PredefinedType(
                            Token(predefinedType)))));
}