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

    public UIProperty(string type, string name)
    {
        TypeName = type;
        Type = UIPropertyTypes.GetUIElementType(type);
        OriginalName = name;
        Name = name.Contains('-') ? name.ToPascalCase() : OriginalName;
    }
}

#endif