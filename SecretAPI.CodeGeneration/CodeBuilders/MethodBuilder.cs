namespace SecretAPI.CodeGeneration.CodeBuilders;

internal class MethodBuilder
{
    private readonly ClassBuilder _classBuilder;
    private readonly List<SyntaxToken> _modifiers = new();
    private readonly List<ParameterSyntax> _parameters = new();
    private readonly string _methodName;
    private readonly string _returnType;
    
    internal MethodBuilder(ClassBuilder classBuilder, string methodName, string returnType)
    {
        _classBuilder = classBuilder;
        _methodName = methodName;
        _returnType = returnType;
    }

    internal MethodBuilder AddParameters(params MethodParameter[] parameters)
    {
        foreach (MethodParameter parameter in parameters)
            _parameters.Add(parameter.Syntax);

        return this;
    }
    
    internal MethodBuilder AddModifiers(params SyntaxKind[] modifiers)
    {
        foreach (SyntaxKind token in modifiers)
            _modifiers.Add(Token(token));

        return this;
    }

    internal ClassBuilder FinishMethodBuild()
    {
        MethodDeclarationSyntax methodDeclaration = MethodDeclaration(ParseTypeName(_returnType), _methodName);
        methodDeclaration = methodDeclaration
            .AddModifiers(_modifiers.ToArray())
            .AddParameterListParameters(_parameters.ToArray())
            .WithBody(Block());
        
        _classBuilder.AddMethodDefinition(methodDeclaration);
        return _classBuilder;
    }
}