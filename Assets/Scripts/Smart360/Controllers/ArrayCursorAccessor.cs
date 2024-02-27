
using Sirenix.Serialization;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
public class ArrayCursorAccessor<T> : IAccessor<T>
{
    [OdinSerialize] private IAccessor<T[]> _array;
    [OdinSerialize] private IReader<int> _cursor;

    T IReader<T>.Read()
    {
        var array = _array.Read();
        var cursor = _cursor.Read();
        return array[cursor];
    }

    void IWriter<T>.Write(T value)
    {
        var array = _array.Read();
        var cursor = _cursor.Read();
        array[cursor] = value;
        _array.Write(array);
    }
    void IWriter.Write(object value)
    {
        ((IWriter<T>)this).Write(value);
    }
}
public class InitialTransformArrayReader : InitialArrayReader<Transform> { }
public class InitialArrayReader<T> : IArrayReader<T>
{
    [OdinSerialize] private IReader<int> _length;
    int ICountProvider.count => _length.Read();

    T[] IReader<T[]>.Read()
    {
        return new T[_length.Read()];
    }

}
public class Cleanup : IController
{
    [OdinSerialize] private IAccessor<Transform[]> _transforms;

    private Action<Object> Destroy => Application.isPlaying ? GameObject.Destroy : GameObject.DestroyImmediate;
    void IController.Execute()
    {
        var transforms = _transforms.Read();
        if (transforms == null) return;
        for (var i = 0; i < transforms.Length; i++)
        {
            if (!transforms[i])
            {
                transforms[i] = null; return;
            }
            Destroy(transforms[i].gameObject);
        }
    }
}

public class TransformChildrenAccessor : IArrayReader<Transform>
{
    [OdinSerialize] private IAccessor<Transform> _parent;
    int ICountProvider.count => _parent.Read().childCount;

    Transform[] IReader<Transform[]>.Read()
    {
        var count = ((ICountProvider)this).count;
        var parent = _parent.Read();

        var rt = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            rt[i] = parent.GetChild(i);
        }
        return rt;
    }
}