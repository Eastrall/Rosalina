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
    public static RosalinaSettings Current => RosalinaSettingsProvider.GetOrCreateSettings();

    [SerializeField]
    private bool _isEnabled;

    /// <summary>
    /// Gets or sets a boolean value that indicates if Rosalina is enabled.
    /// </summary>
    public bool IsEnabled
    {
        get => _isEnabled;
        set => _isEnabled = value;
    }
}

#endif