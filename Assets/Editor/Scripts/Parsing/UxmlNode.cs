using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{Type} (name='{Name}')")]
internal class UxmlNode
{
    /// <summary>
    /// Gets the current UXML node type.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the UXML node name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets a boolean value that indicates if the current UXML node is the root node.
    /// </summary>
    public bool IsRoot { get; }

    /// <summary>
    /// Gets the UXML child nodes.
    /// </summary>
    public IList<UxmlNode> Children { get; } = new List<UxmlNode>();

    /// <summary>
    /// Gets a boolean value that indicates if the current UXML node has a name.
    /// </summary>
    public bool HasName => !string.IsNullOrEmpty(Name);

    /// <summary>
    /// Creates a new <see cref="UxmlNode"/> instance.
    /// </summary>
    /// <param name="type">Node type.</param>
    /// <param name="name">Node name.</param>
    /// <param name="isRoot">Is root node.</param>
    public UxmlNode(string type, string name, bool isRoot = false)
    {
        Type = type;
        Name = name;
        IsRoot = isRoot;
    }
}
