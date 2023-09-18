#if UNITY_EDITOR
using System;
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
            bool bingingsGenerated = RosalinaScriptGeneratorUtilities.TryGenerateBindings(document);
            bool scriptGenerated = RosalinaScriptGeneratorUtilities.TryGenerateScript(document);

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
}
#endif