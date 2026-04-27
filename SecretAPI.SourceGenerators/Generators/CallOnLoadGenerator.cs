namespace SecretAPI.SourceGenerators.Generators;

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

        IncrementalValuesProvider<(ClassDeclarationSyntax?, INamedTypeSymbol?)> pluginClassProvider =
            context.SyntaxProvider.CreateSyntaxProvider(
                    static (node, _) => node is ClassDeclarationSyntax,
                    static (ctx, _) => (
                            ctx.Node as ClassDeclarationSyntax, ctx.SemanticModel.GetDeclaredSymbol(ctx.Node) as INamedTypeSymbol))
                .Where(static c => c.Item2 != null && !c.Item2.IsAbstract && c.Item2.BaseType?.Name == PluginBaseClassName &&
                                   c.Item2.BaseType.ContainingNamespace.ToDisplayString() == PluginNamespace);
        
        context.RegisterSourceOutput(pluginClassProvider.Combine(callProvider.Collect()), static (context, data) =>
        {
            Generate(context, new Tuple<ClassDeclarationSyntax?, INamedTypeSymbol?>(data.Left.Item1, data.Left.Item2), data.Right);
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

    private static bool ValidateMethod(SourceProductionContext context, IMethodSymbol method)
    {
        bool isValid = true;

        if (!method.IsStatic)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Diagnostics.MustBeStaticMethod,
                    method.Locations.FirstOrDefault(),
                    method.Name));

            isValid = false;
        }

        if (method.DeclaredAccessibility is Accessibility.Private)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Diagnostics.MustBeAccessibleMethod,
                    method.Locations.FirstOrDefault(),
                    method.Name,
                    method.DeclaredAccessibility));

            isValid = false;
        }

        return isValid;
    }

    private static void Generate(
        SourceProductionContext context,
        Tuple<ClassDeclarationSyntax?, INamedTypeSymbol?> pluginInfo,
        ImmutableArray<(IMethodSymbol method, bool isLoad, bool isUnload)> methods)
    {
        if (pluginInfo.Item1 == null || pluginInfo.Item2 == null || methods.IsEmpty)
            return;

        if (!pluginInfo.Item1.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Diagnostics.MustBePartialPluginClass,
                    pluginInfo.Item1.GetLocation(),
                    pluginInfo.Item1.Identifier.Text
                )
            );
        }

        IMethodSymbol[] loadCalls = methods
            .Where(m => m.isLoad && ValidateMethod(context, m.method))
            .Select(m => m.method)
            .OrderBy(m => GetPriority(m, CallOnLoadAttributeLocation))
            .ToArray();

        IMethodSymbol[] unloadCalls = methods
            .Where(m => m.isUnload && ValidateMethod(context, m.method))
            .Select(m => m.method)
            .OrderBy(m => GetPriority(m, CallOnUnloadAttributeLocation))
            .ToArray();

        if (!loadCalls.Any() && !unloadCalls.Any())
            return;

        ClassBuilder classBuilder = ClassBuilder.CreateBuilder(pluginInfo.Item2)
            .AddUsingStatements("System")
            .AddModifiers(SyntaxKind.PartialKeyword);

        classBuilder.StartMethodCreation("OnLoad", SyntaxKind.VoidKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword)
            .AddStatements(MethodCallStatements(loadCalls))
            .FinishMethodBuild();

        classBuilder.StartMethodCreation("OnUnload", SyntaxKind.VoidKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword)
            .AddStatements(MethodCallStatements(unloadCalls))
            .FinishMethodBuild();

        classBuilder.Build(context, $"{pluginInfo.Item2.Name}.g.cs");
    }
}