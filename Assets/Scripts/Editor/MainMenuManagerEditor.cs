using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainMenuManager))]
public class MainMenuManagerEditor : OdinEditor
{
    private SerializedProperty SP_applicationManager;
    private SerializedProperty SP_enables;
    private static bool _showEnableGroup;
    protected override void OnEnable()
    {
        SP_enables = serializedObject.FindProperty("_enables");
        SP_applicationManager = serializedObject.FindProperty("_applicationManager");
    }
    protected override void OnDisable()
    {
    }
    public void DrawEnables()
    {
        serializedObject.Update();

        var applicationManager = SP_applicationManager?.objectReferenceValue as ApplicationManager;
        var editions = applicationManager?.editionManager?.data;
        if (editions == null) return;
        SP_enables.arraySize = editions.Length;
        if (_showEnableGroup = EditorGUILayout.Foldout(_showEnableGroup, "Enable Editions"))
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < editions.Length; i++)
            {
                var SP_enable = SP_enables.GetArrayElementAtIndex(i);
                SP_enable.boolValue = EditorGUILayout.ToggleLeft(editions[i].englishName, SP_enable.boolValue);
            }
            EditorGUI.indentLevel--;
        }
        serializedObject.ApplyModifiedProperties();
    }
    public override void OnInspectorGUI()
    {
        DrawEnables();
        base.OnInspectorGUI();
    }
}
