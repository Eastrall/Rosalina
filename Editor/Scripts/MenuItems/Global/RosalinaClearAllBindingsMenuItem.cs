#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RosalinaClearAllBindingsMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Clear All Bindings";

    [MenuItem(MenuItemPath, true)]
    public static bool ClearAllBindingsValidation()
    {
        return RosalinaSettings.instance.IsEnabled;
    }

    [MenuItem(MenuItemPath, priority = 1001)]
    public static void ClearAllBindings()
    {
        try
        {
            string[] uiBindingFiles = Directory.EnumerateFiles("Assets/Rosalina", "*.g.cs*", SearchOption.AllDirectories).ToArray();

            for (int i = 0; i < uiBindingFiles.Length; i++)
            {
                string bindingFile = uiBindingFiles[i];
                RosalinaEditorUtilities.ShowProgressBar("Rosalina", "Clearing bindings...", i, uiBindingFiles.Length);

                if (File.Exists(bindingFile))
                {
                    File.Delete(bindingFile);
                }
            }

            Debug.Log($"[Rosalina]: Bindings cleared.");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        finally
        {
            RosalinaEditorUtilities.HideProgressBar();
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }
    }
}

#endif