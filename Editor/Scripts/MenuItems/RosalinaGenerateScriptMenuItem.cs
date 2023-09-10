#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaGenerateScriptMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Generate UI script";

    [MenuItem(MenuItemPath, true)]
    public static bool GenerateUIScriptValidation()
    {
        return RosalinaSettings.instance.IsEnabled && Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    [MenuItem(MenuItemPath, priority = 1102)]
    public static void GenerateUIScript()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);

        try
        {
            bool bingingsGenerated = TryGenerateBindings(document);
            bool scriptGenerated = TryGenerateScript(document);

            if (bingingsGenerated || scriptGenerated)
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

    private static bool TryGenerateBindings(UIDocumentAsset document)
    {
        if (!File.Exists(document.BindingsOutputFile) && AskGenerateBindings())
        {
            document.GenerateBindings();
            
            return true;
        }

        return false;
    }

    private static bool TryGenerateScript(UIDocumentAsset document)
    {
        string scriptPath = Path.Combine(document.Path, $"{document.Name}.cs");
        
        if (!File.Exists(scriptPath) || (File.Exists(scriptPath) && AskOverrideExistingScript()))
        {
            document.GenerateScript(scriptPath);

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
#endif