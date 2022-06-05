#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using UnityEditor;

/// <summary>
/// Provides helpers for code generation using Roslyn.
/// </summary>
internal static class RosalinaSyntaxFactory
{
    /// <summary>
    /// Creates a field.
    /// </summary>
    /// <param name="fieldType">Field type.</param>
    /// <param name="fieldName">Field name.</param>
    /// <param name="modifiers">Field modifiers.</param>
    /// <returns>Field declaration syntax.</returns>
    public static FieldDeclarationSyntax CreateField(string fieldType, string fieldName, params SyntaxKind[] modifiers)
    {
        SyntaxToken[] fieldModifiers = modifiers.Select(x => SyntaxFactory.Token(x)).ToArray();
        NameSyntax variableTypeName = SyntaxFactory.ParseName(fieldType);
        VariableDeclarationSyntax variableSyntax = SyntaxFactory.VariableDeclaration(variableTypeName)
            .AddVariables(SyntaxFactory.VariableDeclarator(fieldName));

        return SyntaxFactory.FieldDeclaration(variableSyntax)
            .AddModifiers(fieldModifiers);
    }

    /// <summary>
    /// Creates a property.
    /// </summary>
    /// <param name="propertyType">Property type.</param>
    /// <param name="propertyName">Property name.</param>
    /// <param name="modifiers">Property modifiers.</param>
    /// <returns>Property declaration syntax.</returns>
    public static PropertyDeclarationSyntax CreateProperty(string propertyType, string propertyName, params SyntaxKind[] modifiers)
    {
        SyntaxToken[] propertyModifiers = modifiers.Select(x => SyntaxFactory.Token(x)).ToArray();
        NameSyntax propertyTypeName = SyntaxFactory.ParseName(propertyType);
        PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(propertyTypeName, propertyName)
            .AddModifiers(propertyModifiers);

        return property;
    }

    /// <summary>
    /// Creates a method.
    /// </summary>
    /// <param name="returnType">Method return type.</param>
    /// <param name="methodName">Method identifier name.</param>
    /// <param name="modifiers">Method modifiers.</param>
    /// <returns>Method declaration syntax.</returns>
    public static MethodDeclarationSyntax CreateMethod(string returnType, string methodName, params SyntaxKind[] modifiers)
    {
        SyntaxToken[] methodModifiers = modifiers.Select(x => SyntaxFactory.Token(x)).ToArray();
        TypeSyntax methodReturnType = SyntaxFactory.ParseTypeName(returnType);

        MethodDeclarationSyntax initializeMethod = SyntaxFactory
            .MethodDeclaration(methodReturnType, methodName)
            .AddModifiers(methodModifiers)
            .WithBody(SyntaxFactory.Block());

        return initializeMethod;
    }
}

#endif