using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class Router<T> :IAccessor<T>
{
    [OdinSerialize] IWriter<T> _writer;
    [OdinSerialize] IReader<T> _reader;

    [Button] 
    private void Foward()
    {
        var value = _reader.Read();
        //Debug.Log(value);
        _writer.Write(value);
    }

    T IReader<T>.Read()
    {
        return _reader.Read();
    }

    void IWriter<T>.Write(T data)
    {
        _writer.Write(data);
    }
}
