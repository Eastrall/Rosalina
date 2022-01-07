using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RosalinaAssetProcessor : AssetPostprocessor
{
    private const string UIDocumentExtension = ".uxml";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
    {
        string[] uiFilesChanged = importedAssets.Where(x => Path.GetExtension(x) == UIDocumentExtension).ToArray();

        if (uiFilesChanged.Length > 0)
        {
            for (int i = 0; i < uiFilesChanged.Length; i++)
            {
                RosalinaGenerator.Generate(uiFilesChanged[i]);
            }
        }
    }
}
