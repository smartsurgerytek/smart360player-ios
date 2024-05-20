using Sirenix.Serialization;

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