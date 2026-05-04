namespace SecretAPI.SourceGenerators.Generators;

/// <summary>
/// Code generator for CallOnLoad/CallOnUnload
/// TODO: Implement IRegister source generation
/// </summary>
[Generator]
public class CallOnLoadGenerator : IIncrementalGenerator
{
    private const string GeneratedClassName = "SecretApiGenerated";
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
        
        context.RegisterSourceOutput(callProvider.Collect(), Generate);
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
        ImmutableArray<(IMethodSymbol method, bool isLoad, bool isUnload)> methods)
    {
        if (methods.IsEmpty)
            return;

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

        // ClassBuilder classBuilder = ClassBuilder.CreateBuilder(pluginInfo.Item2)
        ClassBuilder classBuilder = ClassBuilder.CreateBuilder(ClassDeclaration(GeneratedClassName))
            .AddUsingStatements("System")
            .AddModifiers(SyntaxKind.InternalKeyword, SyntaxKind.StaticKeyword);

        classBuilder.StartMethodCreation("OnLoad", SyntaxKind.VoidKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword)
            .AddStatements(MethodCallStatements(loadCalls))
            .FinishMethodBuild();

        classBuilder.StartMethodCreation("OnUnload", SyntaxKind.VoidKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword)
            .AddStatements(MethodCallStatements(unloadCalls))
            .FinishMethodBuild();

        classBuilder.Build(context, $"{GeneratedClassName}.g.cs");
    }
}