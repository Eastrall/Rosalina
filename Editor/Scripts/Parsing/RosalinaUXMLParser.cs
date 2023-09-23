#if UNITY_EDITOR
using System.IO;
using System.Xml.Linq;

internal sealed class RosalinaUXMLParser
{
    /// <summary>
    /// Parses the given UI document path.
    /// </summary>
    /// <param name="uiDocumentPath">UI Document path.</param>
    /// <returns>The UXML root node.</returns>
    public static UxmlNode ParseUIDocument(string uiDocumentPath)
    {
        using FileStream documentStream = File.OpenRead(uiDocumentPath);
        XElement root = XElement.Load(documentStream);
        UxmlNode rootNode = ParseUxmlNode(root);

        return rootNode;
    }

    private static UxmlNode ParseUxmlNode(XElement xmlNode)
    {
        var node = new UxmlNode(xmlNode);

        if (xmlNode.HasElements)
        {
            foreach (XElement xmlElement in xmlNode.Elements())
            {
                node.Children.Add(ParseUxmlNode(xmlElement));
            }
        }

        return node;
    }
}
#endif
