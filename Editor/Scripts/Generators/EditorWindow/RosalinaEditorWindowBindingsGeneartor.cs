#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal class RosalinaEditorWindowBindingsGeneartor : IRosalinaCodeGeneartor
{
    private const string OnCreateGUIHookName = "OnCreateGUI";

    public RosalinaGenerationResult Generate(UIDocumentAsset document)
    {
        if (document is null)
        {
            throw new ArgumentNullException(nameof(document), "Cannot generate binding with a null document asset.");
        }

        InitializationStatement[] statements = RosalinaStatementSyntaxFactory.GenerateInitializeStatements(document, $"rootVisualElement?.Q");
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
                    .AddBaseListTypes(
                        SimpleBaseType(
                            ParseName(nameof(EditorWindow))
                        )
                    )
                    .AddMembers(propertyStatements)
                    .AddMembers(
                        // public void CreateGUI() { ... }
                        MethodDeclaration(ParseTypeName("void"), "CreateGUI")
                            .AddModifiers(Token(SyntaxKind.PublicKeyword))
                            .WithBody(
                                Block(
                                    new[]
                                    {
                                         CreateVisualTreeAssetVariable(document.FullPath),
                                         CreateVisualElementVariable(),
                                         AddVisualElementToRootElement()
                                    }
                                    .Concat(initializationStatements)
                                    .Append(CallOnCreateGUIMethod())
                                )
                            ),

                        // partial void OnCreateGUI();
                        MethodDeclaration(ParseTypeName("void"), OnCreateGUIHookName)
                            .AddModifiers(Token(SyntaxKind.PartialKeyword))
                            .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                    )
            );

        string code = compilationUnit
            .NormalizeWhitespace()
            .ToFullString();

        return new RosalinaGenerationResult(code);
    }

    private static MemberAccessExpressionSyntax CreateRootQueryMethodAccessor()
    {
        return
            MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName("rootVisualElement?"),
                Token(SyntaxKind.DotToken),
                IdentifierName("Q")
            );
    }

    private static StatementSyntax CreateVisualTreeAssetVariable(string documentAssetPath)
    {
        return LocalDeclarationStatement(
            VariableDeclaration(
                IdentifierName(nameof(VisualTreeAsset))
            )
            .WithVariables(
                SingletonSeparatedList(
                        VariableDeclarator("asset")
                        .WithInitializer(
                            EqualsValueClause(
                                InvocationExpression(
                                    MemberAccessExpression(
                                        SyntaxKind.SimpleMemberAccessExpression,
                                        IdentifierName(nameof(AssetDatabase)),
                                        GenericName(nameof(AssetDatabase.LoadAssetAtPath))
                                        .WithTypeArgumentList(
                                            TypeArgumentList(
                                                SingletonSeparatedList<TypeSyntax>(
                                                    IdentifierName(nameof(VisualTreeAsset))
                                                )
                                            )
                                        )
                                    )
                                 )
                                 .WithArgumentList(
                                     ArgumentList(
                                         SingletonSeparatedList(
                                             Argument(
                                                 LiteralExpression(
                                                     SyntaxKind.StringLiteralExpression,
                                                     Literal(documentAssetPath)
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
        return
            LocalDeclarationStatement(
                VariableDeclaration(
                    IdentifierName(nameof(VisualElement))
                )
                .WithVariables(
                    SingletonSeparatedList(

                            VariableDeclarator("ui")
                            .WithInitializer(
                                EqualsValueClause(
                                    InvocationExpression(
                                        MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            IdentifierName("asset"),
                                            IdentifierName(nameof(VisualTreeAsset.CloneTree))
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
        return
            ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName("rootVisualElement"),
                        IdentifierName("Add")
                    )
                )
                .WithArgumentList(
                    ArgumentList(
                        SingletonSeparatedList(
                            Argument(
                                IdentifierName("ui")
                            )
                        )
                    )
                )
            );
    }

    private static StatementSyntax CallOnCreateGUIMethod()
    {
        return
            ExpressionStatement(
                InvocationExpression(
                    IdentifierName(OnCreateGUIHookName)
                )
            );
    }
}

#endif