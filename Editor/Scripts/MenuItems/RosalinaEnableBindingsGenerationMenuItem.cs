#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

public static class RosalinaEnableBindingsGenerationMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Enable binding generation";

    [MenuItem(MenuItemPath, true)]
    public static bool EnableBindingsGenerationValidation()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var file = RosalinaSettings.instance.GetFileSetting(assetPath);

        Menu.SetChecked(MenuItemPath, file != null);

        return RosalinaSettings.instance.IsEnabled && Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    [MenuItem(MenuItemPath, priority = 20)]
    public static void EnableBindingsGeneration()
    {
        string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
        RosalinaFileSetting file = RosalinaSettings.instance.GetFileSetting(assetPath);

        if (file == null)
        {
            RosalinaSettings.instance.Files.Add(new RosalinaFileSetting
            {
                Type = RosalinaGenerationType.Document,
                Path = assetPath
            });
        }
        else
        {
            RosalinaSettings.instance.Files.Remove(file);
        }
    }
}
#endif