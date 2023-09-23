#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal sealed class RosalinaComponentScriptGenerator : IRosalinaCodeGeneartor
{
    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        CompilationUnitSyntax compilationUnit = CompilationUnit()
            .AddUsings(
                UsingDirective(IdentifierName("UnityEditor")),
                UsingDirective(IdentifierName("UnityEngine")),
                UsingDirective(IdentifierName("UnityEngine.UIElements"))
            )
            .AddMembers(
                ClassDeclaration(documentAsset.Name)
                    .AddModifiers(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.PartialKeyword)
                    )
            );

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(code, includeHeader: false);
    }
}

#endif