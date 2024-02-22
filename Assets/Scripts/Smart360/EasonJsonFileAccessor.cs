using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;
[Serializable]
public class EasonJsonFileAccessor<T> : IAccessor<T>
{
    [SerializeField] private string _relativePath;
    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector] private bool isFileExist => File.Exists(absolutePath);
    T IReader<T>.Read()
    {
        IReader<T> reader = new EasonJsonFileReader<T>(absolutePath);
        return reader.Read();
    }

    void IWriter<T>.Write(T data)
    {
        IWriter<T> writer = new EasonJsonFileWriter<T>(absolutePath);
        writer.Write(data);
    }

}
