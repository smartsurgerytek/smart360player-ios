using Sirenix.OdinInspector;
using System.IO;
using UnityEngine;

public class MonoFileToMonoAccessor<T> :  SerializedMonoBehaviour ,ICachedAccessor<T>
{
    [SerializeField] private string _relativePath;
    [SerializeField] private DefaultMonoAccessor<T> _model;
    [ShowInInspector] private string absolutePath => Path.Combine(Application.persistentDataPath, _relativePath ?? "")?.Replace('/', Path.DirectorySeparatorChar);
    [ShowInInspector] private bool exists => File.Exists(absolutePath);
    void ICachedReader<T>.Load()
    {
        var source = new WrappedJsonFileAccessor<T>(absolutePath);
        var target = new DefaultMonoAccessor<T>();
        ICachedAccessor<T> innerAccessor = new CachedAccessor<T>(source, target);
        innerAccessor.Load();
    }

    void ICachedWriter<T>.Save()
    {
        var source = new WrappedJsonFileAccessor<T>(absolutePath);
        var target = new DefaultMonoAccessor<T>();
        ICachedAccessor<T> innerAccessor = new CachedAccessor<T>(source, target);
        innerAccessor.Save();
    }
    T IReader<T>.Read()
    {
        var source = new WrappedJsonFileAccessor<T>(absolutePath);
        var target = new DefaultMonoAccessor<T>();
        ICachedAccessor<T> innerAccessor = new CachedAccessor<T>(source, target);
        return innerAccessor.Read();
    }


    void IWriter<T>.Write(T value)
    {
        var source = new WrappedJsonFileAccessor<T>(absolutePath);
        var target = new DefaultMonoAccessor<T>();
        ICachedAccessor<T> innerAccessor = new CachedAccessor<T>(source, target);
        innerAccessor.Write(value);
    }
}