using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

[Serializable]
public class JsonFileArrayAccessor<T> : IArrayAccessor<T>
{

    [SerializeField] private IReader<string> _relativePath;

    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath?.Read() ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector] private bool isFileExist => File.Exists(absolutePath);

    int ICountProvider.count => ((IReader<T[]>)this).Read().Length;

    T[] IReader<T[]>.Read()
    {
        var json = File.ReadAllText(absolutePath);
        return JsonUtility.FromJson<Wrapper<T[]>>(json).value;
    }

    void IWriter<T[]>.Write(T[] value)
    {
        var json  =JsonUtility.ToJson(new Wrapper<T[]>(value)); 
        File.WriteAllText(absolutePath, json);
    }
}
