using Sirenix.OdinInspector;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectFieldAccessor<T> : IAccessor<T>
{
    [SerializeField] private Object _target;
    [SerializeField] private string _propertyName;
    [Button]
    T IReader<T>.Read()
    {
        var field = _target.GetType().GetField(_propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        Debug.Log(field);
        return default;
    }

    void IWriter<T>.Write(T value)
    {
    }
}
