#if UNITY_EDITOR

using Rosalina.Extensions;
using System;
using System.Diagnostics;
using UnityEngine.UIElements;

/// <summary>
/// Describes a UI property in a UXML file.
/// </summary>
[DebuggerDisplay("{TypeName} {Name} (uxml: {OriginalName})")]
internal readonly struct UIProperty
{
    /// <summary>
    /// Gets the UI property type name as described in the UXML file.
    /// </summary>
    public string TypeName { get; }

    /// <summary>
    /// Gets the UI property type as described in the UXML file.
    /// </summary>
    public Type Type { get; }

    /// <summary>
    /// Gets the UI property name that is used for generating C# properties during the code generation process.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the UI property name as described in the UXML file.
    /// </summary>
    public string OriginalName { get; }

    /// <summary>
    /// Gets a boolean value that indicates if the UI property represents a custom component.
    /// </summary>
    public bool IsCustomComponent => TypeName == "Instance";

    /// <summary>
    /// Gets the custom component template name.
    /// </summary>
    public string TemplateName { get; }

    public UIProperty(UxmlNode uxmlNode)
    {
        TypeName = uxmlNode.Type;
        Type = UIPropertyTypes.GetUIElementType(uxmlNode.Type) ?? UIPropertyTypes.GetCustomUIElementType(uxmlNode.Template);
        OriginalName = uxmlNode.Name;
        Name = uxmlNode.Name.Contains('-') ? uxmlNode.Name.ToPascalCase() : OriginalName;
        TemplateName = uxmlNode.Template;
    }
}

#endif