using Sirenix.Serialization;
using System.Linq;

public class DefaultArrayAccessor<T> : IArrayAccessor<T>
{
    [OdinSerialize] T[] _innerData;
    int ICountProvider.count => _innerData.Length;

    T[] IReader<T[]>.Read()
    {
        return _innerData?.ToArray();
    }

    void IWriter<T[]>.Write(T[] value)
    {
        _innerData = value?.ToArray();
    }
}
