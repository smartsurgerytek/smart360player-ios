public abstract class FreeDictionaryAccessor<TKey, TValue> : IDictionaryAccessor<TKey, TValue>
{
    IReader<TKey> _key;
    IDictionaryAccessor<TKey, TValue> _innerAccessor;
    TValue IDictionaryReader<TKey, TValue>.Read(TKey key)
    {
        return _innerAccessor.Read(key);
    }
    void IDictionaryWriter<TKey, TValue>.Write(TKey key, TValue value)
    {
        _innerAccessor.Write(key, value);
    }
    TValue IReader<TValue>.Read()
    {
        var key = _key.Read();
        return ((IDictionaryReader<TKey, TValue>)this).Read(key);
    }
    void IWriter<TValue>.Write(TValue value)
    {
        var key = _key.Read();
        ((IDictionaryWriter<TKey, TValue>)this).Write(key, value);
    }
}