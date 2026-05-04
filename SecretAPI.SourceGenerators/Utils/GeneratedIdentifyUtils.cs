namespace SecretAPI.SourceGenerators.Utils;

internal static class GeneratedIdentifyUtils
{
    private static SyntaxToken CurrentVersion => Literal(typeof(GeneratedIdentifyUtils).Assembly.GetName().Version.ToString());
    
    private static AttributeSyntax GetGeneratedCodeAttributeSyntax()
        => Attribute(IdentifierName("GeneratedCode"))
            .WithArgumentList(
                AttributeArgumentList(
                    SeparatedList<AttributeArgumentSyntax>(
                        new SyntaxNodeOrToken[]
                        {
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal("SecretAPI.SourceGenerators"))),
                            Token(SyntaxKind.CommaToken),
                            AttributeArgument(LiteralExpression(SyntaxKind.StringLiteralExpression, CurrentVersion)),
                        })));

    internal static AttributeListSyntax GetGeneratedCodeAttributeListSyntax()
        => AttributeList(SingletonSeparatedList(GetGeneratedCodeAttributeSyntax()));
}