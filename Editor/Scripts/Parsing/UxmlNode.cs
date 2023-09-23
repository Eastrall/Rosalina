#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

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
    /// Gets or sets if the current node is a template.
    /// </summary>
    public bool IsTemplate => Type == "Template";

    /// <summary>
    /// Gets the UXML template name in case of custom component; null otherwise.
    /// </summary>
    public string Template { get; }

    /// <summary>
    /// Gets the UXML child nodes.
    /// </summary>
    public IList<UxmlNode> Children { get; } = new List<UxmlNode>();

    /// <summary>
    /// Gets a boolean value that indicates if the current UXML node has a name.
    /// </summary>
    public bool HasName => !string.IsNullOrEmpty(Name);

    /// <summary>
    /// Gets the node attributes.
    /// </summary>
    public IReadOnlyDictionary<string, string> Attributes { get; }

    /// <summary>
    /// Creates a new <see cref="UxmlNode"/> based on the given XML node.
    /// </summary>
    /// <param name="xmlNode">Current XML node.</param>
    public UxmlNode(XElement xmlNode)
    {
        Type = xmlNode.Name.LocalName;
        Name = xmlNode.Attribute("name")?.Value ?? string.Empty;
        IsRoot = xmlNode.Parent == null;
        Template = xmlNode.Attribute("template")?.Value ?? string.Empty;
        Attributes = xmlNode.Attributes().ToDictionary(x => x.Name.LocalName, x => x.Value);
    }
}
#endif