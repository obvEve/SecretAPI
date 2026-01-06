namespace SecretAPI.CodeGeneration.Generators;

/// <summary>
/// Code generator for CallOnLoad/CallOnUnload
/// </summary>
[Generator]
public class CallOnLoadGenerator : IIncrementalGenerator
{
    private const string PluginBaseClassName = "Plugin";
    private const string CallOnLoadAttributeLocation = "SecretAPI.Attribute.CallOnLoadAttribute";
    private const string CallOnUnloadAttributeLocation = "SecretAPI.Attribute.CallOnUnloadAttribute";

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<IMethodSymbol> methodProvider =
            context.SyntaxProvider.CreateSyntaxProvider(
                    static (node, _) => node is MethodDeclarationSyntax { AttributeLists.Count: > 0 },
                    static (ctx, _) =>
                        ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as IMethodSymbol)
                .Where(static m => m is not null)!;

        IncrementalValuesProvider<(IMethodSymbol method, bool isLoad, bool isUnload)> callProvider =
            methodProvider.Select(static (method, _) => (
                    method,
                    HasAttribute(method, CallOnLoadAttributeLocation),
                    HasAttribute(method, CallOnUnloadAttributeLocation)))
                .Where(static m => m.Item2 || m.Item3);

        IncrementalValuesProvider<INamedTypeSymbol> pluginClassProvider =
            context.SyntaxProvider.CreateSyntaxProvider(
                    static (node, _) => node is ClassDeclarationSyntax,
                    static (ctx, _) =>
                        ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as INamedTypeSymbol)
                .Where(static c =>
                    c is { IsAbstract: false, BaseType.Name: PluginBaseClassName })!;
        
        context.RegisterSourceOutput(pluginClassProvider.Combine(callProvider.Collect()), static (context, data) =>
        {
            Generate(context, data.Left, data.Right);
        });
    }
    
    private static bool HasAttribute(IMethodSymbol? method, string attributeLocation)
    {
        if (method == null)
            return false;
        
        foreach (AttributeData attribute in method.GetAttributes())
        {
            if (attribute.AttributeClass?.ToDisplayString() == attributeLocation)
                return true;
        }

        return false;
    }
    
    private static int GetPriority(IMethodSymbol method, string attributeLocation)
    {
        AttributeData? attribute = method.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == attributeLocation);
        if (attribute == null)
            return 0;

        if (attribute.ConstructorArguments.Length > 0)
            return (int)attribute.ConstructorArguments[0].Value!;
    
        return 0;
    }

    private static void Generate(
        SourceProductionContext context,
        INamedTypeSymbol? pluginClassSymbol,
        ImmutableArray<(IMethodSymbol method, bool isLoad, bool isUnload)> methods)
    {
        if (pluginClassSymbol == null || methods.IsEmpty)
            return;
        
        IMethodSymbol[] loadCalls = methods
            .Where(m => m.isLoad)
            .Select(m => m.method)
            .OrderBy(m => GetPriority(m, CallOnLoadAttributeLocation))
            .ToArray();

        IMethodSymbol[] unloadCalls = methods
            .Where(m => m.isUnload)
            .Select(m => m.method)
            .OrderBy(m => GetPriority(m, CallOnUnloadAttributeLocation))
            .ToArray();

        CompilationUnitSyntax compilation = ClassBuilder.CreateBuilder(pluginClassSymbol)
            .AddUsingStatements("System")
            .AddModifiers(SyntaxKind.PartialKeyword)
            .StartMethodCreation("OnLoad", "void")
            .AddModifiers(SyntaxKind.PublicKeyword)
            .AddStatements(MethodCallStatements(loadCalls))
            .FinishMethodBuild()
            .StartMethodCreation("OnUnload", "void")
            .AddModifiers(SyntaxKind.PublicKeyword)
            .AddStatements(MethodCallStatements(unloadCalls))
            .FinishMethodBuild()
            .Build();
        
        context.AddSource($"{pluginClassSymbol.Name}.g.cs",  compilation.ToFullString());
    }
}