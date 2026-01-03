namespace SecretAPI.CodeGeneration.CodeBuilders;

/// <summary>
/// Base of a code builder.
/// </summary>
/// <typeparam name="TCodeBuilder">The <see cref="CodeBuilder{TCodeBuilder}"/> this is handling.</typeparam>
internal abstract class CodeBuilder<TCodeBuilder>
    where TCodeBuilder : CodeBuilder<TCodeBuilder>
{
    protected readonly List<SyntaxToken> _modifiers = new();
    
    internal TCodeBuilder AddModifiers(params SyntaxKind[] modifiers)
    {
        foreach (SyntaxKind token in modifiers)
            _modifiers.Add(Token(token));

        return (TCodeBuilder)this;
    }
}