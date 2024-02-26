using Sirenix.Serialization;

public class ArrayCursorAccessor<T> : IAccessor<T>
{
    [OdinSerialize] private IAccessor<T[]> _array;
    [OdinSerialize] private IReader<int> _cursor;

    T IReader<T>.Read()
    {
        var array = _array.Read();
        var cursor = _cursor.Read();
        return array[cursor];
    }

    void IWriter<T>.Write(T value)
    {
        var array = _array.Read();
        var cursor = _cursor.Read();
        array[cursor] = value;
        _array.Write(array);
    }
}
