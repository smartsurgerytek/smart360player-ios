using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MainMenuSceneManager))]
public class MainMenuSceneManagerEditor : OdinEditor
{
    //private SerializedProperty SP_applicationManager;
    //private SerializedProperty SP_enables;
    //private static bool _showEnableGroup;
    protected override void OnEnable()
    {
        //SP_enables = serializedObject.FindProperty("_enables");
        //SP_applicationManager = serializedObject.FindProperty("_applicationManager");
    }
    protected override void OnDisable()
    {
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
