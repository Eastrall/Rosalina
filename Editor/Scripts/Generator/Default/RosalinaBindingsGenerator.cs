#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

internal class RosalinaBindingsGenerator : IRosalinaGeneartor
{
    private const string DocumentFieldName = "_document";
    private const string RootVisualElementPropertyName = "Root";
    private const string InitializeDocumentMethodName = "InitializeDocument";

    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        MemberDeclarationSyntax documentVariable = CreateDocumentVariable();
        MemberDeclarationSyntax visualElementProperty = CreateVisualElementRootProperty();
        InitializationStatement[] statements = RosalinaStatementSyntaxFactory.GenerateInitializeStatements(documentAsset.UxmlDocument, CreateRootQueryMethodAccessor());
        PropertyDeclarationSyntax[] propertyStatements = statements.Select(x => x.Property).ToArray();
        StatementSyntax[] initializationStatements = statements.Select(x => x.Statement).ToArray();

        MethodDeclarationSyntax initializeMethod = RosalinaSyntaxFactory.CreateMethod("void", InitializeDocumentMethodName, SyntaxKind.PublicKeyword)
            .WithBody(SyntaxFactory.Block(initializationStatements));

        ClassDeclarationSyntax @class = SyntaxFactory.ClassDeclaration(documentAsset.Name)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword))
            .AddMembers(documentVariable)
            .AddMembers(propertyStatements)
            .AddMembers(
                visualElementProperty,
                initializeMethod
            );

        CompilationUnitSyntax compilationUnit = SyntaxFactory.CompilationUnit()
            .AddUsings(
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("UnityEngine")),
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("UnityEngine.UIElements"))
             )
            .AddMembers(@class);

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();
        string generatedCode = RosalinaGeneratorConstants.GeneratedCodeHeader + code;

        return new RosalinaGenerationResult(generatedCode);
    }

    private static MemberDeclarationSyntax CreateDocumentVariable()
    {
        string documentPropertyTypeName = typeof(UIDocument).Name;
        NameSyntax serializeFieldName = SyntaxFactory.ParseName(typeof(SerializeField).Name);

        FieldDeclarationSyntax documentField = RosalinaSyntaxFactory.CreateField(documentPropertyTypeName, DocumentFieldName, SyntaxKind.PrivateKeyword)
            .AddAttributeLists(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Attribute(serializeFieldName)
                    )
                )
            );

        return documentField;
    }

    private static MemberDeclarationSyntax CreateVisualElementRootProperty()
    {
        string propertyTypeName = typeof(VisualElement).Name;
        string documentFieldName = $"{DocumentFieldName}?";

        return RosalinaSyntaxFactory.CreateProperty(propertyTypeName, RootVisualElementPropertyName, SyntaxKind.PublicKeyword)
            .AddAccessorListAccessors(
                SyntaxFactory.AccessorDeclaration(
                    SyntaxKind.GetAccessorDeclaration,
                    SyntaxFactory.Block(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.Token(SyntaxKind.ReturnKeyword),
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.IdentifierName(documentFieldName),
                                SyntaxFactory.IdentifierName(UnityConstants.DocumentRootVisualElementFieldName)),
                            SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                        )
                    )
                )
            );
    }

    private static MemberAccessExpressionSyntax CreateRootQueryMethodAccessor()
    {
        return SyntaxFactory.MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            SyntaxFactory.IdentifierName($"{RootVisualElementPropertyName}?"),
            SyntaxFactory.Token(SyntaxKind.DotToken),
            SyntaxFactory.IdentifierName(UnityConstants.RootVisualElementQueryMethodName)
        );
    }
}

#endif