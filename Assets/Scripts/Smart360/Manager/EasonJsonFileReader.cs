using System;
using System.IO;
using UnityEngine;

[Serializable]
public class EasonJsonFileReader<T> : IReader<T>
{
    [SerializeField] private string _relativePath;

    public EasonJsonFileReader(string relativePath)
    {
        _relativePath = relativePath;
    }

    private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath);
    public T Load()
    {
        var json = File.ReadAllText(absolutePath);
        return JsonUtility.FromJson<T>(json);
    }
}
