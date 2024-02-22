using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;

[Serializable]
public class EditionButtonInitializer : ISpawnInitializer<EditionButton>
{
     [OdinSerialize] private IReader<Edition[]> _editions;

    void ISpawnInitializer<EditionButton>.Initialize(EditionButton instance, int index)
    {
        var editions = _editions.Read();
        instance.index = index;
        instance.editionId = editions[index].id;
        instance.title = editions[index].englishName;
    }
}
public interface IDictionaryReader<TKey, TValue> : IReader<IDictionary<TKey,TValue>>
{
    TValue Read(TKey key);
}
public interface IDictionaryWriter<TKey, TValue> : IWriter<IDictionary<TKey, TValue>>
{
    void Write(TKey key, TValue value);
}
public interface IDictionaryAccessor<TKey, TValue> : IDictionaryReader<TKey, TValue>, IDictionaryWriter<TKey, TValue>, IAccessor<IDictionary<TKey, TValue>>
{

}
public class FreeScriptableDictionaryAccessor<TKey, TValue> : ScriptableDictionaryAccessor<TKey, TValue>
{
    [OdinSerialize] private IDictionaryAccessor<TKey, TValue> _innerAccessor;
    protected override IDictionaryAccessor<TKey, TValue> innerAccessor => _innerAccessor;
}
public abstract class ScriptableDictionaryAccessor<TKey, TValue> : SerializedScriptableObject, IDictionaryAccessor<TKey, TValue>
{
    protected abstract IDictionaryAccessor<TKey, TValue> innerAccessor { get; }
    
    TValue IDictionaryReader<TKey, TValue>.Read(TKey key)
    {
        return innerAccessor.Read(key);
    }


    IDictionary<TKey, TValue> IReader<IDictionary<TKey, TValue>>.Read()
    {
        return innerAccessor.Read();
    }

    void IDictionaryWriter<TKey, TValue>.Write(TKey key, TValue value)
    {
        innerAccessor.Write(key, value);
    }


    void IWriter<IDictionary<TKey, TValue>>.Write(IDictionary<TKey, TValue> value)
    {
        innerAccessor.Write(value);
    }
}


public abstract class DictionaryAccessor<TKey, TValue> :  IDictionaryAccessor<TKey, TValue>
{
    protected abstract IDictionaryAccessor<TKey, TValue> innerAccessor { get; }

    TValue IDictionaryReader<TKey, TValue>.Read(TKey key)
    {
        return innerAccessor.Read(key);
    }


    IDictionary<TKey, TValue> IReader<IDictionary<TKey, TValue>>.Read()
    {
        return innerAccessor.Read();
    }

    void IDictionaryWriter<TKey, TValue>.Write(TKey key, TValue value)
    {
        innerAccessor.Write(key, value);
    }


    void IWriter<IDictionary<TKey, TValue>>.Write(IDictionary<TKey, TValue> value)
    {
        innerAccessor.Write(value);
    }
}
