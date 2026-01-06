namespace SecretAPI.CodeGeneration.Generators;

/// <summary>
/// Code generator for custom commands, creating validation etc.
/// </summary>
[Generator]
public class CustomCommandGenerator : IIncrementalGenerator
{
    private const string CommandName = "CustomCommand";
    private const string ExecuteMethodName = "Execute";
    private const string ExecuteCommandMethodAttributeLocation = "SecretAPI.Features.Commands.Attributes.ExecuteCommandAttribute";

    private static readonly MethodParameter ArgumentsParam =
        new(
            identifier: "arguments",
            type: GetSingleGenericTypeSyntax("ArraySegment", SyntaxKind.StringKeyword)
        );
    
    private static readonly MethodParameter SenderParam =
        new(
            identifier: "sender",
            type: IdentifierName("ICommandSender")
        );
    
    private static readonly MethodParameter ResponseParam =
        new(
            identifier: "response",
            type: GetPredefinedTypeSyntax(SyntaxKind.StringKeyword),
            modifiers: TokenList(
                Token(SyntaxKind.OutKeyword))
        );

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(INamedTypeSymbol?, ImmutableArray<MethodDeclarationSyntax>)> classProvider
            = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax,
            static (ctx, cancel) =>
            {
                ClassDeclarationSyntax classSyntax = (ClassDeclarationSyntax)ctx.Node;
                INamedTypeSymbol? typeSymbol = ModelExtensions.GetDeclaredSymbol(ctx.SemanticModel, classSyntax, cancel) as INamedTypeSymbol;
                return (typeSymbol, GetExecuteMethods(ctx, classSyntax));
            }).Where(tuple => tuple is { typeSymbol: not null, Item2.IsEmpty: false });

        context.RegisterSourceOutput(classProvider, (ctx, tuple) => Generate(ctx, tuple.Item1!, tuple.Item2));
    }

    private static ImmutableArray<MethodDeclarationSyntax> GetExecuteMethods(
        GeneratorSyntaxContext context,
        ClassDeclarationSyntax classDeclarationSyntax)
    {
        List<MethodDeclarationSyntax> methods = new();
        foreach (MethodDeclarationSyntax method in classDeclarationSyntax.Members.OfType<MethodDeclarationSyntax>())
        {
            if (!IsExecuteMethod(context, method))
                continue;
            
            methods.Add(method);
        }
        
        return methods.ToImmutableArray();
    }

    private static bool IsExecuteMethod(GeneratorSyntaxContext context, MethodDeclarationSyntax methodDeclarationSyntax)
    {
        foreach (AttributeListSyntax attributeListSyntax in methodDeclarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                ITypeSymbol? attributeTypeSymbol = ModelExtensions.GetTypeInfo(context.SemanticModel, attributeSyntax).Type;
                if (attributeTypeSymbol != null && attributeTypeSymbol.ToDisplayString() == ExecuteCommandMethodAttributeLocation)
                    return true;
            }
        }

        return false;
    }

    private static void Generate(
        SourceProductionContext ctx,
        INamedTypeSymbol namedClassSymbol,
        ImmutableArray<MethodDeclarationSyntax> executeMethods)
    {
        if (namedClassSymbol.IsAbstract)
            return;

        if (namedClassSymbol.BaseType?.Name != CommandName)
            return;

        CompilationUnitSyntax compilation = ClassBuilder.CreateBuilder(namedClassSymbol)
            .AddUsingStatements("System", "System.Collections.Generic")
            .AddUsingStatements("CommandSystem")
            .AddModifiers(SyntaxKind.PartialKeyword)
            .StartMethodCreation(ExecuteMethodName, "bool")
            .AddModifiers(SyntaxKind.PublicKeyword, SyntaxKind.OverrideKeyword)
            .AddParameters(ArgumentsParam, SenderParam, ResponseParam)
            .FinishMethodBuild()
            .Build();

        ctx.AddSource($"{namedClassSymbol.Name}.g.cs", compilation.ToFullString());
    }
}