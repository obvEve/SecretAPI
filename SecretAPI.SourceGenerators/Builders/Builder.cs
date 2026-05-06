namespace SecretAPI.SourceGenerators.Builders;

/// <summary>
/// Base of a builder.
/// </summary>
/// <typeparam name="TBuilder">The <see cref="Builder{TBuilder}"/> this is handling.</typeparam>
internal abstract class Builder<TBuilder>
    where TBuilder : Builder<TBuilder>
{
    protected readonly List<SyntaxToken> _modifiers = new();
    
    internal TBuilder AddModifiers(params SyntaxKind[] modifiers)
    {
        foreach (SyntaxKind token in modifiers)
            _modifiers.Add(Token(token));

        return (TBuilder)this;
    }
}