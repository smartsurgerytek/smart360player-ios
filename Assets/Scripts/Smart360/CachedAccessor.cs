using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

[Serializable]
public class CachedAccessor<T> : ICachedAccessor<T>
{
    [OdinSerialize] IAccessor<T> _source;
    [OdinSerialize] IAccessor<T> _target;

    public CachedAccessor(IAccessor<T> source, IAccessor<T> target)
    {
        _source = source;
        _target = target;
    }

    [Button("Save")]
    void ICachedWriter.Save()
    {
        _source.Write(_target.Read());
    }
    [Button("Load")]
    void ICachedReader.Load()
    {
        _target.Write(_source.Read());
    }

    T IReader<T>.Read()
    {
        return _target.Read();
    }

    void IWriter<T>.Write(T value)
    {
        _target.Write(value);
    }
}
public static class EasonJsonUtility
{
    public static T JsonDeepClone<T>(T obj)
    {
        return JsonUtility.FromJson<T>( JsonUtility.ToJson(obj));
    }
}