#if UNITY_EDITOR
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
public class PersistentDataPathOpener
{
    [MenuItem("Assets/Open Persistent Data Folder",priority =1100)]
    private static void OpenPersistentDataFolder()
    {
        Process.Start(Application.persistentDataPath);
    }
    [MenuItem("Assets/Open Build Folder", priority = 1101)]
    private static void OpenBuildFolder()
    {
        var projectFolder = Directory.GetParent(Application.dataPath).ToString();
        var buildFolder = Path.Combine(projectFolder, "Builds");
        if (!Directory.Exists(buildFolder))
        {
            Directory.CreateDirectory(buildFolder);
        }
        Process.Start(buildFolder);
    }
}
#endif