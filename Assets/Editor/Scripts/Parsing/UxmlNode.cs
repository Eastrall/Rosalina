using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{Type} (name='{Name}')")]
internal class UxmlNode
{
    public string Type { get; set; }

    public string Name { get; set; }

    public bool IsRoot { get; set; }

    public IList<UxmlNode> Children { get; } = new List<UxmlNode>();

    public bool HasName => !string.IsNullOrEmpty(Name);

    public UxmlNode(string type, string name, bool isRoot = false)
    {
        Type = type;
        Name = name;
        IsRoot = isRoot;
    }
}
