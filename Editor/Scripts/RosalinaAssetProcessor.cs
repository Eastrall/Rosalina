using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RosalinaAssetProcessor : AssetPostprocessor
{
    private const string UIDocumentExtension = ".uxml";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
    {
        string[] uiFilesChanged = importedAssets
            .Where(x => Path.GetExtension(x) == UIDocumentExtension)
            .ToArray();

        if (uiFilesChanged.Length > 0)
        {
            for (int i = 0; i < uiFilesChanged.Length; i++)
            {
                string uiDocumentPath = uiFilesChanged[i];
                var document = new UIDocumentAsset(uiDocumentPath);

                try
                {
                    EditorUtility.DisplayProgressBar("Rosalina", $"Generating {document.Name} bindings...", GeneratePercentage(i, uiFilesChanged.Length));
                    Debug.Log($"[Rosalina]: Generating UI bindings for {uiDocumentPath}");

                    RosalinaGenerationResult result = RosalinaGenerator.GenerateBindings(document, $"{document.Name}.g.cs");
                    result.Save();

                    Debug.Log($"[Rosalina]: Done generating: {document.Name} (output: {result.OutputFilePath})");
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

    private static int GeneratePercentage(int value, int total) => Math.Clamp((value / total) * 100, 0, 100);
}
