using Sirenix.Serialization;
using System;
using System.Collections.Generic;

[Serializable]
public class DefaultDictionaryAccessor<TKey, TValue> : IDictionaryAccessor<TKey, TValue>
{
    [OdinSerialize] private IDictionary<TKey, TValue> _value;
    TValue IDictionaryReader<TKey, TValue>.Read(TKey key)
    {
        return _value[key];
    }

    IDictionary<TKey, TValue> IReader<IDictionary<TKey, TValue>>.Read()
    {
        return _value;
    }

    void IDictionaryWriter<TKey, TValue>.Write(TKey key, TValue value)
    {
        _value[key] = value;
    }

    void IWriter<IDictionary<TKey, TValue>>.Write(IDictionary<TKey, TValue> value)
    {
        _value = value;
    }
}