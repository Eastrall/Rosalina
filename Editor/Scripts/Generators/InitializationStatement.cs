#if UNITY_EDITOR

using Microsoft.CodeAnalysis.CSharp.Syntax;

internal readonly struct InitializationStatement
{
    public StatementSyntax Statement { get; }

    public PropertyDeclarationSyntax Property { get; }

    public InitializationStatement(StatementSyntax statement, PropertyDeclarationSyntax property)
    {
        Statement = statement;
        Property = property;
    }
}

#endif