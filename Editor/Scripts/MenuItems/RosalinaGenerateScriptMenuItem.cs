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
            if (TryGenerateBindings(document) || TryGenerateScript(document))
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
        return Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    private static bool TryGenerateBindings(UIDocumentAsset document)
    {
        string generatedBindingsScriptName = $"{document.Name}.g.cs";
        string generatedBindingsScriptPath = Path.Combine(document.Path, generatedBindingsScriptName);

        if (!File.Exists(generatedBindingsScriptPath) && AskGenerateBindings())
        {
            Debug.Log($"[Rosalina]: Generating UI code behind for {generatedBindingsScriptPath}");
            
            RosalinaGenerationResult result = RosalinaGenerator.GenerateBindings(document, generatedBindingsScriptName);
            result.Save();

            Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");
            
            return true;
        }

        return false;
    }

    private static bool TryGenerateScript(UIDocumentAsset document)
    {
        string scriptName = $"{document.Name}.cs";
        string scriptPath = Path.Combine(document.Path, scriptName);
        
        if (!File.Exists(scriptPath) || File.Exists(scriptPath) && AskOverrideExistingScript())
        {
            Debug.Log($"[Rosalina]: Generating UI script for {scriptPath}");

            RosalinaGenerationResult result = RosalinaGenerator.GenerateScript(document, scriptName);
            result.Save();

            Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");

            return true;
        }

        return false;
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
