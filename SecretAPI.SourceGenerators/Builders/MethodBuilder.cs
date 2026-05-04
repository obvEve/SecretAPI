namespace SecretAPI.SourceGenerators.Builders;

internal class MethodBuilder : Builder<MethodBuilder>
{
    private readonly ClassBuilder _classBuilder;
    private readonly List<ParameterSyntax> _parameters = new();
    private readonly List<StatementSyntax> _statements = new();
    private readonly string _methodName;
    private readonly TypeSyntax _returnType;
    
    internal MethodBuilder(ClassBuilder classBuilder, string methodName, TypeSyntax returnType)
    {
        _classBuilder = classBuilder;
        _methodName = methodName;
        _returnType = returnType;
    }

    internal MethodBuilder AddStatements(params StatementSyntax[] statements)
    {
        _statements.AddRange(statements);
        return this;
    }

    internal MethodBuilder AddParameters(params MethodParameter[] parameters)
    {
        foreach (MethodParameter parameter in parameters)
            _parameters.Add(parameter.Syntax);

        return this;
    }

    internal ClassBuilder FinishMethodBuild()
    {
        BlockSyntax body = _statements.Any() ? Block(_statements) : Block();

        MethodDeclarationSyntax methodDeclaration = MethodDeclaration(_returnType, _methodName)
            .AddModifiers(_modifiers.ToArray())
            .AddParameterListParameters(_parameters.ToArray())
            .WithBody(body);

        _classBuilder.AddMethodDefinition(methodDeclaration);
        return _classBuilder;
    }
}