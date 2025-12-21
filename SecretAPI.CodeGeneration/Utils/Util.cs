namespace SecretAPI.CodeGeneration.Utils;

public static class Util
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
    
    public static TypeSyntax GetSingleGenericTypeSyntax(string genericName, SyntaxKind predefinedType)
        => GenericName(genericName)
            .WithTypeArgumentList(
                TypeArgumentList(
                    SingletonSeparatedList<TypeSyntax>(
                        PredefinedType(
                            Token(predefinedType)))));
    
    public static PredefinedTypeSyntax GetPredefinedTypeSyntax(SyntaxKind kind)
        => PredefinedType(Token(kind));

    public static StatementSyntax MethodCallStatement(string typeName, string methodName)
        => ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    ParseTypeName(typeName), IdentifierName(methodName))));

    public static StatementSyntax[] MethodCallStatements(IMethodSymbol[] methodCalls)
    {
        List<StatementSyntax> statements = new();
        
        foreach (IMethodSymbol methodCall in methodCalls)
            statements.Add(MethodCallStatement(methodCall.ContainingType.ToDisplayString(), methodCall.Name));

        return statements.ToArray();
    }
}