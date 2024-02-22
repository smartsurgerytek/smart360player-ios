using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.IO;
using UnityEngine;
[Serializable]
public class WrappedJsonFileWriter<T> : IWriter<T>
{
    [SerializeField] private string _relativePath;

    public WrappedJsonFileWriter(string relativePath)
    {
        _relativePath = relativePath;
    }

    private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath).Replace('/', Path.DirectorySeparatorChar);
    void IWriter<T>.Write(T value)
    {
        var json = JsonUtility.ToJson(new Wrapper<T>(value));
        File.WriteAllText(absolutePath, json);
    }
}
[Serializable]
public class WrappedJsonFileAccessor<T> : IAccessor<T>
{

    [SerializeField] private string _relativePath;

    public WrappedJsonFileAccessor(string relativePath)
    {
        _relativePath = relativePath;
    }

    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector] private bool isFileExist => File.Exists(absolutePath);
    T IReader<T>.Read()
    {
        IReader<T> reader = new WrappedJsonFileReader<T>(absolutePath);
        return reader.Read();
    }

    void IWriter<T>.Write(T data)
    {
        IWriter<T> writer = new WrappedJsonFileWriter<T>(absolutePath);
        writer.Write(data);
    }
}
[Serializable]
public class WrappedJsonFileReader<T> : IReader<T>
{
    [SerializeField] private string _relativePath;

    public WrappedJsonFileReader(string relativePath)
    {
        _relativePath = relativePath;
    }

    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);

    T IReader<T>.Read()
    {
        var json = File.ReadAllText(absolutePath);
        return JsonUtility.FromJson<Wrapper<T>>(json).value;
    }
}
[Serializable]
public class EasonJsonFileWriter<T> : IWriter<T>
{
    [SerializeField] private string _relativePath;
    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    public EasonJsonFileWriter(string relativePath)
    {
        _relativePath = relativePath;
    }
    void IWriter<T>.Write(T value)
    {
        var json = JsonUtility.ToJson(value);
        File.WriteAllText(absolutePath, json);
    }
}
