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

        InitializationStatement[] statements = RosalinaStatementSyntaxFactory.GenerateInitializeStatements(documentAsset.UxmlDocument, CreateRootQueryMethodAccessor());
        PropertyDeclarationSyntax[] propertyStatements = statements.Select(x => x.Property).ToArray();
        StatementSyntax[] initializationStatements = statements.Select(x => x.Statement).ToArray();

        MethodDeclarationSyntax initializeMethod = RosalinaSyntaxFactory.CreateMethod("void", InitializeDocumentMethodName, SyntaxKind.PublicKeyword)
            .WithBody(Block(initializationStatements));

        ClassDeclarationSyntax @class = ClassDeclaration(documentAsset.Name)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddModifiers(Token(SyntaxKind.PartialKeyword))
            .AddMembers(
                CreateDocumentVariable(),
                CreateRootElementVariable()
            )
            .AddMembers(propertyStatements)
            .AddMembers(
                CreateVisualElementRootProperty(),
                CreateConstructorWithVisualElement(documentAsset.Name),
                initializeMethod
            );

        CompilationUnitSyntax compilationUnit = CompilationUnit()
            .AddUsings(
                UsingDirective(IdentifierName("UnityEngine")),
                UsingDirective(IdentifierName("UnityEngine.UIElements"))
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
        NameSyntax serializeFieldName = ParseName(typeof(SerializeField).Name);

        FieldDeclarationSyntax documentField = RosalinaSyntaxFactory.CreateField(documentPropertyTypeName, DocumentFieldName, SyntaxKind.PrivateKeyword)
            .AddAttributeLists(
                AttributeList(
                    SingletonSeparatedList(
                        Attribute(serializeFieldName)
                    )
                )
            );

        return documentField;
    }

    private static MemberDeclarationSyntax CreateRootElementVariable()
    {
        return FieldDeclaration(
                VariableDeclaration(IdentifierName(typeof(VisualElement).Name))
                    .AddVariables(
                        VariableDeclarator("_rootElement")
                    )
            )
            .AddModifiers(Token(SyntaxKind.PrivateKeyword));
    }

    private static MemberDeclarationSyntax CreateVisualElementRootProperty()
    {
        return PropertyDeclaration(IdentifierName(typeof(VisualElement).Name), RootVisualElementPropertyName)
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .WithExpressionBody(
                ArrowExpressionClause(
                    AssignmentExpression(
                        SyntaxKind.CoalesceAssignmentExpression,
                        IdentifierName("_rootElement"),
                        IdentifierName($"{DocumentFieldName}?.rootVisualElement")
                    )
                )
            )
            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken));
    }

    private static MemberAccessExpressionSyntax CreateRootQueryMethodAccessor()
    {
        return MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName($"{RootVisualElementPropertyName}?"),
            Token(SyntaxKind.DotToken),
            IdentifierName(UnityConstants.RootVisualElementQueryMethodName)
        );
    }

    private static ConstructorDeclarationSyntax CreateConstructorWithVisualElement(string className)
    {
        return ConstructorDeclaration(Identifier(className))
            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(
                ParameterList(
                    SingletonSeparatedList<ParameterSyntax>(
                        Parameter(Identifier("root")).WithType(IdentifierName(typeof(VisualElement).Name))
                    )
                )
            )
            .WithBody(
                Block(
                    ExpressionStatement(
                        AssignmentExpression(
                            SyntaxKind.SimpleAssignmentExpression,
                            IdentifierName("_rootElement"),
                            IdentifierName("root")
                        )
                    ),
                    ExpressionStatement(
                        InvocationExpression(IdentifierName("InitializeDocument"))
                    )
                )
            );
    }
}

#endif