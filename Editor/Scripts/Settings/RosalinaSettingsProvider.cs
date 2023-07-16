#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaSettingsProvider : SettingsProvider
{
    private RosalinaSettings _settings;

    public RosalinaSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) 
        : base(path, scopes, keywords)
    {
    }

    public override void OnActivate(string searchContext, VisualElement rootElement)
    {
        _settings = RosalinaSettings.Current;
    }

    public override void OnGUI(string searchContext)
    {
        using (CreateSettingsWindowGUIScope())
        {
            EditorGUI.BeginChangeCheck();

            _settings.IsEnabled = EditorGUILayout.Toggle("Is Enabled", _settings.IsEnabled);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_settings);
            }
        }
    }

    public static RosalinaSettings GetOrCreateSettings()
    {
        string resourcePath = "Assets/Rosalina";
        string settingsFileName = "RosalinaSettings.asset";
        string settingsFilePath = Path.Combine(resourcePath, settingsFileName);

        if (!AssetDatabase.IsValidFolder(resourcePath))
        {
            AssetDatabase.CreateFolder("Assets", "Rosalina");
        }

        if (File.Exists(settingsFilePath))
        {
            return AssetDatabase.LoadAssetAtPath<RosalinaSettings>(settingsFilePath);
        }
        else
        {
            RosalinaSettings newSettings = ScriptableObject.CreateInstance<RosalinaSettings>();
            AssetDatabase.CreateAsset(newSettings, settingsFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return newSettings;
        }
    }

    [SettingsProvider]
    public static SettingsProvider CreateRosalinaSettingsProvider() => new RosalinaSettingsProvider("Project/Rosalina", SettingsScope.Project);

    private static IDisposable CreateSettingsWindowGUIScope()
    {
        var unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
        var type = unityEditorAssembly.GetType("UnityEditor.SettingsWindow+GUIScope");

        return Activator.CreateInstance(type) as IDisposable;
    }
}

#endif