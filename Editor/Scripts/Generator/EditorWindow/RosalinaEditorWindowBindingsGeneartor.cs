#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

internal class RosalinaEditorWindowBindingsGeneartor : IRosalinaGeneartor
{
    private const string OnCreateGUIHookName = "OnCreateGUI";

    public RosalinaGenerationResult Generate(UIDocumentAsset documentAsset)
    {
        if (documentAsset is null)
        {
            throw new ArgumentNullException(nameof(documentAsset), "Cannot generate binding with a null document asset.");
        }

        InitializationStatement[] statements = RosalinaStatementSyntaxFactory.GenerateInitializeStatements(documentAsset.UxmlDocument, CreateRootQueryMethodAccessor());
        PropertyDeclarationSyntax[] propertyStatements = statements.Select(x => x.Property).ToArray();
        StatementSyntax[] initializationStatements = statements.Select(x => x.Statement).ToArray();

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
                    .AddMembers(propertyStatements)
                    .AddMembers(
                        // public void CreateGUI() { ... }
                        RosalinaSyntaxFactory
                            .CreateMethod("void", UnityConstants.CreateGUIHookName, SyntaxKind.PublicKeyword)
                            .WithBody(
                                SyntaxFactory.Block(
                                    new[]
                                    {
                                         CreateVisualTreeAssetVariable(documentAsset.FullPath),
                                         CreateVisualElementVariable(),
                                         AddVisualElementToRootElement()
                                    }
                                    .Concat(initializationStatements)
                                    .Append(CallOnCreateGUIMethod())
                                )
                            ),

                        // partial void OnCreateGUI();
                        RosalinaSyntaxFactory
                            .CreateMethod("void", OnCreateGUIHookName, SyntaxKind.PartialKeyword)
                            .WithSemicolonToken(
                                SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                            )
                    )
            );

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(RosalinaGeneratorConstants.GeneratedCodeHeader + code);
    }

    private static MemberAccessExpressionSyntax CreateRootQueryMethodAccessor()
    {
        return SyntaxFactory
            .MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName($"{UnityConstants.DocumentRootVisualElementFieldName}?"),
                SyntaxFactory.Token(SyntaxKind.DotToken),
                SyntaxFactory.IdentifierName(UnityConstants.RootVisualElementQueryMethodName)
            );
    }

    private static StatementSyntax CreateVisualTreeAssetVariable(string documentAssetPath)
    {
        return SyntaxFactory.LocalDeclarationStatement(
            SyntaxFactory.VariableDeclaration(
                SyntaxFactory.IdentifierName(nameof(VisualTreeAsset))
            )
            .WithVariables(
                SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                    SyntaxFactory
                        .VariableDeclarator("asset")
                        .WithInitializer(
                            SyntaxFactory.EqualsValueClause(
                                SyntaxFactory.InvocationExpression(
                                    SyntaxFactory.MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        SyntaxFactory.IdentifierName(nameof(AssetDatabase)),
                                        SyntaxFactory.GenericName(nameof(AssetDatabase.LoadAssetAtPath))
                                        .WithTypeArgumentList(
                                            SyntaxFactory.TypeArgumentList(
                                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                                    SyntaxFactory.IdentifierName(nameof(VisualTreeAsset))
                                                )
                                            )
                                        )
                                    )
                                 )
                                 .WithArgumentList(
                                     SyntaxFactory.ArgumentList(
                                         SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                                             SyntaxFactory.Argument(
                                                 SyntaxFactory.LiteralExpression(
                                                     SyntaxKind.StringLiteralExpression,
                                                     SyntaxFactory.Literal(documentAssetPath)
                                                 )
                                             )
                                         )
                                     )
                                 )
                            )
                        )
                )
            )
        );
    }

    private static StatementSyntax CreateVisualElementVariable()
    {
        return SyntaxFactory
            .LocalDeclarationStatement(
                SyntaxFactory.VariableDeclaration(
                    SyntaxFactory.IdentifierName(nameof(VisualElement))
                )
                .WithVariables(
                    SyntaxFactory.SingletonSeparatedList<VariableDeclaratorSyntax>(
                        SyntaxFactory
                            .VariableDeclarator("ui")
                            .WithInitializer(
                                SyntaxFactory.EqualsValueClause(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.IdentifierName("asset"),
                                            SyntaxFactory.IdentifierName(nameof(VisualTreeAsset.CloneTree))
                                        )
                                    )
                                )
                            )
                    )
                )
            );
    }

    private static StatementSyntax AddVisualElementToRootElement()
    {
        return SyntaxFactory
            .ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName(UnityConstants.DocumentRootVisualElementFieldName),
                        SyntaxFactory.IdentifierName("Add")
                    )
                )
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList<ArgumentSyntax>(
                            SyntaxFactory.Argument(
                                SyntaxFactory.IdentifierName("ui")
                            )
                        )
                    )
                )
            );
    }

    private static StatementSyntax CallOnCreateGUIMethod()
    {
        return SyntaxFactory
            .ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.IdentifierName(OnCreateGUIHookName)
                )
            );
    }
}

#endif