using System.Linq;
using UnityEngine;
using System.IO;

[CreateAssetMenu(fileName = "File Manager", menuName = "Managers/File Manager")]
public class FileManager : ScriptableObject
{
    [SerializeField] private string _bundlesRoot;
    [SerializeField] private string _directFilesRoot;
    public string GetVideoPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, _directFilesRoot, fileName);
    }

}