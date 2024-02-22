using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

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
public interface IDictionaryReader<TKey, TValue> : IReader<TValue>
{
    TValue Read(TKey key);
}
public interface IDictionaryWriter<TKey, TValue> : IWriter<TValue>
{
    void Write(TKey key, TValue value);
}
public interface IDictionaryAccessor<TKey, TValue> : IDictionaryReader<TKey, TValue>, IDictionaryWriter<TKey, TValue>, IAccessor<TValue>
{

}
public class FreeScriptableDictionaryAccessor<TKey, TValue> : ScriptableDictionaryAccessor<TKey, TValue>
{
    [OdinSerialize] private IAccessor<TKey> _innerKey;
    [OdinSerialize] private IDictionaryAccessor<TKey, TValue> _innerAccessor;
    protected override TKey key { get => _innerKey.Read(); set => _innerKey.Write(value); }
    protected override IDictionaryAccessor<TKey, TValue> innerAccessor => _innerAccessor;
}
public abstract class ScriptableDictionaryAccessor<TKey, TValue> : SerializedScriptableObject, IDictionaryAccessor<TKey, TValue>
{
    protected abstract TKey key { get; set; }
    protected abstract IDictionaryAccessor<TKey, TValue> innerAccessor { get; }
    
    TValue IDictionaryReader<TKey, TValue>.Read(TKey key)
    {
        return innerAccessor.Read(key);
    }

    TValue IReader<TValue>.Read()
    {
        return innerAccessor.Read(this.key);
    }

    void IDictionaryWriter<TKey, TValue>.Write(TKey key, TValue value)
    {
        innerAccessor.Write(key, value);
    }

    void IWriter<TValue>.Write(TValue value)
    {
        innerAccessor.Write(this.key, value);
    }
}

public class StringStringFreeScriptableDictionaryAccessor : FreeScriptableDictionaryAccessor<string, string>
{

}

