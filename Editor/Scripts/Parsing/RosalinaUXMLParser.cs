#if UNITY_EDITOR
using System;
using System.IO;
using System.Xml.Linq;

internal class RosalinaUXMLParser
{
    private const string ElementAttributeName = "name";
    private const string ElementAttributeTemplateName = "template";
    private const string EditorExtensionAttributeName = "editor-extension-mode";

    /// <summary>
    /// Parses the given UI document path.
    /// </summary>
    /// <param name="uiDocumentPath">UI Document path.</param>
    /// <returns>The UXML document.</returns>
    public static UxmlDocument ParseUIDocument(string uiDocumentPath)
    {
        using FileStream documentStream = File.OpenRead(uiDocumentPath);
        XElement root = XElement.Load(documentStream);
        bool isEditorExtension = Convert.ToBoolean(root.Attribute(EditorExtensionAttributeName)?.Value);
        UxmlNode rootNode = ParseUxmlNode(root);

        return new UxmlDocument(Path.GetFileName(uiDocumentPath), uiDocumentPath, rootNode, isEditorExtension);
    }

    private static UxmlNode ParseUxmlNode(XElement xmlNode)
    {
        string type = xmlNode.Name.LocalName;
        string name = xmlNode.Attribute(ElementAttributeName)?.Value ?? string.Empty;
        string template = xmlNode.Attribute(ElementAttributeTemplateName)?.Value ?? string.Empty;
        var node = new UxmlNode(type, name, xmlNode.Parent is null, template);

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
