using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using UnityEditor;

internal class RosalinaGenerator
{
    /// <summary>
    /// Generates the UI document code behind.
    /// </summary>
    /// <param name="uiDocumentPath">UI Document path.</param>
    public void Generate(string uiDocumentPath)
    {
        EditorUtility.DisplayProgressBar("Generating UI code behind", "Working...", 0);
        string outputPath = Path.GetDirectoryName(uiDocumentPath);
        string className = Path.GetFileNameWithoutExtension(uiDocumentPath);
        string generatedCodeBehindFileName = Path.Combine(outputPath, $"{className}.g.cs");

        ClassDeclarationSyntax @class = SyntaxFactory.ClassDeclaration(className)
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PartialKeyword));

        string code = @class
            .NormalizeWhitespace()
            .ToFullString();

        File.WriteAllText(generatedCodeBehindFileName, code);

        EditorUtility.ClearProgressBar();
    }
}
