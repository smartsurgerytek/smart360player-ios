using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Eason/MVC/Controller")]
public class MonoWrappedController : MonoWrapper<IController>, IController
{
    [Button("Execute")]
    void IController.Execute()
    {
        innerData.Execute();
    }
}

public static class GlobalContext
{
    private static readonly Dictionary<string, object> _map = new Dictionary<string, object>();
    public static T Get<T>(string key)
    {
        if (!(_map[key] is T)) throw new KeyNotFoundException();
        return (T)_map[key];
    }
    public static void Set<T>(string key, T value)
    {
        _map[key] = value;
    }
}
public class GlobalContextAccessor<T> : IAccessor<T>
{
    [OdinSerialize] private IAccessor<string> _key;

    T IReader<T>.Read()
    {
        var key = _key.Read();
        return GlobalContext.Get<T>(key);
    }

    void IWriter<T>.Write(T value)
    {
        var key = _key.Read();
        GlobalContext.Set(key, value);
    }
}