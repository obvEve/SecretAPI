namespace SecretAPI.CodeGeneration.Utils;

internal static class MethodUtils
{
    internal static StatementSyntax MethodCallStatement(string typeName, string methodName) =>
        MethodCallStatement(ParseTypeName(typeName), IdentifierName(methodName));
    
    internal static StatementSyntax MethodCallStatement(TypeSyntax type, IdentifierNameSyntax method)
        => ExpressionStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    type, method)));

    internal static StatementSyntax[] MethodCallStatements(IMethodSymbol[] methodCalls)
    {
        IEnumerable<StatementSyntax> statements = methodCalls.Select(s => MethodCallStatement(s.ContainingType.ToDisplayString(), s.Name));
        return statements.ToArray();
    }
}