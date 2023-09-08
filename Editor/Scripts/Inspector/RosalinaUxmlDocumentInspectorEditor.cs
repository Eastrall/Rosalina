#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(VisualTreeAsset), true)]
[CanEditMultipleObjects]
public class RosalinaUxmlDocumentInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        VisualTreeAsset visualTreeAsset = (VisualTreeAsset)target;

        string assetPath = AssetDatabase.GetAssetPath(visualTreeAsset.GetInstanceID());
        var file = RosalinaSettings.instance.GetFileSetting(assetPath);

        //EditorGUI.BeginDisabledGroup(false);
        EditorGUILayout.BeginVertical();
        EditorGUI.BeginChangeCheck();

        GUILayout.Space(20);
        EditorGUILayout.LabelField("Rosalina", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Enable");
        bool test = EditorGUILayout.Toggle(file != null);
        EditorGUILayout.EndHorizontal();

        if (file != null)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Code generation type");
            file.Type = (RosalinaGenerationType)EditorGUILayout.EnumPopup(file.Type);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.LinkButton("test");

        if (EditorGUI.EndChangeCheck())
        {

        }
        EditorGUILayout.EndVertical();
        //EditorGUI.EndDisabledGroup();
    }
}
#endif