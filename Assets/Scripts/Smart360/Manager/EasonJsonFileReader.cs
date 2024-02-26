using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class EasonJsonFileReader<T> : IReader<T>
{
    [SerializeField] private string _relativePath;

    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    public EasonJsonFileReader(string relativePath)
    {
        _relativePath = relativePath;
    }

    T IReader<T>.Read()
    {
        var json = File.ReadAllText(absolutePath);
        return JsonUtility.FromJson<T>(json);
    }
}
