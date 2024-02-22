using Sirenix.Serialization;

public class FreeDictionaryAccessor<TKey, TValue> : DictionaryAccessor<TKey, TValue>
{
     [OdinSerialize] private IDictionaryAccessor<TKey, TValue> _innerAccessor;

    protected override IDictionaryAccessor<TKey, TValue> innerAccessor => _innerAccessor;
}
