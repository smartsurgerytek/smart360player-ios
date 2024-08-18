using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Linq;
public class DefaultArrayAccessor<T> : IArrayAccessor<T>
{
    
    [OdinSerialize, TableList] T[] _innerData;
    int ICountProvider.count => _innerData.Length;

    T[] IReader<T[]>.Read()
    {
        return _innerData?.ToArray();
    }
    void IWriter<T[]>.Write(T[] value)
    {
        _innerData = value?.ToArray();
    }

    void IWriter.Write(object value)
    {
        ((IWriter<T>)this).Write((T)value);
    }
}
