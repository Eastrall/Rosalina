using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaGenerateBindingsMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Generate UI bindings";

    [MenuItem(MenuItemPath, priority = 1)]
    private static void GenerateUIBindings()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);

        try
        {
            EditorUtility.DisplayProgressBar("Rosalina", $"Generating {document.Name} UI bindings...", 50);
            Debug.Log($"[Rosalina]: Generating UI bindings for {assetPath}");

            RosalinaGenerationResult result = RosalinaGenerator.GenerateBindings(document, $"{document.Name}.g.cs");
            result.Save();

            AssetDatabase.Refresh();

            Debug.Log($"[Rosalina]: Done generating UI bindings: {document.Name} (output: {result.OutputFilePath})");
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
    private static bool GenerateUIBindingsValidation()
    {
        return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }
}
