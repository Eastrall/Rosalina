#if UNITY_EDITOR

using System;
using UnityEngine;

[Serializable]
public class RosalinaFileSetting
{
    [SerializeField]
    private RosalinaGenerationType _type;

    [SerializeField]
    private string _path;

    /// <summary>
    /// Gets or sets the generation type.
    /// </summary>
    public RosalinaGenerationType Type
    {
        get => _type;
        set => _type = value;
    }

    /// <summary>
    /// Gets or sets the asset path;
    /// </summary>
    public string Path
    {
        get => _path;
        set => _path = value;
    }
}

#endif