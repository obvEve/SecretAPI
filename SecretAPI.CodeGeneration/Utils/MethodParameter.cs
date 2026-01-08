namespace SecretAPI.CodeGeneration.Utils;

/// <summary>
/// Represents a method parameter used during code generation.
/// </summary>
internal readonly struct MethodParameter
{
    private readonly SyntaxList<AttributeListSyntax> _attributeLists;
    private readonly SyntaxTokenList _modifiers;
    private readonly TypeSyntax? _type;
    private readonly SyntaxToken _identifier;
    private readonly EqualsValueClauseSyntax? _default;

    /// <summary>
    /// Creates a new instance of <see cref="MethodParameter"/>.
    /// </summary>
    /// <param name="identifier">The name of the parameter.</param>
    /// <param name="type">The parameter type. May be <see langword="null"/> for implicitly-typed parameters.</param>
    /// <param name="modifiers">Optional parameter modifiers (e.g. <c>ref</c>, <c>out</c>, <c>in</c>).</param>
    /// <param name="attributeLists">Optional attribute lists applied to the parameter.</param>
    /// <param name="default">Optional default value.</param>
    internal MethodParameter(
        string identifier,
        TypeSyntax? type = null,
        SyntaxTokenList modifiers = default,
        SyntaxList<AttributeListSyntax> attributeLists = default,
        EqualsValueClauseSyntax? @default = null)
    {
        _identifier = IsValidIdentifier(identifier)
            ? Identifier(identifier)
            : throw new ArgumentException("Identifier is not valid.", nameof(identifier));

        _type = type;
        _modifiers = modifiers;
        _attributeLists = attributeLists;
        _default = @default;
    }

    public ParameterSyntax Syntax => Parameter(_attributeLists, _modifiers, _type, _identifier, _default);
}