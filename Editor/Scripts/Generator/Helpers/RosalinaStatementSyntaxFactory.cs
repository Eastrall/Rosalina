#if UNITY_EDITOR

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

internal static class RosalinaStatementSyntaxFactory
{
    public static InitializationStatement[] GenerateInitializeStatements(UxmlDocument uxmlDocument, MemberAccessExpressionSyntax documentQueryMethodAccess)
    {
        var statements = new List<InitializationStatement>();
        IEnumerable<UIProperty> properties = uxmlDocument.GetChildren().Select(x => new UIProperty(x.Type, x.Name)).ToList();

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

            PropertyDeclarationSyntax @property = RosalinaSyntaxFactory.CreateProperty(uiProperty.Type.Name, uiProperty.Name, SyntaxKind.PublicKeyword)
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                )
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword))
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                );

            var argumentList = SyntaxFactory.SeparatedList(new[]
            {
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(uiProperty.OriginalName)
                    )
                )
            });
            var cast = SyntaxFactory.CastExpression(
                SyntaxFactory.ParseTypeName(uiProperty.Type.Name),
                SyntaxFactory.InvocationExpression(documentQueryMethodAccess, SyntaxFactory.ArgumentList(argumentList))
            );
            var statement = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName(uiProperty.Name),
                    cast
                )
            );

            statements.Add(new InitializationStatement(statement, property));
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
}

#endif