#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaGenerateBindingsMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Generate UI bindings";

    [MenuItem(MenuItemPath, true)]
    private static bool GenerateUIBindingsValidation()
    {
        return RosalinaSettings.instance.IsEnabled && Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    [MenuItem(MenuItemPath, priority = 1101)]
    private static void GenerateUIBindings()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);

        try
        {
            EditorUtility.DisplayProgressBar("Rosalina", $"Generating {document.Name} UI bindings...", 50);

            document.GenerateBindings();
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
}
#endif