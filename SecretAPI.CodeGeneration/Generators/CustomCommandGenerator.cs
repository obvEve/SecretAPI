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
    private const string SenderParamName = "sender";
    private const string ResponseParamName = "response";

    private static readonly MethodParameter ArgumentsParam =
        new(
            identifier: ArgumentsParamName,
            type: GetSingleGenericTypeSyntax("ArraySegment", SyntaxKind.StringKeyword)
        );

    private static readonly MethodParameter SenderParam =
        new(
            identifier: SenderParamName,
            type: IdentifierName("ICommandSender")
        );

    private static readonly MethodParameter ResponseParam =
        new(
            identifier: ResponseParamName,
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
            // The CommandSenderAttribute if exists - defines that the param is intended for the sender and is not required
            AttributeSyntax? commandSenderAttribute = parameterSyntax.AttributeLists
                .SelectMany(list => list.Attributes)
                .FirstOrDefault(attribute => attribute.Name.ToString() == "CommandSenderAttribute");

            // The ValidateArgumentAttribute if exists - contains the custom Type for the validator the user wants on the param 
            AttributeSyntax? validateAttribute = parameterSyntax.AttributeLists
                .SelectMany(list => list.Attributes)
                .FirstOrDefault(attribute => attribute.Name.ToString() == "ValidateArgumentAttribute");

            bool isOptional = parameterSyntax.Default != null; // whether the param is optional and thus does not require user input
            ExpressionSyntax? defaultValue = parameterSyntax.Default?.Value; // the default value expression when optional, otherwise null

            TypeOfExpressionSyntax? validateAttributeType = validateAttribute?.ArgumentList?.Arguments.First().Expression as TypeOfExpressionSyntax;
        }

        return Block();
    }

    private static StatementSyntax GenerateSubCommandCheck()
    {
        const string CheckSubCommandMethodIdentifier = "CheckSubCommand";
        const string CheckSubCommandCommandParamIdentifier = "subCommand";
        const string CheckSubCommandResultIdentifier = "checkSubCommandResult";

        // bool checkSubCommandResult = subCommand.Execute(arguments, sender, out response);
        LocalDeclarationStatementSyntax subCommandExecute = LocalDeclarationStatement(
            VariableDeclaration(GetPredefinedTypeSyntax(SyntaxKind.BoolKeyword))
                .AddVariables(VariableDeclarator(CheckSubCommandResultIdentifier)
                    .WithInitializer(EqualsValueClause(
                        InvocationExpression(MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            IdentifierName(CheckSubCommandCommandParamIdentifier),
                            IdentifierName(ExecuteMethodName))).WithArgumentList(ArgumentList(SeparatedList(new[]
                        {
                            Argument(IdentifierName(ArgumentsParamName)),
                            Argument(IdentifierName(SenderParamName)),
                            Argument(IdentifierName(ResponseParamName))
                                .WithRefOrOutKeyword(Token(SyntaxKind.OutKeyword))
                        })))))));

        // return checkSubCommandResult;
        ReturnStatementSyntax returnStatement = ReturnStatement(IdentifierName(CheckSubCommandResultIdentifier));
        
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
            Block(subCommandExecute, returnStatement));

        // if (arguments.Any())
        IfStatementSyntax ifArgumentsAnyStatement = IfStatement(
            InvocationExpression(
                MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(ArgumentsParamName),
                    IdentifierName("Any"))),
            Block(ifSubCommandCheck));

        return ifArgumentsAnyStatement;
    }
}

/*

ValidatorSingleton<PlayerArgumentValidator>.Instance.Validate(arguments.First());
ValidatorSingleton<EnumArgumentValidator<RoleTypeId>>.Instance.Validate(arguments.First());

*/