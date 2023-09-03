#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

internal static class RosalinaStatementSyntaxFactory
{
    public static InitializationStatement[] GenerateInitializeStatements(UxmlDocument uxmlDocument, string methodAccessor)
    {
        var statements = new List<InitializationStatement>();
        IEnumerable<UIProperty> properties = uxmlDocument.GetChildren().Select(x => new UIProperty(x)).ToList();

        if (CheckForDuplicateProperties(properties))
        {
            throw new InvalidProgramException($"Failed to generate bindings for document: {uxmlDocument.Name} because of duplicate properties.");
        }

        foreach (UIProperty uiProperty in properties)
        {
            if (uiProperty.Type is null)
            {
                Debug.LogWarning($"[Rosalina]: Failed to get property type: '{uiProperty.TypeName}', field: '{uiProperty.Name}' for document '{uxmlDocument.Path}'. Property will be ignored.");
                continue;
            }

            PropertyDeclarationSyntax @property = PropertyDeclaration(ParseName(uiProperty.Type.Name), uiProperty.Name)
                .AddModifiers(Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                )
                .AddAccessorListAccessors(
                    AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .AddModifiers(Token(SyntaxKind.PrivateKeyword))
                        .WithSemicolonToken(Token(SyntaxKind.SemicolonToken))
                );

            ExpressionStatementSyntax statement;

            if (uiProperty.IsCustomComponent)
            {
                statement = ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(uiProperty.Name),
                        ObjectCreationExpression(IdentifierName(uiProperty.Type.Name))
                            .WithArgumentList(
                                ArgumentList(
                                    SingletonSeparatedList(
                                        Argument(
                                            GetQueryElementInvocation(methodAccessor, typeof(VisualElement).Name, uiProperty.OriginalName)
                                        )
                                    )
                                )
                            )
                    )
                );
            }
            else
            {
                statement = ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName(uiProperty.Name),
                        GetQueryElementInvocation(methodAccessor, uiProperty.Type.Name, uiProperty.OriginalName)
                    )
                );
            }

            if (statement != null)
            {
                statements.Add(new InitializationStatement(statement, property));
            }
        }

        return statements.ToArray();
    }

    private static bool CheckForDuplicateProperties(IEnumerable<UIProperty> properties)
    {
        var duplicatePropertyGroups = properties.GroupBy(x => x.Name).Where(g => g.Count() > 1);
        bool containsDuplicateProperties = duplicatePropertyGroups.Any();

        if (containsDuplicateProperties)
        {
            foreach (var property in duplicatePropertyGroups)
            {
                string duplicateProperties = string.Join(", ", property.Select(x => $"{x.OriginalName}"));

                Debug.LogError($"Conflict detected between {duplicateProperties}.");
            }
        }

        return containsDuplicateProperties;
    }

    private static InvocationExpressionSyntax GetQueryElementInvocation(string methodAccessor, string elementTypeName, string elementName)
    {
        return InvocationExpression(
            GenericName(methodAccessor)
                .WithTypeArgumentList(
                    TypeArgumentList(
                        SingletonSeparatedList<TypeSyntax>(
                            IdentifierName(elementTypeName)
                        )
                    )
                ),
            ArgumentList(
                SingletonSeparatedList(
                    Argument(
                        LiteralExpression(
                            SyntaxKind.StringLiteralExpression,
                            Literal(elementName)
                        )
                    )
                )
            ));
    }
}

#endif