using Sirenix.OdinInspector.Editor;

using UnityEditor;
using UnityEngine;

//using Tree = Sirenix.Utilities.Editor.

[CustomEditor(typeof(VideoManager))]
public class VideoManagerEditor : OdinEditor
{
    new VideoManager target => (VideoManager)base.target;

    private InspectorProperty _loadDataMethod_IP;
    private EasonGUITable<Video> _videoTable;
    private EasonGUITable<SurroundingVideoModel> _surroundingTable;
    private string[] sourcePropertyName = new[] { "_clip", "_fileName", "_assetName" };

    protected override void OnEnable()
    {
        base.OnEnable();

        _loadDataMethod_IP = Tree.GetPropertyAtPath("_loadDataMethod");
        _videoTable = new EasonGUITable<Video>(Tree, "_data", new[] { "_index", "_edition", "_staff", "", "_startTime" });
        _surroundingTable = new EasonGUITable<SurroundingVideoModel>(Tree, "_surroundingData", new[] { "_index", "_edition", "" });

        _videoTable.SetDisabled(0, true);
        _videoTable.SetFlexibles(3, true);

        _surroundingTable.SetDisabled(0, true);
        _surroundingTable.SetFlexibles(2, true);

        UpdatePropertyName();

        _videoTable.Initialize(target.data.Length, "Flat Videos");
        _surroundingTable.Initialize(target.surroundingData.Length, "Surrounding Videos");

    }


    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Tree.UpdateTree();

        _loadDataMethod_IP.Draw();
        UpdatePropertyName();
        _videoTable.DrawTable();
        _surroundingTable.DrawTable();

        Tree.ApplyChanges();

        serializedObject.ApplyModifiedProperties();
    }

    public void UpdatePropertyName()
    {
        _videoTable.SetPropertyName(3, sourcePropertyName[(int)target.loadDataMethod]);
        _surroundingTable.SetPropertyName(2, sourcePropertyName[(int)target.loadDataMethod]);
    }
}
