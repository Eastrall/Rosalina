using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaGenerateCodeBehindMenuItem
{
    [MenuItem("Assets/Rosalina/Generate Code-Behind")]
    private static void GenerateUICodeBehind()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);

        try
        {
            EditorUtility.DisplayProgressBar("Rosalina", $"Generating {document.Name} code...", 50);

            Debug.Log($"[Rosalina]: Generating UI code behind for {assetPath}");
            RosalinaGenerator.Generate(document);
            Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {document.GeneratedFileOutputPath})");

            AssetDatabase.Refresh();
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

    [MenuItem("Assets/Rosalina/Generate Code-Behind", true)]
    private static bool GenerateUICodeBehindValidation()
    {
        return Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }
}
