namespace SecretAPI.SourceGenerators.Utils;

internal static class GeneratedIdentifyUtils
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
}