using System;
using System.IO;

internal class UIDocumentAsset
{
    /// <summary>
    /// Gets the UI Document name.
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Gets the UI Document generated file name.
    /// </summary>
    public string GeneratedName { get; }

    /// <summary>
    /// Gets the UI document full path.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the UI Document output path.
    /// </summary>
    public string OutputPath { get; }

    /// <summary>
    /// Gets the generated file full path.
    /// </summary>
    public string GeneratedFileOutputPath { get; }

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

        Name = Path.GetFileNameWithoutExtension(uiDocumentPath);
        GeneratedName = $"{Name}.g.cs";
        FullPath = uiDocumentPath;
        OutputPath = Path.GetDirectoryName(uiDocumentPath);
        GeneratedFileOutputPath = Path.Combine(OutputPath, GeneratedName);
    }
}
