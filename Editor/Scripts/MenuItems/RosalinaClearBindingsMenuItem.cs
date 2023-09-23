#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaClearBindingsMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Clear Bindings";

    [MenuItem(MenuItemPath, true)]
    public static bool ClearBindingsValidation()
    {
        return RosalinaSettings.instance.IsEnabled && Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    [MenuItem(MenuItemPath, priority = 1100)]
    public static void ClearBindings()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var document = new UIDocumentAsset(assetPath);

        try
        {
            document.ClearBindings();
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