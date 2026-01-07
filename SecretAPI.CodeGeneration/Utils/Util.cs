namespace SecretAPI.CodeGeneration.Utils;

internal static class Util
{
    private static AttributeSyntax GetGeneratedCodeAttributeSyntax()
        => Attribute(IdentifierName("GeneratedCode"))
            .WithArgumentList(
                AttributeArgumentList(
                    SeparatedList<AttributeArgumentSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("SecretAPI.CodeGeneration"))),
                            Token(SyntaxKind.CommaToken),
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("1.0.0"))),
                        })));
    
    internal static AttributeListSyntax GetGeneratedCodeAttributeListSyntax()
        => AttributeList(SingletonSeparatedList(GetGeneratedCodeAttributeSyntax()));
    
    internal static TypeSyntax GetSingleGenericTypeSyntax(string genericName, SyntaxKind predefinedType)
        => GenericName(genericName)
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList<TypeSyntax>(
                        PredefinedType(
                            Token(predefinedType)))));
    
    internal static PredefinedTypeSyntax GetPredefinedTypeSyntax(SyntaxKind kind)
        => PredefinedType(Token(kind));

    internal static StatementSyntax MethodCallStatement(string typeName, string methodName)
        => ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    ParseTypeName(typeName), IdentifierName(methodName))));

    internal static StatementSyntax[] MethodCallStatements(IMethodSymbol[] methodCalls)
    {
        List<StatementSyntax> statements = new();
        
        foreach (IMethodSymbol method in methodCalls)
            statements.Add(MethodCallStatement(method.ContainingType.ToDisplayString(), method.Name));

        return statements.ToArray();
    }
}