using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityEngine;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

public static class RosalinaGeneratorHelper
{
    public static string Generate(CompilationUnitSyntax compilationUnit, ClassDeclarationSyntax classDeclaration)
    {
        if (!string.IsNullOrEmpty(RosalinaSettings.instance.Namespace))
        {
            compilationUnit = compilationUnit.AddMembers(
                NamespaceDeclaration(IdentifierName(RosalinaSettings.instance.Namespace))
                    .AddMembers(classDeclaration)
            );
        }
        else
        {
            compilationUnit = compilationUnit.AddMembers(classDeclaration);
        }
        
        return compilationUnit
            .NormalizeWhitespace()
            .ToFullString();
    }
}
