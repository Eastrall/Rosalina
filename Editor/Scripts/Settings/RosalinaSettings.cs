#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
/// <summary>
/// Provides a data structure for Rosalina settings.
/// </summary>
public class RosalinaSettings : ScriptableObject
{
    /// <summary>
    /// Gets the current Rosalina settings.
    /// </summary>
    public static RosalinaSettings Current => AssetDatabase.LoadAssetAtPath<RosalinaSettings>("Assets/Rosalina/RosalinaSettings.asset");

    [SerializeField]
    private bool _isEnabled;

    [SerializeField]
    private string _defaultNamespace;

    /// <summary>
    /// Gets or sets a boolean value that indicates if Rosalina is enabled.
    /// </summary>
    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }

    /// <summary>
    /// Gets or sets the default namespace to use during code generation.
    /// </summary>
    public string DefaultNamespace
    {
        get => _defaultNamespace;
        set => _defaultNamespace = value;
    }
}

#endif