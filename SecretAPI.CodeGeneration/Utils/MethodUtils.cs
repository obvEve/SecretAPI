namespace SecretAPI.CodeGeneration.Utils;

internal static class MethodUtils
{
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