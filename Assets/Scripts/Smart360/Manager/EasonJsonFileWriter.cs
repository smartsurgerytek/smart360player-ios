using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.ComponentModel;
using System.IO;
using UnityEngine;

[Serializable]
public class EasonJsonFileWriter<T> : IWriter<T>
{
    [SerializeField] private string _relativePath;
    public EasonJsonFileWriter(string relativePath)
    {
        _relativePath = relativePath;
    }
    private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath);
    void IWriter<T>.Write(T value)
    {
        var json = JsonUtility.ToJson(value);
        File.WriteAllText(absolutePath, json);
    }
}

[SerializeField]
public class CacheAccessor<T> : IAccessor<T>
{
    [OdinSerialize] private IAccessor<T> _innerAccessor;
    [OdinSerialize] private T _cache;

    T IReader<T>.Read()
    {
        return _cache = _innerAccessor.Read();
    }

    void IWriter<T>.Write(T value)
    {
        _cache = value;
    }
    [Button]
    public void Load()
    {
        _cache = _innerAccessor.Read();
    }
    [Button]
    public void Save()
    {
        _innerAccessor.Write(_cache);
    }
}