namespace SecretAPI.CodeGeneration.Generators;

/// <summary>
/// Code generator for CallOnLoad/CallOnUnload
/// </summary>
[Generator]
public class CallOnLoadGenerator : IIncrementalGenerator
{
    private const string PluginNamespace = "LabApi.Loader.Features.Plugins";
    private const string PluginBaseClassName = "Plugin";
    private const string CallOnLoadAttributeLocation = "SecretAPI.Attributes.CallOnLoadAttribute";
    private const string CallOnUnloadAttributeLocation = "SecretAPI.Attributes.CallOnUnloadAttribute";

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

        IncrementalValuesProvider<INamedTypeSymbol?> pluginClassProvider =
            context.SyntaxProvider.CreateSyntaxProvider(
                    static (node, _) => node is ClassDeclarationSyntax,
                    static (ctx, _) =>
                        ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as INamedTypeSymbol)
                .Where(static c => c != null && !c.IsAbstract && c.BaseType?.Name == PluginBaseClassName &&
                                   c.BaseType.ContainingNamespace.ToDisplayString() == PluginNamespace);
        
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

    private static bool ShouldAutogenerate(IMethodSymbol method, string attributeLocation)
    {
        AttributeData? attribute = method.GetAttributes()
            .FirstOrDefault(a => a.AttributeClass?.ToDisplayString() == attributeLocation);

        if (attribute is { ConstructorArguments.Length: >= 2 })
            return (bool)attribute.ConstructorArguments[1].Value!;

        return false;
    }

    private static void Generate(
        SourceProductionContext context,
        INamedTypeSymbol? pluginClassSymbol,
        ImmutableArray<(IMethodSymbol method, bool isLoad, bool isUnload)> methods)
    {
        if (pluginClassSymbol == null || methods.IsEmpty)
            return;

        IMethodSymbol[] loadCalls = methods
            .Where(m => m.isLoad && ShouldAutogenerate(m.method, CallOnLoadAttributeLocation))
            .Select(m => m.method)
            .OrderBy(m => GetPriority(m, CallOnLoadAttributeLocation))
            .ToArray();

        IMethodSymbol[] unloadCalls = methods
            .Where(m => m.isUnload && ShouldAutogenerate(m.method, CallOnUnloadAttributeLocation))
            .Select(m => m.method)
            .OrderBy(m => GetPriority(m, CallOnUnloadAttributeLocation))
            .ToArray();

        if (!loadCalls.Any() && !unloadCalls.Any())
            return;

        CompilationUnitSyntax compilation = ClassBuilder.CreateBuilder(pluginClassSymbol)
            .AddUsingStatements("System")
            .AddModifiers(SyntaxKind.PartialKeyword)
            .StartMethodCreation("OnLoad", SyntaxKind.VoidKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword)
            .AddStatements(MethodCallStatements(loadCalls))
            .FinishMethodBuild()
            .StartMethodCreation("OnUnload", SyntaxKind.VoidKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword)
            .AddStatements(MethodCallStatements(unloadCalls))
            .FinishMethodBuild()
            .Build();

        context.AddSource($"{pluginClassSymbol.Name}.g.cs",  compilation.ToFullString());
    }
}