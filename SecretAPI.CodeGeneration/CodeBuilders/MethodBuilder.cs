namespace SecretAPI.CodeGeneration.CodeBuilders;

using Microsoft.CodeAnalysis.CSharp.Syntax;

internal class MethodBuilder
{
    private readonly ClassBuilder _classBuilder;
    
    internal MethodBuilder(ClassBuilder classBuilder)
    {
        _classBuilder = classBuilder;
    }

    internal ClassBuilder FinishMethodBuild()
    {
        MethodDeclarationSyntax methodDeclarationSyntax;
        _classBuilder.AddMethodDefinition(methodDeclarationSyntax);
        return _classBuilder;
    }
}