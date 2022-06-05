#if UNITY_EDITOR

/// <summary>
/// Describes an UXML UI property.
/// </summary>
internal class UIPropertyDescriptor
{
    /// <summary>
    /// Gets the UI property type as described in the UXML file.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the UI property name as described in the UXML file.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the UI property private name.
    /// </summary>
    /// <remarks>
    /// This property is only used in code generation process.
    /// </remarks>
    public string PrivateName { get; }

    /// <summary>
    /// Creates a new <see cref="UIPropertyDescriptor"/> instance.
    /// </summary>
    /// <param name="type">Property type.</param>
    /// <param name="name">Property name.</param>
    public UIPropertyDescriptor(string type, string name)
    {
        Type = type;
        Name = name;
        PrivateName = $"_{char.ToLowerInvariant(name[0])}{name[1..]}";
    }
}

#endif