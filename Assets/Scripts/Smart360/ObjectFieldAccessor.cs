using Sirenix.OdinInspector;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public class ObjectMemberAccessor<T> : IAccessor<T>
{
    [SerializeField] private Object _target;
    [SerializeField] private string _propertyName;
    [Button]
    T IReader<T>.Read()
    {
        var rt = default(T);
        var members = _target.GetType().GetMember(_propertyName ,BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        var first = members.First();

        if (first is FieldInfo) rt = (T)((FieldInfo)first).GetValue(_target);
        if (first is PropertyInfo) rt = (T)((PropertyInfo)first).GetValue(_target);
        if (first is MethodInfo) rt = (T)((MethodInfo)first).Invoke(_target, null);
        return rt;
    }

    void IWriter<T>.Write(T value)
    {
    }
}
