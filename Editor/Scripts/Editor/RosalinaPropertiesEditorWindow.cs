#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RosalinaPropertiesEditorWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset _visualTreeAsset;
    private RosalinaFileSetting _fileSettings = null;

    public VisualElement Container { get; private set; }

    public VisualElement BasicSettingsContainer { get; private set; }

    public Toggle EnableFile { get; private set; }

    public EnumField GeneratorTypeSelector { get; private set; }

    private void OnEnable()
    {
    }

    private void OnDestroy()
    {
        EnableFile.UnregisterValueChangedCallback(OnEnableFileChanged);
        GeneratorTypeSelector.UnregisterValueChangedCallback(OnGeneratorTypeSelectionChanged);
    }

    private void OnSelectionChange()
    {
        bool isActive = Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
        
        Container.SetEnabled(isActive);

        if (isActive)
        {
            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            _fileSettings = RosalinaSettings.instance.GetFileSetting(assetPath);
        }
        else
        {
            _fileSettings = null;
        }

        RefreshFileSettings();
    }

    private void CreateGUI()
    {
        rootVisualElement.Add(_visualTreeAsset.Instantiate());

        Container = rootVisualElement.Q<VisualElement>("Container");
        BasicSettingsContainer = rootVisualElement.Q<VisualElement>("BasicSettingsContainer");
        EnableFile = rootVisualElement.Q<Toggle>("EnableFile");
        GeneratorTypeSelector = rootVisualElement.Q<EnumField>("GeneratorTypeSelector");

        OnSelectionChange();
        EnableFile.RegisterValueChangedCallback(OnEnableFileChanged);
        GeneratorTypeSelector.RegisterValueChangedCallback(OnGeneratorTypeSelectionChanged);
    }

    private void RefreshFileSettings()
    {
        EnableFile.SetValueWithoutNotify(_fileSettings != null);
        BasicSettingsContainer.SetEnabled(_fileSettings != null);

        if (_fileSettings != null)
        {
            GeneratorTypeSelector.value = _fileSettings.Type;
        }
        else
        {
            GeneratorTypeSelector.value = null;
        }
    }

    private void OnEnableFileChanged(ChangeEvent<bool> @event)
    {
        if (@event.newValue)
        {
            _fileSettings = new RosalinaFileSetting
            {
                Path = AssetDatabase.GetAssetPath(Selection.activeObject),
                Type = RosalinaGenerationType.Document
            };
            RosalinaSettings.instance.Files.Add(_fileSettings);
        }
        else
        {
            RosalinaSettings.instance.Files.Remove(_fileSettings);
            _fileSettings = null;
        }

        RefreshFileSettings();
        OnSettingsChanged();
    }

    private void OnGeneratorTypeSelectionChanged(ChangeEvent<Enum> @event)
    {
        if (@event.newValue != null && @event.newValue != @event.previousValue)
        {
            _fileSettings.Type = (RosalinaGenerationType)@event.newValue;

            RefreshFileSettings();
            OnSettingsChanged();
        }
    }

    private void OnSettingsChanged()
    {
        RosalinaSettings.instance.Save();

    }

    [MenuItem("Assets/Rosalina/Properties...", validate = true)]
    public static bool ShowWindowValidation()
    {
        return RosalinaSettings.instance.IsEnabled && Selection.activeObject != null && Selection.activeObject.GetType() == typeof(VisualTreeAsset);
    }

    [MenuItem("Assets/Rosalina/Properties...")]
    public static void ShowWindow()
    {
        if (HasOpenInstances<RosalinaPropertiesEditorWindow>())
        {
            FocusWindowIfItsOpen<RosalinaPropertiesEditorWindow>();
            return;
        }

        Type inspectorWindowType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
        EditorWindow window = CreateWindow<RosalinaPropertiesEditorWindow>(inspectorWindowType);

        window.titleContent = new GUIContent("Rosalina", EditorGUIUtility.FindTexture("SettingsIcon"));
    }
}

#endif