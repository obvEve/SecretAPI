namespace SecretAPI.CodeGeneration.Utils;

public static class Util
{
    public static TypeSyntax GetSingleGenericTypeSyntax(string genericName, SyntaxKind predefinedType)
        => GenericName(genericName)
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList<TypeSyntax>(
                        PredefinedType(
                            Token(predefinedType)))));
    
    public static PredefinedTypeSyntax GetPredefinedTypeSyntax(SyntaxKind kind)
        => PredefinedType(Token(kind));
}