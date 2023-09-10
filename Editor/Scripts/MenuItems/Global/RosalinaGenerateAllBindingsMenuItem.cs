#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RosalinaGenerateAllBindingsMenuItem
{
    private const string MenuItemPath = "Assets/Rosalina/Generate All UI Bindings";

    [MenuItem(MenuItemPath, true)]
    public static bool GenerateAllBindingsValidation()
    {
        return RosalinaSettings.instance.IsEnabled;
    }

    [MenuItem(MenuItemPath, priority = 1000)]
    public static void GenerateAllBindings()
    {
        try
        {
            string[] uiDocumentFiles = Directory.EnumerateFiles("Assets", "*.uxml", SearchOption.AllDirectories).ToArray();

            for (int i = 0; i < uiDocumentFiles.Length; i++)
            {
                string uiDocumentFile = uiDocumentFiles[i];
                var document = new UIDocumentAsset(uiDocumentFile);
                RosalinaEditorUtilities.ShowProgressBar("Rosalina", "Generating bindings...", i, uiDocumentFiles.Length);

                document.GenerateBindings();
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