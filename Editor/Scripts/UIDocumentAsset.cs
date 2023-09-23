#if UNITY_EDITOR
using Rosalina.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Diagnostics.DebuggerDisplay("{Name} ({Path})")]
internal class UIDocumentAsset
{
    /// <summary>
    /// Gets the UI Document name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the UI Document output path.
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the UI document full path.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the UI Document asset bindings output file.
    /// </summary>
    public string BindingsOutputFile { get; }

    /// <summary>
    /// Gets the UI Document UXML root node.
    /// </summary>
    public UxmlNode RootNode { get; }

    /// <summary>
    /// Gets a boolean value that indicates if the UI Document is an editor extension.
    /// </summary>
    public bool IsEditorExtension { get; }

    /// <summary>
    /// Creates a new <see cref="UIDocumentAsset"/> instance that represents a document to be generated.
    /// </summary>
    /// <param name="uiDocumentPath">UXML UI document file path.</param>
    /// <exception cref="ArgumentException">Thrown when the given file path is null, empty or with only white spaces.</exception>
    public UIDocumentAsset(string uiDocumentPath)
    {
        if (string.IsNullOrWhiteSpace(uiDocumentPath))
        {
            throw new ArgumentException($"'{nameof(uiDocumentPath)}' cannot be null or whitespace.", nameof(uiDocumentPath));
        }

        Name = System.IO.Path.GetFileNameWithoutExtension(uiDocumentPath);
        Path = System.IO.Path.GetDirectoryName(uiDocumentPath);
        FullPath = uiDocumentPath;
        RootNode = RosalinaUXMLParser.ParseUIDocument(FullPath) ?? throw new ArgumentNullException(nameof(RootNode), $"Failed to parse UXML file: '{FullPath}'");
        IsEditorExtension = Convert.ToBoolean(RootNode.Attributes.GetValueOrDefault("editor-extension-mode"));
        BindingsOutputFile = BuildOutputFile($"{Name}.g.cs");
    }

    /// <summary>
    /// Generates a C# script containing the bindings of the given UI document.
    /// </summary>
    public void GenerateBindings()
    {
        RosalinaFileSetting fileSetting = RosalinaSettings.instance.GetFileSetting(FullPath);

        if (fileSetting == null)
        {
            Debug.LogWarning($"Cannot find '{Name}' in Rosalina's configuration. Ensure that binding generation is enabled for this file. " +
                $"Right-Click on the UXML file > Rosalina > Properties... Then enable generation on Basic Settings.");
            return;
        }

        IRosalinaCodeGeneartor generator = fileSetting.Type switch
        {
            RosalinaGenerationType.Document => new RosalinaDocumentBindingsGenerator(),
            RosalinaGenerationType.Component => new RosalinaComponentBindingsGenerator(),
            RosalinaGenerationType.EditorWindow => new RosalinaEditorWindowBindingsGeneartor(),
            _ => throw new NotImplementedException()
        };

        Debug.Log($"[Rosalina]: Generating UI bindings for {FullPath}");
        ExecuteCodeGenerator(generator, BindingsOutputFile);
    }

    /// <summary>
    /// Geneartes a C# script for the UI logic.
    /// </summary>
    public void GenerateScript(string outputFile)
    {
        RosalinaFileSetting fileSetting = RosalinaSettings.instance.GetFileSetting(FullPath);

        if (fileSetting == null)
        {
            Debug.LogWarning($"Cannot find '{Name}' in Rosalina's configuration. Ensure that binding generation is enabled for this file. " +
                $"Right-Click on the UXML file > Rosalina > Properties... Then enable generation on Basic Settings.");
            return;
        }

        IRosalinaCodeGeneartor generator = fileSetting.Type switch
        {
            RosalinaGenerationType.Document => new RosalinaDocumentScriptGenerator(),
            RosalinaGenerationType.Component => new RosalinaComponentScriptGenerator(),
            RosalinaGenerationType.EditorWindow => new RosalinaEditorWindowScriptGenerator(),
            _ => throw new NotImplementedException()
        };

        Debug.Log($"[Rosalina]: Generating UI script for {FullPath}");
        ExecuteCodeGenerator(generator, outputFile);
    }

    /// <summary>
    /// Clears the bindings for the current UI Document.
    /// </summary>
    public void ClearBindings()
    {
        if (System.IO.File.Exists(BindingsOutputFile))
        {
            System.IO.File.Delete(BindingsOutputFile);
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Gets the UXML child nodes based on the Root node.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UxmlNode> GetChildren() => RootNode.Children.FlattenTree(x => x.Children).Where(x => x.HasName);

    private void ExecuteCodeGenerator(IRosalinaCodeGeneartor generator, string outputFile)
    {
        RosalinaGenerationResult result = generator.Generate(this);
        result.Save(outputFile);

        Debug.Log($"[Rosalina]: Done generating: {Name} (output: {outputFile})");
    }

    private string BuildOutputFile(string fileName)
    {
        string autogeneratedFolder = System.IO.Path.Combine("Assets", "Rosalina", "AutoGenerated");

        if (IsEditorExtension)
        {
            autogeneratedFolder = System.IO.Path.Combine(autogeneratedFolder, "Editor");
        }

        if (!System.IO.Directory.Exists(autogeneratedFolder))
        {
            System.IO.Directory.CreateDirectory(autogeneratedFolder);
        }

        return System.IO.Path.Combine(autogeneratedFolder, fileName);
    }
}
#endif