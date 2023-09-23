#if UNITY_EDITOR
using System.IO;
using UnityEditor;

/// <summary>
/// Provides utility methods for the scripting generation.
/// </summary>
internal static class RosalinaScriptGeneratorUtilities
{
    public static bool TryGenerateBindings(UIDocumentAsset document)
    {
        if (!File.Exists(document.BindingsOutputFile) && AskGenerateBindings())
        {
            document.GenerateBindings();

            return true;
        }

        return false;
    }

    public static bool TryGenerateScript(UIDocumentAsset document)
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