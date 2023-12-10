#if UNITY_EDITOR

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System;
using UnityEditor;
using Microsoft.CodeAnalysis;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal class RosalinaEditorWindowScriptGenerator : IRosalinaCodeGeneartor
{
    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        var compilationUnit = CompilationUnit()
            .AddUsings(
                UsingDirective(IdentifierName("UnityEditor")),
                UsingDirective(IdentifierName("UnityEngine")),
                UsingDirective(IdentifierName("UnityEngine.UIElements"))
            );

        var classDeclaration = ClassDeclaration(documentAsset.Name)
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword),
                Token(SyntaxKind.PartialKeyword)
            )
            .AddBaseListTypes(
                SimpleBaseType(
                    ParseName(nameof(EditorWindow))
                )
            )
            .AddMembers(
                MethodDeclaration(ParseTypeName("void"), "OnCreateGUI")
                    .AddModifiers(Token(SyntaxKind.PartialKeyword))
                    .WithBody(
                        Block()
                    )
            );
        
        var code = RosalinaGeneratorHelper.Generate(compilationUnit, classDeclaration);
        return new RosalinaGenerationResult(code, includeHeader: false);
    }
}

#endif