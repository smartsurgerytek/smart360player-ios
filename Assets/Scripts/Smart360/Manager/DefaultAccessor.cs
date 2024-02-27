using Sirenix.Serialization;
using System;

[Serializable]
public class DefaultAccessor<T> : IAccessor<T>
{
    [OdinSerialize] T _value;
    T IReader<T>.Read()
    {
        return _value;
    }

    void IWriter<T>.Write(T value)
    {
        _value = value;
    }
    void IWriter.Write(object value)
    {
        ((IWriter<T>)this).Write((T)value);
    }
}

