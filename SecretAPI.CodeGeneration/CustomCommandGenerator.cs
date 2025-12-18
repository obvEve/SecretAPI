namespace SecretAPI.CodeGeneration;

using System.CodeDom.Compiler;
using System.Collections.Immutable;
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
    private const string ExecuteMethodName = "Execute";
    private const string ExecuteCommandMethodAttributeLocation = "SecretAPI.Features.Commands.Attributes.ExecuteCommandAttribute";

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(INamedTypeSymbol?, ImmutableArray<MethodDeclarationSyntax>)> classProvider
            = context.SyntaxProvider.CreateSyntaxProvider(
            static (node, _) => node is ClassDeclarationSyntax,
            static (ctx, cancel) =>
            {
                ClassDeclarationSyntax classSyntax = (ClassDeclarationSyntax)ctx.Node;
                INamedTypeSymbol? typeSymbol = ctx.SemanticModel.GetDeclaredSymbol(classSyntax, cancel) as INamedTypeSymbol;
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
                ITypeSymbol? attributeTypeSymbol = context.SemanticModel.GetTypeInfo(attributeSyntax).Type;
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

        using StringWriter writer = new();
        using IndentedTextWriter indentWriter = new(writer);

        indentWriter.WriteGeneratedText()
            .WriteNamespace(namedClassSymbol, true)
            .WriteUsings("System", "CommandSystem")
            .WritePartialClass(namedClassSymbol.Name, true)
            .WriteMethod(ExecuteMethodName, "bool", true, Accessibility.Public, true, "ArraySegment<string> arguments",
                "ICommandSender sender", "out string response");

        indentWriter.WriteLine("response = \"Command not implemented.\";");
        indentWriter.WriteLine("return false;");
        indentWriter.WriteLine($"// {string.Join(" -> ", executeMethods.Select(m => m.Identifier))}");

        indentWriter.FinishAllIndentations();

        ctx.AddSource($"{namedClassSymbol.Name}.g.cs", writer.ToString());
    }
}