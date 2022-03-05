using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.IO;
using UnityEngine;

internal class RosalinaScriptGenerator
{
    private const string InitializeMethodName = "InitializeDocument";
    private const string UnityInitializeHookName = "OnEnable";

    public RosalinaGenerationResult Generate(UIDocumentAsset document, string outputFileName)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document), "Cannot generate binding with an empty UI document definition.");
        }

        if (string.IsNullOrEmpty(outputFileName))
        {
            throw new ArgumentException("An output file name is required.", nameof(outputFileName));
        }

        var initializeDocumentMethod = SyntaxFactory.ExpressionStatement(
            SyntaxFactory.InvocationExpression(
                SyntaxFactory.IdentifierName(InitializeMethodName)
            )
        );

        MethodDeclarationSyntax onEnableMethod = RosalinaSyntaxFactory.CreateMethod("void", UnityInitializeHookName, SyntaxKind.PrivateKeyword)
            .WithBody(SyntaxFactory.Block(initializeDocumentMethod));

        ClassDeclarationSyntax @class = SyntaxFactory.ClassDeclaration(document.Name)
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

        return new RosalinaGenerationResult(code, Path.Combine(document.Path, outputFileName));
    }
}
