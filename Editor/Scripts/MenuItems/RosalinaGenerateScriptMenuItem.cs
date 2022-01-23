using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaGenerateScriptMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Generate UI script";

    [MenuItem(MenuItemPath, priority = 0)]
    public static void GenerateUIScript()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);

        try
        {
            bool refreshAssetDatabase = false;
            string scriptName = $"{document.Name}.cs";
            string scriptPath = Path.Combine(document.Path, scriptName);
            string generatedBindingsScriptName = $"{document.Name}.g.cs";
            string generatedBindingsScriptPath = Path.Combine(document.Path, generatedBindingsScriptName);

            if (!File.Exists(generatedBindingsScriptPath) && AskGenerateBindings())
            {
                Debug.Log($"[Rosalina]: Generating UI code behind for {generatedBindingsScriptPath}");
                RosalinaGenerationResult result = new RosalinaBindingsGenerator().Generate(document, generatedBindingsScriptName);

                File.WriteAllText(result.OutputFilePath, result.Code);
                Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");
                refreshAssetDatabase = true;
            }

            if (!File.Exists(scriptPath) || File.Exists(scriptPath) && AskOverrideExistingScript())
            {
                Debug.Log($"[Rosalina]: Generating UI script for {scriptPath}");
                RosalinaGenerationResult result = new RosalinaScriptGenerator().Generate(document, scriptName);

                File.WriteAllText(result.OutputFilePath, result.Code);
                Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");
                refreshAssetDatabase = true;
            }

            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh();
            }
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
    public static bool GenerateUIScriptValidation()
    {
        return Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    private static bool AskOverrideExistingScript()
    {
        return EditorUtility.DisplayDialog("Overwrite existing script", 
            "A UI script already exists for this UI Document. Do you want to overwrite the script?",
            "Yes", 
            "No");
    }

    private static bool AskGenerateBindings()
    {
        return EditorUtility.DisplayDialog("UI Bindings mising",
            "The UI bindings for this UI Document are missing. Do you want to generate bindings?",
            "Yes",
            "No");
    }
}
