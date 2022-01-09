using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

internal class RosalinaUXMLParser
{
    public static UxmlNode ParseUIDocument(string uiDocumentPath)
    {
        using FileStream documentStream = File.OpenRead(uiDocumentPath);
        XElement root = XElement.Load(documentStream);

        return ParseUxmlNode(root);
    }

    private static UxmlNode ParseUxmlNode(XElement xmlNode)
    {
        string type = xmlNode.Name.LocalName;
        string name = xmlNode.Attribute("name")?.Value ?? string.Empty;

        UxmlNode node = new(type, name, xmlNode.Parent is null);

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
