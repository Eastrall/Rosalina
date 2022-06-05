#if UNITY_EDITOR
using Rosalina.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Represents a UXML template document.
/// </summary>
[DebuggerDisplay("{Name} (Path='{Path}')")]
internal class UxmlDocument
{
    /// <summary>
    /// Gets the document name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the document path.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets the document root node.
    /// </summary>
    public UxmlNode RootNode { get; }

    /// <summary>
    /// Creates a new <see cref="UxmlDocument"/> instance.
    /// </summary>
    /// <param name="name">Document name.</param>
    /// <param name="path">Document path.</param>
    /// <param name="rootNode">Document root node.</param>
    /// <exception cref="ArgumentException">Thrown when the 'name' or 'path' parameters are null or empty.</exception>
    public UxmlDocument(string name, string path, UxmlNode rootNode)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException($"'{nameof(path)}' cannot be null or empty.", nameof(path));
        }

        Name = name;
        Path = path;
        RootNode = rootNode;
    }

    /// <summary>
    /// Gets the UXML child nodes based on the Root node.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UxmlNode> GetChildren()
    {
        return RootNode.Children
            .FlattenTree(x => x.Children)
            .Where(x => x.HasName);
    }
}
#endif