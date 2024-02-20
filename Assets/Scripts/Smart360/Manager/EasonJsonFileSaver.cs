using System;
using System.IO;
using UnityEngine;

[Serializable]
public class EasonJsonFileSaver<T> : ISaver<T>
{
    [SerializeField] private string _relativePath;

    public EasonJsonFileSaver(string relativePath)
    {
        _relativePath = relativePath;
    }

    private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath);
    public void Save(T data)
    {
        var json = JsonUtility.ToJson(data);
        File.WriteAllText(absolutePath, json); 
    }
}
