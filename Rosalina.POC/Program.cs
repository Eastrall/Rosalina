// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Xml.Linq;

using var stream = File.OpenRead("document.xml");
var root = XElement.Load(stream);
var parsedFile = ParseFile(root);
DisplayTree(parsedFile);

static Node ParseFile(XElement xmlNode)
{
    string type = xmlNode.Name.LocalName;
    string name = xmlNode.Attribute("name")?.Value ?? string.Empty;

    Node node = new(type, name, xmlNode.Parent is null);

    if (xmlNode.HasElements)
    {
        foreach (XElement xmlElement in xmlNode.Elements())
        {
            node.Children.Add(ParseFile(xmlElement));
        }
    }

    return node;
}

static void DisplayTree(Node node, int level = 0)
{
    if (level > 0)
    {
        for (int i = 0; i < level; i++)
        {
            Console.Write("-");
        }
        Console.Write(' ');
    }

    Console.WriteLine($"{node.Type} (name = '{node.Name}')");

    if (node.Children.Any())
    {
        foreach (Node childNode in node.Children)
        {
            DisplayTree(childNode, level + 2);
        }
    }
}

[DebuggerDisplay("{Type} (name='{Name}')")]
public class Node
{
    public bool IsRoot { get; }

    public string Type { get; }

    public string Name { get; }

    public IList<Node> Children { get; } = new List<Node>();

    public bool HasChildren => Children.Any();

    public Node(string type, string name, bool isRoot = false)
    {
        Type = type;
        Name = name;
        IsRoot = isRoot;
    }
}