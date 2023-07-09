#if UNITY_EDITOR
using System;
using System.Collections.Generic;
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
        _settings = RosalinaSettings.Current != null ? RosalinaSettings.Current : CreateRosalinaSettingsAsset();
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

    public static RosalinaSettings CreateRosalinaSettingsAsset()
    {
        string resourcePath = "Assets/Rosalina";

        if (!AssetDatabase.IsValidFolder(resourcePath))
        {
            AssetDatabase.CreateFolder("Assets", "Rosalina");
        }

        RosalinaSettings newSettings = ScriptableObject.CreateInstance<RosalinaSettings>();
        AssetDatabase.CreateAsset(newSettings, $"{resourcePath}/RosalinaSettings.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return RosalinaSettings.Current;
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