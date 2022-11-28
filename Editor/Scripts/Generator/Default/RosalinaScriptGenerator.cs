#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using UnityEngine;

internal class RosalinaScriptGenerator : IRosalinaGeneartor
{
    private const string InitializeMethodName = "InitializeDocument";

    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        var initializeDocumentMethod = SyntaxFactory.ExpressionStatement(
            SyntaxFactory.InvocationExpression(
                SyntaxFactory.IdentifierName(InitializeMethodName)
            )
        );

        MethodDeclarationSyntax onEnableMethod = RosalinaSyntaxFactory.CreateMethod("void", UnityConstants.OnEnableHookName, SyntaxKind.PrivateKeyword)
            .WithBody(SyntaxFactory.Block(initializeDocumentMethod));

        ClassDeclarationSyntax @class = SyntaxFactory.ClassDeclaration(documentAsset.Name)
           .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
           .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
           .AddBaseListTypes(
               SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseName(typeof(MonoBehaviour).Name))
           )
           .AddMembers(onEnableMethod);

        CompilationUnitSyntax compilationUnit = SyntaxFactory.CompilationUnit()
            .AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("UnityEngine"))
            )
            .AddMembers(@class);

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(code);
    }
}

#endif
