using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using UnityEditor;

internal class RosalinaSyntaxFactory
{
    public static FieldDeclarationSyntax CreateField(string fieldType, string fieldName, params SyntaxKind[] modifiers)
    {
        SyntaxToken[] fieldModifiers = modifiers.Select(x => SyntaxFactory.Token(x)).ToArray();
        NameSyntax variableTypeName = SyntaxFactory.ParseName(fieldType);
        VariableDeclarationSyntax variableSyntax = SyntaxFactory.VariableDeclaration(variableTypeName)
            .AddVariables(SyntaxFactory.VariableDeclarator(fieldName));

        return SyntaxFactory.FieldDeclaration(variableSyntax)
            .AddModifiers(fieldModifiers);
    }

    public static PropertyDeclarationSyntax CreateProperty(string propertyType, string propertyName, params SyntaxKind[] modifiers)
    {
        SyntaxToken[] propertyModifiers = modifiers.Select(x => SyntaxFactory.Token(x)).ToArray();
        NameSyntax propertyTypeName = SyntaxFactory.ParseName(propertyType);
        PropertyDeclarationSyntax property = SyntaxFactory.PropertyDeclaration(propertyTypeName, propertyName)
            .AddModifiers(propertyModifiers);

        return property;
    }
}
