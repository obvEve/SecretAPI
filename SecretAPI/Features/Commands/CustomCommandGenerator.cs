/*namespace SecretAPI.Features.Commands
{
    using System.CodeDom.Compiler;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    /// <summary>
    /// Hello.
    /// </summary>
    [Generator]
    internal class CustomCommandGenerator : IIncrementalGenerator
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

            indentWriter.Write("hello");

            ctx.AddSource($"{symbol.ContainingNamespace}.{symbol.Name}.Generated.cs", writer.ToString());
        }
    }
}*/