#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using UnityEngine;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal class RosalinaDocumentScriptGenerator : IRosalinaCodeGeneartor
{
    private const string InitializeMethodName = "InitializeDocument";

    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        ClassDeclarationSyntax @class = ClassDeclaration(documentAsset.Name)
           .AddModifiers(Token(SyntaxKind.PublicKeyword))
           .AddModifiers(Token(SyntaxKind.PartialKeyword))
           .AddBaseListTypes(
               SimpleBaseType(ParseName(typeof(MonoBehaviour).Name))
           )
           .AddMembers(
                // private void OnEnable()
                MethodDeclaration(ParseTypeName("void"), "OnEnable")
                    .AddModifiers(Token(SyntaxKind.PrivateKeyword))
                    .WithBody(
                        Block(
                            ExpressionStatement(
                                InvocationExpression(IdentifierName(InitializeMethodName))
                            )
                        )
                    )
            );

        string code = CompilationUnit()
            .AddUsings(
                UsingDirective(IdentifierName("UnityEngine"))
            )
            .AddMembers(@class)
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(code, includeHeader: false);
    }
}

#endif
