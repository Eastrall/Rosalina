#if UNITY_EDITOR

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using UnityEditor;
using Microsoft.CodeAnalysis;

internal class RosalinaEditorWindowScriptGenerator : IRosalinaGeneartor
{
    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        CompilationUnitSyntax compilationUnit = SyntaxFactory
            .CompilationUnit()
            .AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("UnityEditor")),
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("UnityEngine")),
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("UnityEngine.UIElements"))
            )
            .AddMembers(
                SyntaxFactory
                    .ClassDeclaration(documentAsset.Name)
                    .AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword),
                        SyntaxFactory.Token(SyntaxKind.PartialKeyword)
                    )
                    .AddBaseListTypes(
                        SyntaxFactory.SimpleBaseType(
                            SyntaxFactory.ParseName(nameof(EditorWindow))
                        )
                    )
                    .AddMembers(
                        RosalinaSyntaxFactory
                            .CreateMethod("void", "OnCreateGUI", SyntaxKind.PartialKeyword)
                            .WithBody(
                                SyntaxFactory.Block()
                            )
                    )
            );

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(code);
    }
}

#endif