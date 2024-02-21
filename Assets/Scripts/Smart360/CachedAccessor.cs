using Sirenix.Serialization;
using System;
using UnityEngine;

[Serializable]
public class CachedAccessor<T> : IAccessor<T>
{
    [OdinSerialize] IAccessor<T> _source;
    [OdinSerialize] IAccessor<T> _target;

    public void Save()
    {
        _source.Write(_target.Read());
    }
    public void Load()
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