#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal class RosalinaDocumentBindingsGenerator : IRosalinaCodeGeneartor
{
    private const string DocumentFieldName = "_document";
    private const string RootVisualElementPropertyName = "Root";
    private const string InitializeDocumentMethodName = "InitializeDocument";

    public RosalinaGenerationResult Generate(UIDocumentAsset document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document), "Cannot generate binding with a null document asset.");
        }

        InitializationStatement[] statements =
            RosalinaStatementSyntaxFactory.GenerateInitializeStatements(document, $"{RootVisualElementPropertyName}?.Q");
        PropertyDeclarationSyntax[] propertyStatements = statements.Select(x => x.Property).ToArray();
        StatementSyntax[] initializationStatements = statements.Select(x => x.Statement).ToArray();

        MethodDeclarationSyntax initializeMethod = MethodDeclaration(ParseTypeName("void"), InitializeDocumentMethodName)
            .AddModifiers(
                Token(SyntaxKind.PublicKeyword)
            )
            .WithBody(
                Block(initializationStatements)
            );

        ClassDeclarationSyntax classDeclaration = ClassDeclaration(document.Name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddModifiers(Token(SyntaxKind.PartialKeyword))
            .AddMembers(
                CreateDocumentVariable()
            )
            .AddMembers(propertyStatements)
            .AddMembers(
                CreateVisualElementRootProperty(),
                initializeMethod
            );

        var compilationUnit = CompilationUnit().AddUsings(
            UsingDirective(IdentifierName("UnityEngine")),
            UsingDirective(IdentifierName("UnityEngine.UIElements"))
        );
            
        var code = RosalinaGeneratorHelper.Generate(compilationUnit, classDeclaration);
        return new RosalinaGenerationResult(code);
    }

    private static MemberDeclarationSyntax CreateDocumentVariable()
    {
        NameSyntax serializeFieldName = ParseName(typeof(SerializeField).Name);

        return FieldDeclaration(
                VariableDeclaration(
                    ParseName(typeof(UIDocument).Name)
                )
                .AddVariables(
                    VariableDeclarator(DocumentFieldName)
                )
            )
            .AddModifiers(Token(SyntaxKind.PrivateKeyword))
            .AddAttributeLists(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(serializeFieldName)
                    )
                )
            );
    }

    private static MemberDeclarationSyntax CreateVisualElementRootProperty()
    {
        return PropertyDeclaration(
                IdentifierName(typeof(VisualElement).Name), RootVisualElementPropertyName
             )
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(
                ArrowExpressionClause(
                    IdentifierName($"{DocumentFieldName}?.rootVisualElement")
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }
}

#endif