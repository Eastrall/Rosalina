using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaGenerateCodeBehindMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Generate Code-Behind";

    [MenuItem(MenuItemPath, priority = 1)]
    private static void GenerateUICodeBehind()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);
        IRosalinaGenerator generator = new RosalinaBindingsGenerator();

        try
        {
            EditorUtility.DisplayProgressBar("Rosalina", $"Generating {document.Name} code...", 50);
            Debug.Log($"[Rosalina]: Generating UI code behind for {assetPath}");

            RosalinaGenerationResult result = generator.Generate(document, $"{document.Name}.g.cs");

            File.WriteAllText(result.OutputFilePath, result.Code);
            AssetDatabase.Refresh();

            Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");
        }
        catch (Exception e)
        {
            Debug.LogException(e, Selection.activeObject);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }

    [MenuItem(MenuItemPath, true)]
    private static bool GenerateUICodeBehindValidation()
    {
        return Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }
}
