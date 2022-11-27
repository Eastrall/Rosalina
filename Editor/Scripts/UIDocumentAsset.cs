#if UNITY_EDITOR
using System;

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
    /// Gets the Uxml document.
    /// </summary>
    public UxmlDocument UxmlDocument { get; }

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
        UxmlDocument = RosalinaUXMLParser.ParseUIDocument(FullPath) ?? throw new ArgumentNullException(nameof(UxmlDocument), $"Failed to parse UXML file: '{FullPath}'");
    }
}
#endif