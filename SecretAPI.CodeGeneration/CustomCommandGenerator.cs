namespace SecretAPI.CodeGeneration;

using System.CodeDom.Compiler;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;

/// <summary>
/// Code generator for custom commands, creating validation etc.
/// </summary>
[Generator]
public class CustomCommandGenerator : IIncrementalGenerator
{
    private const string CommandName = "CustomCommand";
    private const string ExecuteMethodName = "ExecuteGenerated";

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

        if (symbol.IsAbstract)
            return;

        if (symbol.BaseType?.Name != CommandName)
            return;

        using StringWriter writer = new();
        using IndentedTextWriter indentWriter = new(writer);

        indentWriter.WriteGeneratedText()
            .WriteNamespace(symbol, true)
            .WriteUsings("System", "CommandSystem")
            .WritePartialClass(symbol.Name, true);

        indentWriter.WriteLine($"protected override bool {ExecuteMethodName}(");
        indentWriter.Indent++;
        indentWriter.WriteLine("ArraySegment<string> arguments,");
        indentWriter.WriteLine("ICommandSender sender,");
        indentWriter.WriteLine("out string response)");
        indentWriter.Indent--;
        indentWriter.WriteLine("{");
        indentWriter.Indent++;

        indentWriter.WriteLine("response = \"Command not implemented.\";");
        indentWriter.WriteLine("return false;");

        indentWriter.FinishAllIndentations();

        ctx.AddSource($"{symbol.ContainingNamespace}.{symbol.Name}.g.cs", writer.ToString());
    }
}