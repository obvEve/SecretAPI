namespace SecretAPI.CodeGeneration.Generators;

using System.Linq.Expressions;

/// <summary>
/// Code generator for custom commands, creating validation etc.
/// </summary>
[Generator]
public class CustomCommandGenerator : IIncrementalGenerator
{
    private const string CommandName = "CustomCommand";
    private const string ExecuteMethodName = "Execute";
    private const string ExecuteCommandMethodAttributeLocation = "SecretAPI.Features.Commands.Attributes.ExecuteCommandAttribute";
    private const string CommandResultLocation = "CommandResult";
    private const string ArgumentsParamName = "arguments";

    private static readonly MethodParameter ArgumentsParam =
        new(
            identifier: ArgumentsParamName,
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
        SourceProductionContext context,
        INamedTypeSymbol namedClassSymbol,
        ImmutableArray<MethodDeclarationSyntax> executeMethods)
    {
        const string ResultArgName = "result";

        if (namedClassSymbol.IsAbstract)
            return;

        if (namedClassSymbol.BaseType?.Name != CommandName)
            return;

        ClassBuilder classBuilder = ClassBuilder.CreateBuilder(namedClassSymbol)
            .AddUsingStatements("System", "System.Linq", "System.Collections.Generic")
            .AddUsingStatements("CommandSystem")
            .AddUsingStatements("SecretAPI.Features.Commands", "SecretAPI.Features.Commands.Validators")
            .AddModifiers(SyntaxKind.PartialKeyword);

        List<StatementSyntax> executeValidateStatements = new();
        foreach (MethodDeclarationSyntax method in executeMethods)
        {
            if (method.ReturnType.ToString() != CommandResultLocation)
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(
                        CommandDiagnostics.InvalidExecuteMethod,
                        method.ReturnType.GetLocation(),
                        method.Identifier.Text,
                        "Return type should be of type " + CommandResultLocation
                    )
                );

                continue;
            }

            executeValidateStatements.Add(GetExecuteCheckSyntax(method));
        }

        LocalDeclarationStatementSyntax resultDeclaration = LocalDeclarationStatement(
            VariableDeclaration(NullableType(IdentifierName(CommandResultLocation)))
                .AddVariables(VariableDeclarator(ResultArgName)
                    .WithInitializer(EqualsValueClause(LiteralExpression(SyntaxKind.NullLiteralExpression)))));

        classBuilder.StartMethodCreation(ExecuteMethodName, SyntaxKind.BoolKeyword)
            .AddModifiers(SyntaxKind.PublicKeyword, SyntaxKind.OverrideKeyword)
            .AddParameters(ArgumentsParam, SenderParam, ResponseParam)
            .AddStatements(GenerateSubCommandCheck())
            .AddStatements(resultDeclaration)
            .AddStatements(executeValidateStatements.ToArray())
            /*.AddStatements(ReturnStatement(LiteralExpression(SyntaxKind.TrueLiteralExpression)))*/
            .FinishMethodBuild();

        classBuilder.Build(context, $"{namedClassSymbol.Name}.g.cs");
    }

    private static StatementSyntax GetExecuteCheckSyntax(MethodDeclarationSyntax methodDeclarationSyntax)
    {
        List<StatementSyntax> statements = new();

        foreach (ParameterSyntax parameterSyntax in methodDeclarationSyntax.ParameterList.Parameters)
        {
        }

        return Block();
    }

    private static StatementSyntax GenerateSubCommandCheck()
    {
        const string CheckSubCommandMethodIdentifier = "CheckSubCommand";
        const string CheckSubCommandCommandParamIdentifier = "subCommand";
        const string CheckSubCommandResultIdentifier = "checkSubCommandResult";

        /*ForEachStatementSyntax forEach =
            ForEachStatement(IdentifierName(CommandName), IdentifierName(SubCommandIdentifier), IdentifierName(SubCommandGetterIdentifier), Block());*/

        // if (CheckSubCommand(arguments.First(), out CustomCommand? subCommand))
        IfStatementSyntax ifSubCommandCheck = IfStatement(
            InvocationExpression(IdentifierName(CheckSubCommandMethodIdentifier))
                .WithArgumentList(ArgumentList(SeparatedList(new[]
                {
                    Argument(
                        InvocationExpression(
                            MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression, 
                                IdentifierName(ArgumentsParamName),
                                IdentifierName("First")))),
                    Argument(
                        DeclarationExpression(
                            NullableType(
                                IdentifierName(CommandName)),
                            SingleVariableDesignation(
                                Identifier(CheckSubCommandCommandParamIdentifier))))
                        .WithRefOrOutKeyword(Token(SyntaxKind.OutKeyword))
                }))),
            Block());
        
        IfStatementSyntax ifArgumentsAnyStatement = IfStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(ArgumentsParamName),
                    IdentifierName("Any"))),
            Block(ifSubCommandCheck));

        /*
          Generate :
          if (arguments.Any())
          {
            if (CheckSubCommand(arguments.First(), out CustomCommand? command))
            {
                bool subCommandResult = command.Execute(arguments, sender, out response);
                return subCommandResult;
            }
          }
        */

        return ifArgumentsAnyStatement;
    }
}

// ! Example of basic structure needed

/*

ValidatorSingleton<PlayerArgumentValidator>.Instance.Validate(arguments.First());
ValidatorSingleton<EnumArgumentValidator<RoleTypeId>>.Instance.Validate(arguments.First());

// <auto-generated>
#pragma warning disable

namespace SecretAPI.Examples.Commands
{
    using System.CodeDom.Compiler;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommandSystem;
    using Features.Commands;

    partial class ExampleParentCommand
    {
        public override bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Any())
            {
                string argument = arguments.First();
                foreach (CustomCommand subCommand in SubCommands)
                {
                    if (argument == subCommand.Command || subCommand.Aliases.Any(a => a == argument))
                        subCommand.Execute(arguments, sender, out response);
                }
            }
        }
    }
}

*/