namespace SecretAPI.CodeGeneration;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// Hello.
/// </summary>
[Generator]
public class CustomCommandGenerator : IIncrementalGenerator
{
    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<INamedTypeSymbol?> classProvider = context.SyntaxProvider.CreateSyntaxProvider(
                static (node, _) => node is ClassDeclarationSyntax,
                static (context, cancellationToken) =>
                {
                    if (context.Node is not ClassDeclarationSyntax classDecl)
                        return null;

                    return context.SemanticModel.GetDeclaredSymbol(classDecl, cancellationToken) as INamedTypeSymbol;
                })
            .Where(static cls => cls != null);

        context.RegisterSourceOutput(classProvider, Generate);
    }

    private static void Generate(SourceProductionContext ctx, INamedTypeSymbol? symbol)
    {
        if (symbol == null)
            return;

        using StringWriter writer = new();
        using IndentedTextWriter indentWriter = new(writer);

        indentWriter.WriteLine($"// {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        indentWriter.Write($"//{DateTime.Now::yyyy-MM-dd HH:mm:ss}");

        // ctx.AddSource($"{symbol.ContainingNamespace}.{symbol.Name}.g.cs", writer.ToString());
    }
}