#if UNITY_EDITOR
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal sealed class RosalinaComponentBindingsGenerator : IRosalinaCodeGeneartor
{
    public RosalinaGenerationResult Generate(UIDocumentAsset document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document), "Cannot generate binding with a null document asset.");
        }

        InitializationStatement[] statements = RosalinaStatementSyntaxFactory.GenerateInitializeStatements(document, $"Root?.Q");
        PropertyDeclarationSyntax[] propertyStatements = statements.Select(x => x.Property).ToArray();
        StatementSyntax[] initializationStatements = statements.Select(x => x.Statement).ToArray();

        CompilationUnitSyntax compilationUnit = CompilationUnit()
            .AddUsings(
                UsingDirective(IdentifierName("UnityEditor")),
                UsingDirective(IdentifierName("UnityEngine")),
                UsingDirective(IdentifierName("UnityEngine.UIElements"))
            )
            .AddMembers(
                ClassDeclaration(document.Name)
                    .AddModifiers(
                        Token(SyntaxKind.PublicKeyword),
                        Token(SyntaxKind.PartialKeyword)
                    )
                    .AddMembers(propertyStatements)
                    .AddMembers(CreateRootElementProperty())
                    .AddMembers(CreateConstructor(document.Name, initializationStatements))
            );

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(code);
    }

    private static ConstructorDeclarationSyntax CreateConstructor(string className, StatementSyntax[] initializationStatements)
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
                    new StatementSyntax[]
                    {
                        ExpressionStatement(
                            AssignmentExpression(
                                SyntaxKind.SimpleAssignmentExpression,
                                IdentifierName("Root"),
                                IdentifierName("root")
                            )
                        )
                    }.Concat(initializationStatements)
                )
            );
    }

    private static PropertyDeclarationSyntax CreateRootElementProperty()
    {
        return PropertyDeclaration(ParseTypeName(typeof(VisualElement).Name), Identifier("Root"))
            .AddModifiers(Token(SyntaxKind.PublicKeyword))
            .AddAccessorListAccessors(
                AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                    .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
            );
    }
}

#endif