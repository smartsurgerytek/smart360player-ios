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
    public void Execute()
    {
        ((IController)this).Execute();
    }
}

public static class GlobalContext
{
    private static readonly Dictionary<string, object> _map = new Dictionary<string, object>();
    public static bool TryGet<T>(string key, out T value)
    {
        value = default(T);
        try
        {
            var found = _map.TryGetValue(key, out var uncastValue);
            if (found && uncastValue is T)
            {
                value = (T)uncastValue;
                return true;
            }
        }
        catch
        {
            return false;
        }
        return false;
    }
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
    [OdinSerialize] private bool _throwError = true;
    T IReader<T>.Read()
    {
        var key = _key.Read();
        if (_throwError)
        {
            return GlobalContext.Get<T>(key);
        }
        else
        {
            var found= GlobalContext.TryGet<T>(key, out var value);
            if (!found) return default;
            return value;
        }
    }

    void IWriter<T>.Write(T value)
    {
        var key = _key.Read();
        GlobalContext.Set(key, value);
    }

    void IWriter.Write(object value)
    {
        ((IWriter<T>)this).Write((T)value);
    }
}