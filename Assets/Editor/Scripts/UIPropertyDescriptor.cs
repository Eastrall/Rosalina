internal class UIPropertyDescriptor
{
    public string Type { get; set; }

    public string Name { get; set; }

    public string PrivateName { get; }

    public UIPropertyDescriptor(string type, string name)
    {
        Type = type;
        Name = name;
        PrivateName = $"_{char.ToLowerInvariant(name[0])}{name[1..]}";
    }
}
