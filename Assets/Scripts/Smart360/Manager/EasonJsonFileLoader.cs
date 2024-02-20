using System;
using System.IO;
using UnityEngine;

[Serializable]
public class EasonJsonFileLoader<T> : ILoader<T>
{
    [SerializeField] private string _relativePath;

    public EasonJsonFileLoader(string relativePath)
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
