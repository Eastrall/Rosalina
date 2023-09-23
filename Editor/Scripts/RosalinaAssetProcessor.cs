#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RosalinaAssetProcessor : AssetPostprocessor
{
    private static readonly string UIDocumentExtension = ".uxml";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
    {
        if (!RosalinaSettings.instance.IsEnabled)
        {
            return;
        }

        string[] uiFilesChanged = importedAssets
            .Where(assetPath => assetPath.StartsWith("Assets") && RosalinaSettings.instance.ContainsFile(assetPath))
            .Where(assetPath => Path.GetExtension(assetPath) == UIDocumentExtension)
            .ToArray();

        if (uiFilesChanged.Length > 0)
        {
            for (int i = 0; i < uiFilesChanged.Length; i++)
            {
                string uiDocumentPath = uiFilesChanged[i];
                var document = new UIDocumentAsset(uiDocumentPath);

                try
                {
                    RosalinaEditorUtilities.ShowProgressBar("Rosalina", $"Generating {document.Name} bindings...", i, uiFilesChanged.Length);

                    document.GenerateBindings();
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }

            Debug.Log($"[Rosalina]: Done.");
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }
    }
}
#endif